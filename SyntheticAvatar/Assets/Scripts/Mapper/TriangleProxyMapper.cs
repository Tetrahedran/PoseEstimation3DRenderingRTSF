using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleProxyMapper : AbstractVectorFollowProxy
{
    private Transform staticProxy;
    private Transform proxy1, proxy2;

    public TriangleProxyMapper(Transform bone, Transform staticProxy, Transform proxy1, Transform proxy2, BoneForward boneDir, BoneForward mappingPlaneNormal) : 
        base(bone, boneDir, mappingPlaneNormal)
    {
        this.staticProxy = staticProxy;
        this.proxy1 = proxy1;
        this.proxy2 = proxy2;
    }

    protected override Vector3 getFollowVector()
    {
        Vector3 v1 = proxy1.localPosition - staticProxy.localPosition;
        Vector3 v2 = proxy2.localPosition - staticProxy.localPosition;
        Vector3 normal = Vector3.Cross(v1, v2).normalized;
        return normal;
    }
}
