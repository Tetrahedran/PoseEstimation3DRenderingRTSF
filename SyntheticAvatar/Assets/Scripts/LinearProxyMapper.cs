using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProxyMapper : ProxyMapper
{
    protected Transform dynamicProxy;

    public LinearProxyMapper(Transform bone, Transform staticProxy, Transform dynamicProxy, BoneForward forwardDir) : 
        base(bone, staticProxy, forwardDir)
    {
        this.dynamicProxy = dynamicProxy;
    }

    // Update is called once per frame
    public override void Update()
    {
        Vector3 dir = dynamicProxy.localPosition - staticProxy.localPosition;
        if (dir.sqrMagnitude != 0)
        {
            Vector3 boneDir = getBoneDir();
            Quaternion boneRot = joint.rotation;
            Debug.DrawRay(joint.position, boneDir * 5, Color.red);
            Debug.DrawRay(joint.position, dir, Color.black);
            Quaternion rot = Quaternion.FromToRotation(boneDir, dir);
            Quaternion localRot = rot * boneRot;
            joint.localRotation = Quaternion.Inverse(joint.parent.rotation) * localRot;
        }
    }
}
