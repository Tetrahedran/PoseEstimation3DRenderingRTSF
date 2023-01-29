using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriangleProxyMapper : AbstractVectorFollowProxy
{
    [Serializable]
    public struct TriangleProxyMapperAttributes
    {
        public BoneAxis boneDirection;
        public BoneAxis mappingPlaneNormal;
        public Transform staticProxy;
        public Transform proxy1, proxy2;
    }

    private Transform staticProxy;
    private Transform proxy1, proxy2;

    public TriangleProxyMapper(Transform bone, TriangleProxyMapperAttributes attr) : 
        base(bone, attr.boneDirection, attr.mappingPlaneNormal)
    {
        this.staticProxy = attr.staticProxy;
        this.proxy1 = attr.proxy1;
        this.proxy2 = attr.proxy2;
    }

    protected override Vector3 getFollowVector()
    {
        Vector3 v1 = proxy1.localPosition - staticProxy.localPosition;
        Vector3 v2 = proxy2.localPosition - staticProxy.localPosition;
        Vector3 normal = Vector3.Cross(v1, v2).normalized;
        return normal;
    }
}
