using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractVectorFollowProxy : ProxyMapper
{
    private BoneForward mappingPlaneNormal;
    private BoneForward boneDir;

    public AbstractVectorFollowProxy(Transform bone, BoneForward boneDir, BoneForward mappingPlaneNormal): base(bone)
    {
        this.mappingPlaneNormal = mappingPlaneNormal;
        this.boneDir = boneDir;
    }

    protected abstract Vector3 getFollowVector();

    public sealed override void Update()
    {
        Vector3 dir = getFollowVector();
        Vector3 normal = getBoneDir(mappingPlaneNormal);
        dir = Vector3.ProjectOnPlane(dir, normal);
        if (dir.sqrMagnitude != 0)
        {
            Vector3 boneDir = getBoneDir(this.boneDir);
            Quaternion boneRot = joint.rotation;
            Quaternion rot = Quaternion.FromToRotation(boneDir, dir);
            Quaternion localRot = rot * boneRot;
            joint.localRotation = Quaternion.Inverse(joint.parent.rotation) * localRot;
        }
    }
}
