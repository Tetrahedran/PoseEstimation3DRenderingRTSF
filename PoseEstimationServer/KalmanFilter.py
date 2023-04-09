import time
import timeit

import numpy as np


class KalmanFilter:
    """
    Implementation according to material provided by W. Franklin
    https://de.wikipedia.org/wiki/Kalman-Filter
    https://thekalmanfilter.com/kalman-filter-python-example/
    https://thekalmanfilter.com/kalman-filter-explained-simply/
    https://thekalmanfilter.com/covariance-matrix-explained/
    """

    x, P, A, AT, H, HT, R, Q = None, None, None, None, None, None, None, None

    def __init__(self):
        self.start = time.time()
        #Init Filter
        """
        state vector in form
        x
        y
        z
        dx
        dy
        dz
        ddx
        ddy
        ddz
        """
        self.x = np.array([[1], [1], [1], [1], [1], [1], [.1], [.1], [.1]], dtype="float")

        """
        State noise matrix
        """
        self.P = np.identity(9, dtype="float") * 3
        self.P[3][3] = 20
        self.P[4][4] = 20
        self.P[5][5] = 20

        """
        Matrix to get from one state to the next
        """
        self.A = np.identity(9, dtype="float")

        """
        Matrix to get from state to measurement
        """
        self.H = np.zeros((3, 9), dtype="float")
        self.H[0][0] = 1
        self.H[1][1] = 1
        self.H[2][2] = 1

        self.HT = self.H.transpose()

        """
        Noise for measurement in form
        s2x sxy sxz
        syx s2y syz
        szx szy s2z
        """
        self.R = np.identity(3, dtype="float") * .1

        """
        Noise for state in form
            x   y   z   dx  dy  dz  ddx ddy ddz
        x   s2x syx ...........................
        y   sxy s2y
        z   .       s2z
        dx  .           .
        dy  .               .
        dz  .                   .
        ddx .                       .
        ddy .                           .
        ddz sxddz ..........................s2ddz
        """
        self.Q = np.identity(9, dtype="float")
        self.Q[3][3] = .5
        self.Q[4][4] = .5
        self.Q[5][5] = .5
        self.Q[6][6] = .1
        self.Q[7][7] = .1
        self.Q[8][8] = .1
        self.Q *= .025

    def filter(self, z):
        """

        :param z:
        :return:
        """
        # construct terms for position in form of pos = pos + vel*dt + 1/2*acc*dt²
        # construct terms for velocity in form of vel = vel + acc*dt
        dt = time.time() - self.start
        halfdtsq = .5 * dt * dt
        # x += v(x) * dt
        self.A[0][3] = dt
        # y += v(y) * dt
        self.A[1][4] = dt
        # z += v(z) * dt
        self.A[2][5] = dt
        # x += a(x) * .5 dt^2
        self.A[0][6] = halfdtsq
        # y += a(y) ' .5 dt ^2
        self.A[1][7] = halfdtsq
        # z += a(z) * .5 dt^2
        self.A[2][8] = halfdtsq
        # v(x) += a(x) * dt
        self.A[3][6] = dt
        # v(y) += a(y) * dt
        self.A[4][7] = dt
        # v(z) += a(z) * dt
        self.A[5][8] = dt

        AT = self.A.transpose()

        """
        Measurement in form
        x
        y 
        z
        """
        z = np.array([[z[0]], [z[1]], [z[2]]], dtype="float")

        # calculate state estimation based on previous state
        x_p = self.A.dot(self.x)

        # calculate covariance matrix estimation based on previous state and process noise (Q)
        P_p = self.A.dot(self.P).dot(AT) + self.Q

        # calculate kalman gain using measurement noise and estimations
        # to decide whether to follow measurement or state estimation
        S = self.H.dot(P_p).dot(self.HT) + self.R
        K = P_p.dot(self.HT).dot(np.linalg.inv(S))

        # calculate filtered new state and covariance
        residual = z - self.H.dot(x_p)
        self.x = x_p + K.dot(residual)
        self.P = P_p - K.dot(self.H).dot(P_p)

        self.start = time.time()

        return self.x, self.P

    def reset(self):
        self.__init__()
