from flask import Flask, json, request
from threading import Thread, Lock
from time import time, sleep

api = Flask(__name__)
lock = Lock()
imgData = bytearray()


def perform_rtsf():
    while True:
        getImg()
        sleep(1)


def create_rtsf_thread():
    x = Thread(target=perform_rtsf, daemon=True)
    return x


def getImg():
    lock.acquire()
    try:
        return imgData
    finally:
        lock.release()


def setImg(img):
    lock.acquire()
    try:
        global imgData
        imgData = img
    finally:
        lock.release()


@api.post("/img")
def get_unity_img_data():
    data = bytearray(request.data)
    setImg(data)
    return json.jsonify(200)

@api.get("/data")
def get_binary_data():
	data = getImg()
	return data

if __name__ == "__main__":
    rtsf = create_rtsf_thread()
    rtsf.start()
    api.run(port=5001)
