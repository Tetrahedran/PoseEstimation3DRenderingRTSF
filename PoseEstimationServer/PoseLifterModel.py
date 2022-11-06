from mmpose.apis import init_pose_model
from enum import Enum
import os


class PoseLifterModelName(Enum):

    def __init__(self, config, checkpoint):
        self.config = config
        self.checkpoint = checkpoint

    pose_lift_video_lift_27Frame = "video_pose_lift_videopose3d_h36m_27frames_fullconv_supervised.py", "videopose_h36m_27frames_fullconv_supervised-fe8fbba9_20210527.pth"
    pose_lift_video_lift_243Frame = "video_pose_lift_videopose3d_h36m_243frames_fullconv_supervised.py", "videopose_h36m_243frames_fullconv_supervised-880bea25_20210527.pth"


class PoseLifterModel:

    @staticmethod
    def get(base: str, model_name: PoseLifterModelName, device='cuda:0'):
        config = os.path.join(base, model_name.config)
        checkpoint = os.path.join(base, model_name.checkpoint)
        return init_pose_model(config, checkpoint, device.lower())
