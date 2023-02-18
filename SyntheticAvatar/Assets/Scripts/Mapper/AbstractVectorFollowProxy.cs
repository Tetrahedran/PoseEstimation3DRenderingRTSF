using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractVectorFollowProxy : ProxyMapper
{
    private BoneAxis primary;
    private BoneAxis secondary;

    public AbstractVectorFollowProxy(Transform bone, BoneAxis secondary, BoneAxis mappingPlaneNormal): base(bone)
    {
        this.primary = mappingPlaneNormal;
        this.secondary = secondary;
    }

    protected abstract Vector3 getFollowVector();

    public sealed override void Update()
    {
        Vector3 dir = getFollowVector();
        Vector3 normal = getBoneDir(primary);
        dir = Vector3.ProjectOnPlane(dir, normal);
        if (dir.sqrMagnitude != 0)
        {
            Vector3 secondaryAxis = getBoneDir(this.secondary);
            Quaternion boneRot = joint.rotation;
            Quaternion rot = Quaternion.FromToRotation(secondaryAxis, dir);
            Quaternion localRot = rot * boneRot;
            joint.localRotation = Quaternion.Inverse(joint.parent.rotation) * localRot;
        }
    }
}
