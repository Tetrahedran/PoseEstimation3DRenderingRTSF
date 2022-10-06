using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyMover
{
    private Transform transform;
    private ProxyPositionSmoother smoother;

    public ProxyMover(Transform transform, ProxyPositionSmoother smoother)
    {
        this.transform = transform;
        this.smoother = smoother;
    }

    public ProxyMover(Transform transform) : this(transform, new NoneSmoother())
    {
    }

    public void move(Vector3 newLocalPos)
    {
        Vector3 smoothedPosition = this.smoother.getSmoothedPosition(newLocalPos);
        transform.localPosition = smoothedPosition;
    }
}
