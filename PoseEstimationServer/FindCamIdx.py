import cv2

if __name__=="__main__":
    working = []
    for i in range(50):
        vid = cv2.VideoCapture(i, cv2.CAP_DSHOW)
        print(i)
        if vid.isOpened():
            working.append(i)
    print(working)
