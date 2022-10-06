using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePositionSmoother : ProxyPositionSmoother
{
    private Vector3 lastPos = Vector3.positiveInfinity;

    public Vector3 getSmoothedPosition(Vector3 rawPosition)
    {
        if (lastPos.Equals(Vector3.positiveInfinity))
        {
            lastPos = rawPosition;
            return rawPosition;
        }
        else
        {
            Vector3 smoothedPos = (rawPosition + lastPos) / 2;
            lastPos = rawPosition;
            return smoothedPos;
        }
    }
}
