using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DistanceCollector : MonoBehaviour
{
    public GameCycleScript gameCycle;

    bool meassure;
    Vector3 lastPos;
    float distance;
    // Start is called before the first frame update
    void Start()
    {
        meassure = false;
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (meassure)
        {
            distance += calcDelta();
        }
    }

    public void handleNewMeassurement(object sender, EventArgs args)
    {
        meassure = true;
        distance = 0;
        lastPos = transform.position;
    }

    public float endMeassurement()
    {
        meassure = false;
        distance += calcDelta();
        return distance;
    }

    private float calcDelta()
    {
        Vector3 delta = transform.position - lastPos;
        lastPos = transform.position;
        return delta.magnitude;
    }
}
