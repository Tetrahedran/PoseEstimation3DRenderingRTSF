# This is a sample Python script.

# Press Umschalt+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.

from flask import Flask, json
import math

api = Flask(__name__)
counter = 0


@api.get("/test")
def get_hello_world():
    global counter
    counter += .1
    sin = math.sin(counter) * 0.2
    cos = math.cos(counter) * 0.2
    test = {
        "Hips": [0, .1, 0],
        "Spine": [0, 0.25, 0],
        "Neck": [0, 0.5, 0],
        "Head": [0, .75, 0],
        "LeftUpperArm": [0, .5, 0],
        "LeftLowerArm": [-0.25, .5 + sin, cos],
        "LeftHand": [-0.5, .5 + sin, cos],
        "LeftFingers": [-0.75, .5 + sin, cos],
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
    return json.dumps(test)


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    api.run()

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
