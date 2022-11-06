using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyMover
{
    private Transform transform;

    public ProxyMover(Transform transform)
    {
        this.transform = transform;
    }

    public void move(Vector3 newLocalPos)
    {
        transform.localPosition = newLocalPos;
    }
}
