using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show_FUR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.right * -1, Color.green);
    }
}
