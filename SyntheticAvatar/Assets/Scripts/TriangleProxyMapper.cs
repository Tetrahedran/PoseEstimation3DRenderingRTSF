using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleProxyMapper : ProxyMapper
{
    protected Transform proxy1, proxy2;
    protected BoneForward forward;

    public TriangleProxyMapper(Transform bone, Transform staticProxy, Transform proxy1, Transform proxy2, BoneForward up, BoneForward forward) : 
        base(bone, staticProxy, up)
    {
        this.proxy1 = proxy1;
        this.proxy2 = proxy2;
        this.forward = forward;
    }

    // Update is called once per frame
    public override void Update()
    {
        Quaternion boneRot = joint.rotation;
        Vector3 v1 = proxy1.localPosition - staticProxy.localPosition;
        Vector3 v2 = proxy2.localPosition - staticProxy.localPosition;
        Vector3 normal = Vector3.Cross(v1, v2).normalized;

        Vector3 normalAtFollowPlane = Vector3.ProjectOnPlane(normal, getBoneDir());
        Quaternion followAxisRot = Quaternion.FromToRotation(getBoneDir(forward), normalAtFollowPlane);

        Quaternion localRot = followAxisRot * boneRot;
        joint.localRotation = Quaternion.Inverse(joint.parent.rotation) * localRot;
    }
}
