using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyMover
{
    private Transform transform;
    private Vector3 prevSmoothedPosition;
    private int a = 1;
    private int b = 1;

    public ProxyMover(Transform transform)
    {
        this.transform = transform;
        prevSmoothedPosition = Vector3.positiveInfinity;
    }

    public void move(Vector3 newLocalPos)
    {
        // Setting init value for smoothing
        if(prevSmoothedPosition.Equals(Vector3.positiveInfinity))
        {
            prevSmoothedPosition = newLocalPos;
        }
        // Apply simple smoothing
        else
        {
            Vector3 smoothedPos = (a * prevSmoothedPosition + b * newLocalPos) / (a + b);
            prevSmoothedPosition = smoothedPos;
            transform.localPosition = smoothedPos;
        }
    }
}
