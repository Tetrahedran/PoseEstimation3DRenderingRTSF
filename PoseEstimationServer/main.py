# This is a sample Python script.

# Press Umschalt+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.

from flask import Flask, json

api = Flask(__name__)


@api.get("/test")
def get_hello_world():
    return json.dumps({"LeftUpperArm": [-0.25,0,0], "LeftLowerArm": [-0.5, 0, 0]})


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    api.run()

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
