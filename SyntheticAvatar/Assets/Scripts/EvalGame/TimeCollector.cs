using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCollector : MonoBehaviour
{
    public GameCycleScript GameCycle;

    private float start;

    // Start is called before the first frame update
    void Start()
    {
        start = Time.realtimeSinceStartup;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hand" && GameCycle != null)
        {
            float delta = Time.realtimeSinceStartup - start;
            float dist = other.GetComponent<DistanceCollector>().endMeassurement();
            GameCycle.Hit(delta, dist, gameObject);
        }
    }
}
