using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VectorFollowMapper : AbstractVectorFollowProxy
{
    [Serializable]
    public struct VectorFollowMapperAttributes
    {
        public BoneAxis boneDirection;
        public BoneAxis mappingPlaneNormal;
        public Transform staticProxy;
        public Transform dynamicProxy;
    }

    private Transform staticProxy;
    private Transform dynamicProxy;

    public VectorFollowMapper(Transform bone, VectorFollowMapperAttributes attr) : 
        base(bone, attr.boneDirection, attr.mappingPlaneNormal)
    {
        this.dynamicProxy = attr.dynamicProxy;
        this.staticProxy = attr.staticProxy;
    }

    protected override Vector3 getFollowVector()
    {
        return dynamicProxy.position - staticProxy.position;
    }
}
