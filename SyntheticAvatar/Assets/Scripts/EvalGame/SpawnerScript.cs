using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameCycleScript))]
public class SpawnerScript : MonoBehaviour
{
    public GameObject prefab;
    public Transform shoulderL;
    public Transform shoulderR;
    public float maxDist;

    private GameCycleScript GameCycle;
    private Vector3 lastRndPos;

    // Start is called before the first frame update
    void Start()
    {
        GameCycle = GetComponent<GameCycleScript>();
        lastRndPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnOne()
    {
        GameObject obj = Instantiate(prefab);
        obj.GetComponent<TimeCollector>().GameCycle = this.GameCycle;
        obj.transform.position = getRndPosition();
    }

    private Vector3 getRndPosition()
    {
        Vector3 basis;
        if(Random.Range(0.0f,1) >= 0.5)
        {
            basis = shoulderL.position;
        }
        else
        {
            basis = shoulderR.position;
        }

        Vector3 outputVector;
        do
        {
            float xFactor = Random.Range(-1.0f, 1);
            float yFactor = Random.Range(-1.0f, 1);

            outputVector = new Vector3(xFactor * maxDist, yFactor * maxDist, 0) + basis;
        } while ((lastRndPos - outputVector).magnitude < (maxDist * .25f));
        lastRndPos = outputVector;
        return outputVector;
    }
}
