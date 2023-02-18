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

    protected Vector3 getBoneDir(BoneAxis forward)
    {
        switch (forward)
        {
            case BoneAxis.UP:
                return joint.up;
            case BoneAxis.DOWN:
                return joint.up * -1;
            case BoneAxis.FORWARD:
                return joint.forward;
            case BoneAxis.BACKWARD:
                return joint.forward * -1;
            case BoneAxis.RIGHT:
                return joint.right;
            case BoneAxis.LEFT:
                return joint.right * -1;
            default:
                throw new System.Exception();
        }
    }
}

public enum BoneAxis
{
    UP, DOWN, FORWARD, BACKWARD, RIGHT, LEFT
}

public static class BoneAxisExtension
{
    public static BoneAxis negate(this BoneAxis val)
    {
        switch (val)
        {
            case BoneAxis.UP:
                return BoneAxis.DOWN;
            case BoneAxis.DOWN:
                return BoneAxis.UP;
            case BoneAxis.FORWARD:
                return BoneAxis.BACKWARD;
            case BoneAxis.BACKWARD:
                return BoneAxis.FORWARD;
            case BoneAxis.RIGHT:
                return BoneAxis.LEFT;
            case BoneAxis.LEFT:
                return BoneAxis.RIGHT;
            default:
                throw new System.Exception("Unexpected enum value");
        }
    }
}
