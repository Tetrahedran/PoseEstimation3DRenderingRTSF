import mmcv
from mmpose.apis import process_mmdet_results, inference_top_down_pose_model, extract_pose_sequence, \
    inference_pose_lifter_model, get_track_id
from mmpose.datasets import DatasetInfo
from mmpose.models import TopDown, PoseLifter
from mmdet.apis import inference_detector
from time import time
from DatasetKonverter import DatasetConverter
import copy
import cv2
import warnings
import numpy as np
from concurrent.futures import ThreadPoolExecutor

from PoseEstimationModel import PoseEstimationModelName, PoseEstimationModel
from DetectionModel import DetectionModelName, DetectionModel
from PoseLifterModel import PoseLifterModel, PoseLifterModelName

from KalmanFilter import KalmanFilter

import timeit


class WebCam3DPoseEstimation:
    """
    Code based on https://github.com/open-mmlab/mmpose/blob/master/demo/body3d_two_stage_video_demo.py provided by MMPose
    """

    def __init__(self, base: str, det_name: DetectionModelName, pe_name: PoseEstimationModelName,
                 pl_name: PoseLifterModelName, rebase_keypoint_height=True,
                 norm_pose_2d=False, device='cuda:0', det_cat_id=1,
                 bbox_thr=0.9, use_smoothing=False):

        self.base = base
        self.device = device.lower()
        self.rebase_keypoint_height = rebase_keypoint_height
        self.norm_pose_2d = norm_pose_2d
        self.det_cat_id = det_cat_id
        self.bbox_thr = bbox_thr
        self.use_smoothing = use_smoothing

        self.Smoother = [KalmanFilter() for _ in range(21)]

        self.ds_converter = DatasetConverter()

        self.detector = DetectionModel.get(base, det_name, self.device)

        self.pose_det_model = PoseEstimationModel.get(base, pe_name, self.device)

        self.pose_lift_model = PoseLifterModel.get(base, pl_name, self.device)

        self.last_pose_det_results = []

        self.executor = ThreadPoolExecutor()

        if not isinstance(self.pose_det_model, TopDown):
            raise Exception('Only "TopDown" model is supported for the 1st stage (2D pose detection)')

        if not isinstance(self.pose_lift_model, PoseLifter):
            raise Exception('Only "PoseLifter" model is supported for the 2nd stage (2D-to-3D lifting)')

        self.pose_det_dataset = self.pose_det_model.cfg.data['test']['type']
        # get datasetinfo
        self.dataset_info = self.pose_det_model.cfg.data['test'].get('self.dataset_info', None)
        if self.dataset_info is None:
            warnings.warn(
                'Please set `self.dataset_info` in the config.'
                'Check https://github.com/open-mmlab/mmpose/pull/663 for details.',
                DeprecationWarning)
        else:
            self.dataset_info = DatasetInfo(self.dataset_info)

        self.pose_lift_dataset = self.pose_lift_model.cfg.data['test']['type']
        self.pose_lift_dataset_info = self.pose_lift_model.cfg.data['test'].get(
            'self.dataset_info', None)
        if self.pose_lift_dataset_info is None:
            warnings.warn(
                'Please set `self.dataset_info` in the config.'
                'Check https://github.com/open-mmlab/mmpose/pull/663 for details.',
                DeprecationWarning)
        else:
            self.pose_lift_dataset_info = DatasetInfo(self.pose_lift_dataset_info)

    def infer(self, img, bypass_det=False):
        pose_det_results_list = []
        pose_det_results = []

        # whether to return heatmap, optional
        return_heatmap = False

        # return the output of some desired layers, e.g. use ('backbone', ) to return backbone feature
        output_layer_names = None

        # test a single image, the resulting box is (x1, y1, x2, y2)
        if not bypass_det:
            mmdet_results = inference_detector(self.detector, img)
        else:
            mmdet_results = [[[0, 0, img.shape[0], img.shape[1], 1]]]

        # keep the person class bounding boxes.
        person_det_results = process_mmdet_results(mmdet_results, self.det_cat_id)

        pose_det_results, _ = inference_top_down_pose_model(self.pose_det_model, img,
                                                            person_det_results,
                                                            bbox_thr=self.bbox_thr,
                                                            format='xyxy',
                                                            dataset=self.pose_det_dataset,
                                                            dataset_info=self.dataset_info,
                                                            return_heatmap=return_heatmap,
                                                            outputs=output_layer_names)

        # get track id for each person instance
        pose_det_results, next_id = get_track_id(
            pose_det_results,
            [],
            0,
            use_oks=False,
            tracking_thr=0.3)

        # convert keypoint definition
        for res in pose_det_results:
            keypoints = res['keypoints']
            res['keypoints'] = self.ds_converter.convert_keypoint_definition(keypoints,
                                self.pose_det_dataset, self.pose_lift_dataset)

        pose_det_results_list.append(copy.deepcopy(pose_det_results))

        # load temporal padding config from model.data_cfg
        if hasattr(self.pose_lift_model.cfg, 'test_data_cfg'):
            data_cfg = self.pose_lift_model.cfg.test_data_cfg
        else:
            data_cfg = self.pose_lift_model.cfg.data_cfg

        results = {}

        for res in pose_det_results_list:
            # self.last_pose_det_results.append(res)
            self.last_pose_det_results.insert(0, res)

        while len(self.last_pose_det_results) > data_cfg.seq_len:
            # self.last_pose_det_results.pop(0)
            self.last_pose_det_results.pop(-1)

        for i, pose_det_results in enumerate(mmcv.track_iter_progress(pose_det_results)):
            pose_results_2d = extract_pose_sequence(
                self.last_pose_det_results,
                frame_idx=0,  # len(self.last_pose_det_results) - 1,
                causal=data_cfg.causal,
                seq_len=data_cfg.seq_len,
                step=data_cfg.seq_frame_interval)

            # 2D-to-3D pose lifting
            pose_lift_results = inference_pose_lifter_model(
                self.pose_lift_model,
                pose_results_2d=pose_results_2d,
                dataset=self.pose_lift_dataset,
                dataset_info=self.pose_lift_dataset_info,
                with_track_id=True,
                image_size=img.shape[:-1],
                norm_pose_2d=self.norm_pose_2d)

            pose_lift_results_vis = []
            for idx, res in enumerate(pose_lift_results):
                keypoints_3d = res['keypoints_3d']
                # exchange y,z-axis, and then reverse the direction of x,z-axis
                # scale down
                # x = 0, y = 1, z = 2
                keypoints_3d = keypoints_3d[..., [0, 1, 2]]
                keypoints_3d[..., 0] = -keypoints_3d[..., 0]
                keypoints_3d[..., 2] = -keypoints_3d[..., 2]
                # reverse y-axis
                keypoints_3d[..., 1] = -keypoints_3d[..., 1]
                # rebase height (z-axis)
                if self.rebase_keypoint_height:
                    keypoints_3d[..., 2] -= np.min(
                        keypoints_3d[..., 2], axis=-1, keepdims=True)
                res['keypoints_3d'] = keypoints_3d

            kp_3d = pose_lift_results[i]["keypoints_3d"]

            leftFingers = kp_3d[13] + (kp_3d[13] - kp_3d[12])
            rightFingers = kp_3d[16] + (kp_3d[16] - kp_3d[15])

            kp_dict = {
                "Hips": kp_3d[0].astype("float").tolist(),
                "RightUpperLeg": kp_3d[1].astype("float").tolist(),
                "RightLowerLeg": kp_3d[2].astype("float").tolist(),
                "RightFoot": kp_3d[3].astype("float").tolist(),
                "RightToes": kp_3d[3].astype("float").tolist(),
                "LeftUpperLeg": kp_3d[4].astype("float").tolist(),
                "LeftLowerLeg": kp_3d[5].astype("float").tolist(),
                "LeftFoot": kp_3d[6].astype("float").tolist(),
                "LeftToes": kp_3d[6].astype("float").tolist(),
                "Spine": kp_3d[7].astype("float").tolist(),
                "Neck": kp_3d[8].astype("float").tolist(),
                "Head": kp_3d[10].astype("float").tolist(),
                "Nose": kp_3d[9].astype("float").tolist(),
                "LeftUpperArm": kp_3d[11].astype("float").tolist(),
                "LeftLowerArm": kp_3d[12].astype("float").tolist(),
                "LeftHand": kp_3d[13].astype("float").tolist(),
                "LeftFingers": leftFingers.astype("float").tolist(),
                "RightUpperArm": kp_3d[14].astype("float").tolist(),
                "RightLowerArm": kp_3d[15].astype("float").tolist(),
                "RightHand": kp_3d[16].astype("float").tolist(),
                "RightFingers": rightFingers.astype("float").tolist()
            }

            if self.use_smoothing:
                all_results = []
                all_inputs = []
                for j, key in enumerate(kp_dict):
                    in_data = (key, kp_dict[key], self.Smoother[j])
                    all_inputs.append(in_data)

                p_results = self.executor.map(self.perform_smoothing, all_inputs)

                for result in p_results:
                    all_results.append(result)

                for key, smoothed in all_results:
                    kp_dict[key] = smoothed

            results[i] = kp_dict

        return results

    def perform_smoothing(self, k_r_s_input):
        key = k_r_s_input[0]
        raw = k_r_s_input[1]
        smoother = k_r_s_input[2]

        x, p = smoother.filter(raw)
        x = x.flatten()
        x = [x[0], x[1], x[2]]
        return key, x

    def switch_pe_network(self, pe_network: PoseEstimationModelName):
        self.pose_det_model = PoseEstimationModel.get(self.base, pe_network, self.device)

    def switch_pl_network(self, pl_network: PoseLifterModelName):
        self.pose_lift_model = PoseLifterModel.get(self.base, pl_network, self.device)


if __name__ == '__main__':
    base = "checkpoints"

    pose_lifter_config = base + "/video_pose_lift_videopose3d_h36m_27frames_fullconv_supervised.py"
    pose_lifter_checkpoint = base + '/videopose_h36m_27frames_fullconv_supervised-fe8fbba9_20210527.pth'
    pe_estimator = WebCam3DPoseEstimation("checkpoints", DetectionModelName.yolox_tiny, PoseEstimationModelName.mobilenetv2,
                                          pose_lifter_config, pose_lifter_checkpoint)

    vid = cv2.VideoCapture(0)

    while True:
        ret, frame = vid.read()
        pe_estimator.infer(frame)




