using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LinearProxyMapper : ProxyMapper
{
    [Serializable]
    public struct LinearProxyMapperAttributes
    {
        public BoneAxis primaryAxis;
        public Transform staticProxy;
        public Transform dynamicProxy;
        public bool resetRotation;
        public Vector3 resetTo;
    }

    private Transform staticProxy;
    private Transform dynamicProxy;
    private BoneAxis primary;
    private bool resetRotation;
    private Quaternion resetTo;

    public LinearProxyMapper(Transform bone, LinearProxyMapperAttributes attr) : 
        base(bone)
    {
        this.staticProxy = attr.staticProxy;
        this.dynamicProxy = attr.dynamicProxy;
        this.primary = attr.primaryAxis;
        this.resetRotation = attr.resetRotation;
        this.resetTo = Quaternion.Euler(attr.resetTo);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (resetRotation)
        {
            this.joint.localRotation = this.resetTo;
        }

        Vector3 dir = dynamicProxy.position - staticProxy.position;

        if (dir.sqrMagnitude != 0)
        {
            Vector3 primaryAxis = getBoneDir(this.primary);
            Quaternion boneRot = joint.rotation;
            Debug.DrawRay(joint.position, primaryAxis * 5, Color.red);
            Debug.DrawRay(joint.position, dir, Color.black);
            Quaternion rot = Quaternion.FromToRotation(primaryAxis, dir);
            Quaternion localRot = rot * boneRot;
            joint.localRotation = Quaternion.Inverse(joint.parent.rotation) * localRot;
        }
    }
}
