from mim import download

if __name__ == "__main__":
    mmposeModels = ['topdown_heatmap_vipnas_mbv3_coco_256x192', "topdown_heatmap_vipnas_res50_coco_256x192", "topdown_heatmap_shufflenetv1_coco_384x288",
                    "topdown_heatmap_mspn50_coco_256x192", "topdown_heatmap_mobilenetv2_coco_256x192", "topdown_heatmap_litehrnet_30_coco_256x192"]
    download("mmpose", mmposeModels, dest_root="checkpoints")
    lifterModels = ["video_pose_lift_videopose3d_h36m_27frames_fullconv_supervised", "video_pose_lift_videopose3d_h36m_243frames_fullconv_supervised"]
    download("mmpose", lifterModels, dest_root="checkpoints")
    detModels = ["faster_rcnn_r50_fpn_1x_coco", "ssd512_coco", "yolov3_d53_320_273e_coco", "yolov3_mobilenetv2_320_300e_coco",
                 "yolox_tiny_8x8_300e_coco", "ssdlite_mobilenetv2_scratch_600e_coco"]
    download("mmdet", detModels, dest_root="checkpoints")
