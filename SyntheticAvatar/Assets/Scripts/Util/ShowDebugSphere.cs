using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDebugSphere : MonoBehaviour
{
    [SerializeField]
    private bool hideDebugSpheres;

    // Start is called before the first frame update
    void Start()
    {
        if (hideDebugSpheres)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
