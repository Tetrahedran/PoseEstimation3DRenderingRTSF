from mmpose.apis import (init_pose_model, inference_bottom_up_pose_model, inference_top_down_pose_model, vis_pose_result, vis_pose_tracking_result)
from mmpose.apis.webcam import WebcamExecutor
from mmpose.apis.webcam.nodes import model_nodes
from mmcv import Config, DictAction
import cv2

import time

if __name__ == '__main__':
    model = "vipnas_res50"
    config_file, checkpoint_file = "", ""
    if model == "vipnas_mbv3":
        # ca. 13fps
        config_file = "checkpoints/topdown_heatmap_vipnas_mbv3_coco_256x192.py"
        checkpoint_file = "checkpoints/vipnas_mbv3_coco_256x192-7018731a_20211122.pth"
    elif model == "vipnas_res50":
        # ca. 21fps
        config_file = "checkpoints/topdown_heatmap_vipnas_res50_coco_256x192.py"
        checkpoint_file = "checkpoints/vipnas_res50_coco_256x192-cc43b466_20210624.pth"
    elif model == "shufflev1":
        # ca. 24fps
        config_file = "checkpoints/topdown_heatmap_shufflenetv1_coco_384x288.py"
        checkpoint_file = "checkpoints/shufflenetv1_coco_384x288-b2930b24_20200804.pth"
    elif model == "mspn":
        # ca. 20fps
        config_file = "checkpoints/topdown_heatmap_mspn50_coco_256x192.py"
        checkpoint_file = "checkpoints/mspn50_coco_256x192-8fbfb5d0_20201123.pth"
    elif model == "mobilenetv2":
        # ca. 29fps
        config_file = "checkpoints/topdown_heatmap_mobilenetv2_coco_256x192.py"
        checkpoint_file = "checkpoints/mobilenetv2_coco_256x192-d1e58e7b_20200727.pth"
    elif model == "litehr":
        # ca. 6fps
        config_file = "checkpoints/topdown_heatmap_litehrnet_30_coco_256x192.py"
        checkpoint_file = "checkpoints/litehrnet30_coco_256x192-4176555b_20210626.pth"

    pose_model = init_pose_model(config_file, checkpoint_file)

    image_name = "demo/student-euclidean-vector-png-favpng-4T9235DY4cMQYfzR82sPLWnu5.jpg"

    vid = cv2.VideoCapture(0)

    pose_results = None
    fps = []
    startTime = time.time()
    while (time.time() - startTime) < 60:
        ret, frame = vid.read()
        start = time.time()
        pose_results, _ = inference_top_down_pose_model(pose_model, frame)
        loc_fps = 1 / (time.time() - start)
        print(loc_fps)
        fps.append(loc_fps)
        print(pose_results)
        for person in pose_results:
            for point in person["keypoints"]:
                confidence = point[2]
                if confidence > 0.5:
                    cv2.drawMarker(frame, (int(point[0]), int(point[1])), (0, 255, 0), markerType=cv2.MARKER_CROSS)
        cv2.imshow("frame", frame)
        if cv2.waitKey(1) & 0xff == ord("q"):
            break
    print(f"Average fps {sum(fps) / len(fps)}")
    vid.release()
    cv2.destroyAllWindows()

