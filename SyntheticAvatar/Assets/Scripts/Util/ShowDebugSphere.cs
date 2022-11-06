using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDebugSphere : MonoBehaviour
{
    [SerializeField]
    private bool showDebugSphere;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = Vector3.one * .1f;
            sphere.transform.parent = child;
            sphere.transform.localPosition = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
