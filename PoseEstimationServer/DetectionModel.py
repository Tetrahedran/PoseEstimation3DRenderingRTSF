from enum import Enum
from mmdet.apis import init_detector
import os


class DetectionModelName(Enum):

    def __init__(self, config, checkpoint):
        self.config = config
        self.checkpoint = checkpoint

    # ca 3 fps
    faster_rcnn_r50 = "faster_rcnn_r50_fpn_1x_coco.py", "faster_rcnn_r50_fpn_1x_coco_20200130-047c8118.pth"
    # ca 6 fps
    ssd512 = "ssd512_coco.py", "ssd512_coco_20210803_022849-0a47a1ca.pth"
    # ca 22 fps
    yolov3 = "yolov3_d53_320_273e_coco.py", "yolov3_d53_320_273e_coco-421362b6.pth"
    # ca 30 fps
    yolov3_mobiv2 = "yolov3_mobilenetv2_320_300e_coco.py", "yolov3_mobilenetv2_320_300e_coco_20210719_215349-d18dff72.pth"
    # ca 35 - 50 fps
    yolox_tiny = "yolox_tiny_8x8_300e_coco.py", "yolox_tiny_8x8_300e_coco_20211124_171234-b4047906.pth"
    # ca 30 - 35 fps
    ssd_mobilev2 = "ssdlite_mobilenetv2_scratch_600e_coco.py", "ssdlite_mobilenetv2_scratch_600e_coco_20210629_110627-974d9307.pth"


class DetectionModel:

    @staticmethod
    def get(base: str, model_name: DetectionModelName, device="cuda:0"):
        config = os.path.join(base, model_name.config)
        checkpoint = os.path.join(base, model_name.checkpoint)
        return init_detector(config, checkpoint, device.lower())
