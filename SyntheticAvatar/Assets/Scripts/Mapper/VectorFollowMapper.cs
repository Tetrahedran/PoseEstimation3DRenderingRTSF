using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorFollowMapper : AbstractVectorFollowProxy
{
    private Transform staticProxy;
    private Transform dynamicProxy;

    public VectorFollowMapper(Transform bone, Transform staticProxy, Transform dynamicProxy, BoneForward boneDir, BoneForward mappingPlaneNormal) : 
        base(bone, boneDir, mappingPlaneNormal)
    {
        this.dynamicProxy = dynamicProxy;
        this.staticProxy = staticProxy;
    }

    protected override Vector3 getFollowVector()
    {
        return dynamicProxy.position - staticProxy.position;
    }
}
