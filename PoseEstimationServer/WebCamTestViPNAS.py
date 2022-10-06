from mmpose.apis import (init_pose_model, inference_bottom_up_pose_model, inference_top_down_pose_model, vis_pose_result, vis_pose_tracking_result)
from mmpose.apis.webcam import WebcamExecutor
from mmpose.apis.webcam.nodes import model_nodes
from mmcv import Config, DictAction
import cv2
import numpy as np

import time


class WebCamPoseInference:

    model = None

    def __init__(self, model, confidence=0.75):
        self.model_name = model
        self.vid = cv2.VideoCapture(0)
        self.confidence = confidence

    def start(self):
        config_file, checkpoint_file = "", ""
        if self.model_name == "vipnas_mbv3":
            # ca. 13fps
            config_file = "checkpoints/topdown_heatmap_vipnas_mbv3_coco_256x192.py"
            checkpoint_file = "checkpoints/vipnas_mbv3_coco_256x192-7018731a_20211122.pth"
        elif self.model_name == "vipnas_res50":
            # ca. 21fps
            config_file = "checkpoints/topdown_heatmap_vipnas_res50_coco_256x192.py"
            checkpoint_file = "checkpoints/vipnas_res50_coco_256x192-cc43b466_20210624.pth"
        elif self.model_name == "shufflev1":
            # ca. 24fps
            config_file = "checkpoints/topdown_heatmap_shufflenetv1_coco_384x288.py"
            checkpoint_file = "checkpoints/shufflenetv1_coco_384x288-b2930b24_20200804.pth"
        elif self.model_name == "mspn":
            # ca. 20fps
            config_file = "checkpoints/topdown_heatmap_mspn50_coco_256x192.py"
            checkpoint_file = "checkpoints/mspn50_coco_256x192-8fbfb5d0_20201123.pth"
        elif self.model_name == "mobilenetv2":
            # ca. 29fps
            config_file = "checkpoints/topdown_heatmap_mobilenetv2_coco_256x192.py"
            checkpoint_file = "checkpoints/mobilenetv2_coco_256x192-d1e58e7b_20200727.pth"
        elif self.model_name == "litehr":
            # ca. 6fps
            config_file = "checkpoints/topdown_heatmap_litehrnet_30_coco_256x192.py"
            checkpoint_file = "checkpoints/litehrnet30_coco_256x192-4176555b_20210626.pth"
        self.model = init_pose_model(config_file, checkpoint_file)

    def infer(self):
        if self.model is None:
            raise Exception("Model not created")
        ret, frame = self.vid.read()
        #frame = cv2.flip(frame, 0)
        pose_results, _ = inference_top_down_pose_model(self.model, frame)
        self.show_results(frame, pose_results)
        return self.reformat_results(pose_results, frame.shape)

    def show_results(self, img, results):
        for person in results:
            for point in person["keypoints"]:
                confidence = point[2]
                if confidence > self.confidence:
                    cv2.drawMarker(img, (int(point[0]), int(point[1])), (0, 255, 0), markerType=cv2.MARKER_CROSS)
        cv2.imshow("frame", img)
        if cv2.waitKey(1) & 0xff == ord("q"):
            return

    def transform_result(self, raw, x_offset, y_offset, div):
        #TODO add z-value
        new = np.array((x_offset - raw[0], -raw[1] + y_offset, 0)) / div
        return new.tolist()

    def infer_point(self, first, second, x_offset, y_offset, div):
        calc = self.calculate_point(first, second)
        return self.transform_result(calc, x_offset, y_offset, div)

    def calculate_point(self, first, second):
        new = (np.array((first[0], first[1], 0)) + np.array((second[0], second[1], 0))) / 2
        return new

    def set_extremity(self, points, bone_indices, x_offset, y_offset, div):
        ret = {}
        for key in bone_indices:
            bone_index = bone_indices[key]
            if points[bone_index][2] >= self.confidence:
                ret[key] = self.transform_result(points[bone_index], x_offset, y_offset, div)
        return ret

    def reformat_results(self, pose_results, img_shape):
        x_offset = img_shape[1] / 2
        y_offset = img_shape[0] / 2
        points = pose_results[0]["keypoints"]
        div = img_shape[1]
        ret = {}

        # Left Arm Chain
        ret.update(self.set_extremity(points,
                                      {"LeftUpperArm": 5, "LeftLowerArm": 7, "LeftHand": 9, "LeftFingers": 9},
                                      x_offset, y_offset, div))
        # Right Arm Chain
        """ret.update(self.set_extremity(points,
                                      {"RightUpperArm": 6, "RightLowerArm": 8, "RightHand": 10, "RightFingers": 10},
                                      x_offset, y_offset, div))"""
        return ret


if __name__ == '__main__':
    wc = WebCamPoseInference("mobilenetv2")
    wc.start()
    while True:
        print(wc.infer())

