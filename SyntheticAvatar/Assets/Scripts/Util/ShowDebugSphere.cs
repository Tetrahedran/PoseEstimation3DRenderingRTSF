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
            child.transform.localScale = Vector3.one * 0.1f;
            MeshFilter filter = child.gameObject.AddComponent<MeshFilter>();
            filter.mesh = sphere.GetComponent<MeshFilter>().mesh;
            child.gameObject.AddComponent<MeshRenderer>();
            Destroy(sphere);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
