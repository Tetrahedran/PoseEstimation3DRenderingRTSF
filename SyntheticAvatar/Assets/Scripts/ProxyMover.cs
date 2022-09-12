using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyMover : MonoBehaviour
{
    public Transform root;
    public Transform ellBowProxy;
    public Transform handProxy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void move(HumanBodyBones bone, Vector3 pos)
    {
        switch (bone)
        {
            case HumanBodyBones.LeftUpperArm:
                ellBowProxy.localPosition = pos;
                break;
            case HumanBodyBones.LeftLowerArm:
                handProxy.localPosition = pos;
                break;
            default:
                break;
        }
    }
}
