using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LinearProxyMapper : ProxyMapper
{
    [Serializable]
    public struct LinearProxyMapperAttributes
    {
        public BoneForward boneDirection;
        public Transform staticProxy;
        public Transform dynamicProxy;
        public bool resetRotation;
    }

    private Transform staticProxy;
    private Transform dynamicProxy;
    private BoneForward boneDir;
    private bool resetRotation;

    public LinearProxyMapper(Transform bone, LinearProxyMapperAttributes attr) : 
        base(bone)
    {
        this.staticProxy = attr.staticProxy;
        this.dynamicProxy = attr.dynamicProxy;
        this.boneDir = attr.boneDirection;
        this.resetRotation = attr.resetRotation;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (resetRotation)
        {
            this.joint.localRotation = Quaternion.identity;
        }
        Vector3 dir = dynamicProxy.position - staticProxy.position;
        if (dir.sqrMagnitude != 0)
        {
            Vector3 boneDir = getBoneDir(this.boneDir);
            Quaternion boneRot = joint.rotation;
            Debug.DrawRay(joint.position, boneDir * 5, Color.red);
            Debug.DrawRay(joint.position, dir, Color.black);
            Quaternion rot = Quaternion.FromToRotation(boneDir, dir);
            Quaternion localRot = rot * boneRot;
            joint.localRotation = Quaternion.Inverse(joint.parent.rotation) * localRot;
        }
    }
}
