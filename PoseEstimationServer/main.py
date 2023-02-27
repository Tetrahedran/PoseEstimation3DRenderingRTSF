# This is a sample Python script.

# Press Umschalt+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.

from flask import Flask, json
from threading import Thread, Lock
from Pose_Estimation_2D import WebCamPoseInference
from time import time
from PoseEstimationModel import PoseEstimationModelName
from Pose_Estimation_3D import WebCam3DPoseEstimation
from DetectionModel import DetectionModelName
from PoseLifterModel import PoseLifterModelName
import cv2

pe_estimator = None


def perform_pose_estimation():
    """
    Loop for performing continuous pose estimation
    """
    base = "checkpoints"
    global pe_estimator

    vid = cv2.VideoCapture(0)
    fps = []
    while True:
        start = time()
        _, frame = vid.read()
        estimate = pe_estimator.infer(frame, bypass_det=False)
        end = time()
        cv2.imshow("frame", frame)
        if cv2.waitKey(1) & 0xff == ord("q"):
            return
        if len(estimate) > 0:
            set_pose(estimate[0])
        loc_fps = 1 / (end - start)
        fps.append(loc_fps)
        if len(fps) >= 10:
            avg_fps = sum(fps) / len(fps)
            print(f"Average {avg_fps} fps")
            """
            with open('Speed_OR_on_Smooth_off.txt', 'a') as f:
                f.write(f"{avg_fps},")
            """
            fps = []


def create_pose_estimation_thread():
    """
    Creates a separate thread to execute continuous pose estimation

    :return: a thread that continuously executes pose estimation on webcam images
    """
    x = Thread(target=perform_pose_estimation, daemon=True)
    return x


# API section
api = Flask(__name__)
pose_lock = Lock()
pose = {
        "Hips": [0, .1, 0],
        "Spine": [0, 0.25, 0],
        "Neck": [0, 0.5, 0],
        "Head": [0, .75, 0],
        "Nose": [0, .75, .25],
        "LeftUpperArm": [0, .5, 0],
        "LeftLowerArm": [-0.25, .5, 0],
        "LeftHand": [-0.5, .5, 0],
        "LeftFingers": [-0.75, .5, 0],
        "RightUpperArm": [0, .5, 0],
        "RightLowerArm": [0.25, .5, 0],
        "RightHand": [0.5, .5, 0],
        "RightFingers": [0.75, .5, 0],
        "LeftUpperLeg": [-.1, -.1, 0],
        "LeftLowerLeg": [-.1, -.25, 0],
        "LeftFoot": [-.1, -.5, 0],
        "LeftToes": [-.1, -.75, 0],
        "RightUpperLeg": [.1, -.1, 0],
        "RightLowerLeg": [.1, -.25, 0],
        "RightFoot": [.1, -.5, 0],
        "RightToes": [.1, -.75, 0]
    }


def get_pose() -> dict:
    """
    Get current pose dict
    :return: the current poses
    """
    pose_lock.acquire()
    try:
        return pose
    finally:
        pose_lock.release()


def set_pose(new_pose: dict):
    """
    Override entries in pose with new_pose
    :param new_pose: dict that maps bone names to new positions
    """
    pose_lock.acquire()
    try:
        for key in new_pose:
            pose[key] = new_pose[key]
    finally:
        pose_lock.release()


@api.get("/estimation")
def get_hello_world():
    return json.dumps(get_pose())


@api.get("/available")
def get_available_networks():
    return json.jsonify(list(range(0, len(networks))))


@api.post("/switch/<networkID>")
def switch_network(networkID):
    network = networks[int(networkID)]
    pl_nw = network[1]
    pe_nw = network[0]
    pe_estimator.switch_pl_network(pl_nw)
    pe_estimator.switch_pe_network(pe_nw)
    """
    with open('Speed_OR_on_Smooth_off.txt', 'a') as f:
        f.write('\n')
        f.write(f"{networkID}:")
    """
    return json.jsonify(200)


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    networks = [(PoseEstimationModelName.vipnas_res50, PoseLifterModelName.pose_lift_video_lift_27Frame),
                (PoseEstimationModelName.vipnas_mbv3, PoseLifterModelName.pose_lift_video_lift_27Frame),
                (PoseEstimationModelName.mobilenetv2, PoseLifterModelName.pose_lift_video_lift_27Frame),
                (PoseEstimationModelName.mspn, PoseLifterModelName.pose_lift_video_lift_27Frame),
                (PoseEstimationModelName.litehr, PoseLifterModelName.pose_lift_video_lift_27Frame),
                (PoseEstimationModelName.vipnas_res50, PoseLifterModelName.pose_lift_video_lift_243Frame),
                (PoseEstimationModelName.vipnas_mbv3, PoseLifterModelName.pose_lift_video_lift_243Frame),
                (PoseEstimationModelName.mobilenetv2, PoseLifterModelName.pose_lift_video_lift_243Frame),
                (PoseEstimationModelName.mspn, PoseLifterModelName.pose_lift_video_lift_243Frame),
                (PoseEstimationModelName.litehr, PoseLifterModelName.pose_lift_video_lift_243Frame)
                ]
    pe_estimator = WebCam3DPoseEstimation("checkpoints", DetectionModelName.yolox_tiny,
                                          PoseEstimationModelName.vipnas_res50,
                                          PoseLifterModelName.pose_lift_video_lift_27Frame, use_smoothing=False)
    pose_thread = create_pose_estimation_thread()
    pose_thread.start()
    api.run()

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
