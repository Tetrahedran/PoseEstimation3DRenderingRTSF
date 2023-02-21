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

    public Transform touchSphereL;
    public Transform touchSphereR;
    public float sphereDistance;

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

        do
        {
            float xFactor = Random.Range(-1.0f, 1);
            float yFactor = Random.Range(-1.0f, 1);

            outputVector = new Vector3(xFactor * maxDist, yFactor * maxDist, 0) + basis;
        } while (tooCloseTolastPos(lastRndPos, outputVector) || insideSphere(outputVector));
        lastRndPos = outputVector;
        return outputVector;
    }

    private bool tooCloseTolastPos(Vector2 lastPos, Vector2 newPos)
    {
        return (lastPos - newPos).magnitude < (maxDist * .45f);
    }

    private bool insideSphere(Vector2 newPos)
    {
        Vector2 leftSpherePos = touchSphereL.position;
        Vector2 rightSpherePos = touchSphereR.position;

        if((leftSpherePos - newPos).magnitude < sphereDistance)
        {
            return true;
        }
        else if ((rightSpherePos - newPos).magnitude < sphereDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
