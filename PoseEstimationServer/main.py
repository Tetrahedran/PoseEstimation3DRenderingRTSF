# This is a sample Python script.

# Press Umschalt+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.

from flask import Flask, json
from threading import Thread, Lock
from WebCamTestViPNAS import WebCamPoseInference
from time import time


def perform_pose_estimation():
    """
    Loop for performing continuous pose estimation
    """
    wc = WebCamPoseInference("vipnas_res50", .75)
    wc.start()
    fps = []
    while True:
        start = time()
        estimate = wc.infer()
        end = time()
        set_pose(estimate)
        loc_fps = 1 / (end - start)
        fps.append(loc_fps)
        if len(fps) >= 10:
            avg_fps = sum(fps) / len(fps)
            print(f"Average {avg_fps} fps")
            fps = []


def create_pose_detection_thread():
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


@api.get("/test")
def get_hello_world():
    return json.dumps(get_pose())


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    pose_thread = create_pose_detection_thread()
    pose_thread.start()
    api.run()

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
