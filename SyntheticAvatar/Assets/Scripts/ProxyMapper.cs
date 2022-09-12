using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyMapper
{
    public enum BoneForward
    {
        UP, DOWN, FORWARD, BACKWARD, RIGHT, LEFT
    }

    private Transform staticProxy;
    private Transform dynamicProxy;
    private Transform joint;
    private BoneForward forward;

    public ProxyMapper(Transform bone, Transform staticProxy, Transform dynamicProxy, BoneForward forwardDir)
    {
        this.joint = bone;
        this.staticProxy = staticProxy;
        this.dynamicProxy = dynamicProxy;
        this.forward = forwardDir;
    }

    public void Update()
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
            joint.rotation = localRot;
        }
    }

    private Vector3 getBoneDir()
    {
        switch (forward)
        {
            case BoneForward.UP:
                return joint.up;
            case BoneForward.DOWN:
                return joint.up * -1;
            case BoneForward.FORWARD:
                return joint.forward;
            case BoneForward.BACKWARD:
                return joint.forward * -1;
            case BoneForward.RIGHT:
                return joint.right;
            case BoneForward.LEFT:
                return joint.right * -1;
            default:
                throw new System.Exception();
        }
    }
}
