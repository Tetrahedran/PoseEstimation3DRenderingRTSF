from enum import Enum
import os
from mmpose.apis import init_pose_model


class PoseEstimationModelName(Enum):

    def __init__(self, config: str, checkpoint: str):
        self.config = config
        self.checkpoint = checkpoint

    # ca 13 fps
    vipnas_mbv3 = "topdown_heatmap_vipnas_mbv3_coco_256x192.py", "vipnas_mbv3_coco_256x192-7018731a_20211122.pth"
    # ca 21 fps
    vipnas_res50 = "topdown_heatmap_vipnas_res50_coco_256x192.py", "vipnas_res50_coco_256x192-cc43b466_20210624.pth"
    # ca 24 fps
    shufflev1 = "topdown_heatmap_shufflenetv1_coco_384x288.py", "shufflenetv1_coco_384x288-b2930b24_20200804.pth"
    # ca 20 fps
    mspn = "topdown_heatmap_mspn50_coco_256x192.py", "mspn50_coco_256x192-8fbfb5d0_20201123.pth"
    # ca 29 fps
    mobilenetv2 = "topdown_heatmap_mobilenetv2_coco_256x192.py", "mobilenetv2_coco_256x192-d1e58e7b_20200727.pth"
    # ca 6 fps
    litehr = "topdown_heatmap_litehrnet_30_coco_256x192.py", "litehrnet30_coco_256x192-4176555b_20210626.pth"


class PoseEstimationModel:

    @staticmethod
    def get(base: str, model_name: PoseEstimationModelName, device="cuda:0"):
        config = os.path.join(base, model_name.config)
        checkpoint = os.path.join(base, model_name.checkpoint)
        return init_pose_model(config, checkpoint, device.lower())
