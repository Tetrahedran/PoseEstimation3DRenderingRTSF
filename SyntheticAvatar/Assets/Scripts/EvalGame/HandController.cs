using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField]
    private GameCycleScript gameController;

    private bool Meassure;

    private float StartTime;

    private Vector3 lastPos;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        Meassure = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Meassure)
        {
            distance += updateDistance();
        }
    }

    public void StartMeassurement()
    {
        distance = 0;
        lastPos = transform.position;
        Meassure = true;
        StartTime = Time.realtimeSinceStartup;
    }

    public void ResetMeassurement()
    {
        Meassure = false;
        StartTime = 0;
        distance = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Meassure && other.tag == "Target" && gameController != null)
        {
            distance += updateDistance();
            float timeDiff = calcTimeDiff();
            gameController.Hit(timeDiff, distance, other.gameObject);
        }
    }

    private float calcTimeDiff()
    {
        return Time.realtimeSinceStartup - StartTime;
    }

    private float updateDistance()
    {
        Vector3 delta = transform.position - lastPos;
        lastPos = transform.position;
        return delta.magnitude;
    }
}
