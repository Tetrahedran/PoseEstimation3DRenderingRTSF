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

    private Vector3 lastRndPos;

    // Start is called before the first frame update
    void Start()
    {
        lastRndPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 SpawnOne()
    {
        GameObject obj = Instantiate(prefab);
        Vector3 pos = getRndPosition();
        obj.transform.position = pos;
        return pos;
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
        Vector2 lastrndpos, outvec;

        do
        {
            float xFactor = Random.Range(-1.0f, 1);
            float yFactor = Random.Range(-1.0f, 1);

            outputVector = new Vector3(xFactor * maxDist, yFactor * maxDist, 0) + basis;
            lastrndpos = lastRndPos;
            outvec = outputVector;
        } while ((lastrndpos - outvec).magnitude < (maxDist * .45f));
        lastRndPos = outputVector;
        return outputVector;
    }
}
