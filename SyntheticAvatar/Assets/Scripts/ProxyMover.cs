using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyMover : MonoBehaviour
{
    public Transform proxy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void move(Vector3 pos)
    {
        proxy.localPosition = pos;
    }
}
