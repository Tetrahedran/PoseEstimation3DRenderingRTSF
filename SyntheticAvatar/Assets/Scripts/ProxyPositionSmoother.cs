
using System;
using UnityEngine;

public interface ProxyPositionSmoother
{
    public Vector3 getSmoothedPosition(Vector3 rawPosition);
}

public class NoneSmoother: ProxyPositionSmoother
{
    public Vector3 getSmoothedPosition(Vector3 rawPosition)
    {
        return rawPosition;
    }
}
