using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyMover
{
    private Transform transform;
    private Vector3 prevSmoothedPosition;

    public ProxyMover(Transform transform)
    {
        this.transform = transform;
        prevSmoothedPosition = Vector3.positiveInfinity;
    }

    public void move(Vector3 newLocalPos)
    {
        // Setting init value for smoothing
        if(prevSmoothedPosition == Vector3.positiveInfinity)
        {
            prevSmoothedPosition = newLocalPos;
        }
        // Apply simple smoothing
        else
        {
            Vector3 smoothedPos = (prevSmoothedPosition + newLocalPos) / 2;
            prevSmoothedPosition = smoothedPos;
            transform.localPosition = smoothedPos;
        }
    }
}
