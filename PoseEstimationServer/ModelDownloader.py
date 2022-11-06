from mim import download

if __name__ == "__main__":
    mmposeModels = ['topdown_heatmap_vipnas_mbv3_coco_256x192', "topdown_heatmap_vipnas_res50_coco_256x192", "topdown_heatmap_shufflenetv1_coco_384x288",
                    "topdown_heatmap_mspn50_coco_256x192", "topdown_heatmap_mobilenetv2_coco_256x192", "topdown_heatmap_litehrnet_30_coco_256x192"]
    download("mmpose", mmposeModels, dest_root="checkpoints")
