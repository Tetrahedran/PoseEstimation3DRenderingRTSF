import numpy as np


class KalmanFilter:
    """
    https://de.wikipedia.org/wiki/Kalman-Filter
    https://thekalmanfilter.com/kalman-filter-python-example/
    https://thekalmanfilter.com/kalman-filter-explained-simply/
    https://thekalmanfilter.com/covariance-matrix-explained/
    """

    x, P, A, AT, H, HT, R, Q = None, None, None, None, None, None, None, None

    def __init__(self):
        self.calls = 0
        #Init Filter
        """
        state vector in form
        x
        y
        z
        """
        self.x = np.array([[1], [1], [1]])

        """
        State noise matrix
        """
        self.P = np.matrix([[1, 0, 0], [0, 1, 0], [0, 0, 1]])

        """
        Matrix to get from one state to the next
        """
        self.A = np.matrix([[1, 0, 0], [0, 1, 0], [0, 0, 1]])

        self.AT = self.A.transpose()

        """
        Matrix to get from state to measurement
        """
        self.H = np.matrix([[1, 0, 0], [0, 1, 0], [0, 0, 1]])

        self.HT = self.H.transpose()

        """
        Noise for measurement in form
        s2x sxy sxz
        syx s2y syz
        szx szy s2z
        """
        self.R = np.matrix([[1, 0, 0], [0, 1, 0], [0, 0, 1]]) * 0.25

        """
        Noise for state in form
        s2x sxy sxz
        syx s2y syz
        szx szy s2z
        """
        self.Q = np.matrix([[1, 0, 0], [0, 1, 0], [0, 0, 1]]) * 0.1

    def filter(self, z):
        """

        :param z:
        :return:
        """
        """
        Measurement in form
        x
        y 
        z
        """
        z = np.array([[z[0]], [z[1]], [z[2]]])

        x_p = self.A.dot(self.x)

        P_p = self.A.dot(self.P).dot(self.AT) + self.Q

        S = self.H.dot(P_p).dot(self.HT) + self.R

        K = P_p.dot(self.HT).dot(np.linalg.inv(S))

        residual = z - self.H.dot(x_p)

        self.x = x_p + K.dot(residual)

        self.P = P_p - K.dot(self.H).dot(P_p)

        self.calls += 1

        return self.x, self.P

    def reset(self):
        self.calls = 0
