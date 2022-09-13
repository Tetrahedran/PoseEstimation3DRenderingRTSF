using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProxyMapper
{
    public enum BoneForward
    {
        UP, DOWN, FORWARD, BACKWARD, RIGHT, LEFT
    }

    protected Transform staticProxy;
    protected Transform joint;
    protected BoneForward forward;

    public ProxyMapper(Transform bone, Transform staticProxy, BoneForward forwardDir)
    {
        this.joint = bone;
        this.staticProxy = staticProxy;
        this.forward = forwardDir;
    }

    public abstract void Update();

    protected Vector3 getBoneDir()
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
