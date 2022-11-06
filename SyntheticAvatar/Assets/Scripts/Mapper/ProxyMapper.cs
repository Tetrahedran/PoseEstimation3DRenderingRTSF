using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ProxyMapper
{

    protected Transform joint;

    public ProxyMapper(Transform bone)
    {
        this.joint = bone;
    }

    public abstract void Update();

    protected Vector3 getBoneDir(BoneForward forward)
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

public enum BoneForward
{
    UP, DOWN, FORWARD, BACKWARD, RIGHT, LEFT
}

public static class BoneForwardExtension
{
    public static BoneForward negate(this BoneForward val)
    {
        switch (val)
        {
            case BoneForward.UP:
                return BoneForward.DOWN;
            case BoneForward.DOWN:
                return BoneForward.UP;
            case BoneForward.FORWARD:
                return BoneForward.BACKWARD;
            case BoneForward.BACKWARD:
                return BoneForward.FORWARD;
            case BoneForward.RIGHT:
                return BoneForward.LEFT;
            case BoneForward.LEFT:
                return BoneForward.RIGHT;
            default:
                throw new System.Exception("Unexpected enum value");
        }
    }
}
