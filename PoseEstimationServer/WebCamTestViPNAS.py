from mmpose.apis import inference_top_down_pose_model

import cv2
import numpy as np
from typing import Dict
from PoseEstimationModel import PoseEstimationModelName, PoseEstimationModel

import KalmanFilter


class WebCamPoseInference:
    """
    Wrapper class for performing human pose estimation on webcam images
    """

    model = None

    def __init__(self, model_name: PoseEstimationModelName, confidence=0.75, video_id=0):
        """
        Initializer

        :param model_name: name of the pose estimation model to be used for pose estimation.
                            Has to be downloaded in advance
        :param confidence: confidence threshold for pose estimation results
        :param video_id: id of the video device to be used
        """
        self.model_name = model_name
        self.vid = cv2.VideoCapture(video_id)
        self.confidence = confidence
        self.filters = []
        for i in range(17):
            self.filters.append(KalmanFilter.KalmanFilter())

    def start(self):
        """
        Setup method for generating the pose estimation model defined by self.model_name.
        Has to be called before performing inference

        :raises Exception: if self.model_name is not matching any of the predefined model names
        """
        self.model = PoseEstimationModel.get("checkpoints", self.model_name)

    def infer(self) -> dict:
        """
        Performs one cycle of webcam image pose estimation. Takes one image from webcam,
        performs pose estimation on that image and returns reformatted results

        :return: a dict mapping key point names to reformatted 3D-coordinates.
        Only includes points with pose estimation confidence above self.confidence
        :raises Exception: if no pose estimation model has been created beforehand
        """
        if self.model is None:
            raise Exception("pose inference model has not been created")
        ret, frame = self.vid.read()
        #frame = cv2.flip(frame, 0)
        pose_results, _ = inference_top_down_pose_model(self.model, frame)
        self.show_results(frame, pose_results)
        return self.reformat_results(pose_results, frame.shape)

    def show_results(self, img, results):
        """
        Shows result of pose estimation. shows only results with confidence above self.confidence

        :param img: image used for pose estimation
        :param results: results from pose estimation
        """
        for i in range(len(results[0]["keypoints"])):
            point = results[0]["keypoints"][i]
            confidence = point[2]
            if confidence > self.confidence:
                cv2.drawMarker(img, (int(point[0]), int(point[1])), (0, 255, 0), markerType=cv2.MARKER_CROSS)
                filtered_point, p = self.filters[i].filter((point[0], point[1], 0))
                filtered_point = filtered_point.flatten().tolist()
                print(filtered_point)
                cv2.drawMarker(img, (int(filtered_point[0]), int(filtered_point[1])), (255,0,0), markerType=cv2.MARKER_TILTED_CROSS)
        cv2.imshow("frame", img)
        if cv2.waitKey(1) & 0xff == ord("q"):
            return

    def transform_result(self, raw, img_shape: tuple, div: float) -> list:
        """
        Transforms the coordinates of a point from image coordinate system (as used by pose estimation),
        whose origin is in the left upper corner of the image and presumably has an inverted y-axis,
        to coordinate system used in Unity, whose origin is in the middle bottom part of the generated image

        :param raw: raw vector of one key point generated by pose estimation
        :param img_shape: shape of the image used for pose estimation
        :param div: uniform scaling value to reduce the length of the output vector
        :param div: uniform scaling value to reduce the length of the output vector
                    (raw normally in range [0,x*100] depending on size of image used for inference,
                    but Unity uses coordinates to actual meters)
        :return: The transformed point where the x-value has been offset by the half image width
                and the y-value has been inverted and offset by the half image height
        """
        x_offset = img_shape[1] / 2
        y_offset = img_shape[0] * 1.2
        #TODO add z-value
        new = np.array((x_offset - raw[0], -raw[1] + y_offset, 0)) / div
        return new.tolist()

    def calculate_point_between(self, first, second):
        """
        Calculates a new point that is between first and second.
        Currently this method assumes 2D-style input vectors.

        :param first: 2D vector to first point
        :param second: 2D vector to Second point
        :return: a 2D vector pointing to the point between first and second
        #TODO extend to 3D points
        """
        new = (np.array((first[0], first[1], 0)) + np.array((second[0], second[1], 0))) / 2
        return new

    def set_extremity(self, points: list, bone_indices: dict, img_shape: tuple, div) -> dict:
        """
        Wrapper for extracting key points for human extremities.
        Works only if there are no intermediate points that have to be calculated from already existing points

        :param points: list of all key points from pose estimation
        :param bone_indices: dict that maps bone name to corresponding index in points
        :param img_shape: shape of the image used for pose estimation
        :param div: uniform scaling value to reduce the length of the output vector
                    (raw normally in range [0,x*100] depending on size of image used for inference,
                    but Unity uses coordinates to actual meters)
        :return: a dict that maps the bone name to its transformed coordinate.
                Only includes points with pose estimation confidence above self.confidence
        """
        ret = {}
        for key in bone_indices:
            bone_index = bone_indices[key]
            if points[bone_index][2] >= self.confidence:
                ret[key] = self.transform_result(points[bone_index], img_shape, div)
        return ret

    def get_spinal_chain(self, points, is_between: Dict[str, tuple], img_shape: tuple, div) -> dict:
        ret = {}
        for key in is_between:
            calc_from = is_between[key]
            if len(calc_from) == 2:
                first = calc_from[0]
                second = calc_from[1]
                if isinstance(first, int) and isinstance(second, int):
                    first = points[first]
                    second = points[second]
                    if (first[2] >= self.confidence) and (second[2] >= self.confidence):
                        point_between = self.calculate_point_between(first, second)
                        ret[key] = self.transform_result(point_between, img_shape, div)
                elif isinstance(first, str) and isinstance(second, str):
                    if (first in ret) and (second in ret):
                        ret[key] = list(self.calculate_point_between(ret[first], ret[second]))
            else:
                raise Exception("Currently not supported")

        return ret

    def reformat_results(self, pose_results, img_shape) -> dict:
        """
        Reformats results from pose estimation. Sorts out points with to low confidence,
        changes coordinate system from image coordinate system to Unity coordinate system
        (see :py:func:`WebCamPoseInference.transform_result`)

        :param pose_results: raw results from pose estimation
        :param img_shape: shape of the image used for pose estimation
        :return: a dict mapping the bone names to the reformated coordinates.
                Only include points with confidence above self.confidence
        """
        points = pose_results[0]["keypoints"]
        div = img_shape[1]
        ret = {}

        # Left Arm Chain
        ret.update(self.set_extremity(points,
                                      {"LeftUpperArm": 5, "LeftLowerArm": 7, "LeftHand": 9, "LeftFingers": 9},
                                      img_shape, div))
        # Right Arm Chain
        ret.update(self.set_extremity(points,
                                      {"RightUpperArm": 6, "RightLowerArm": 8, "RightHand": 10, "RightFingers": 10},
                                      img_shape, div))
        # Left Leg Chain
        ret.update(self.set_extremity(points,
                                      {"LeftUpperLeg": 11, "LeftLowerLeg": 13, "LeftFoot": 15, "LeftToes": 15},
                                      img_shape, div))

        # Right Leg Chain
        ret.update(self.set_extremity(points,
                                      {"RightUpperLeg": 12, "RightLowerLeg": 14, "RightFoot": 16, "RightToes": 16},
                                      img_shape, div))

        # Spinal chain
        ret.update(self.get_spinal_chain(points,
                                         {"Hip": (11, 12), "Neck": (5, 6), "Spine": ("Hip", "Neck"),
                                          "Head": (1, 2)},
                                         img_shape, div))

        print(ret)

        return ret


if __name__ == '__main__':
    wc = WebCamPoseInference("mobilenetv2")
    wc.start()
    while True:
        print(wc.infer())

