using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FullProxyMapper : MonoBehaviour
{
    [SerializeField]
    private BoneForward mainBoneDirection = BoneForward.LEFT;
    [SerializeField]
    private BoneForward secondaryBoneDirection = BoneForward.UP;
    [Space]
    [Header("Required bone proxies")]
    [Header("Spinal chain")]
    [SerializeField]
    private Transform hips;
    [SerializeField]
    private Transform spine;
    [SerializeField]
    private Transform neck;
    [SerializeField]
    private Transform head;
    [SerializeField]
    private Transform nose;
    [Header("Right Arm Chain")]
    [SerializeField]
    private Transform rUpperArm;
    [SerializeField]
    private Transform rLowerArm;
    [SerializeField]
    private Transform rHand;
    [SerializeField]
    private Transform rFingers;
    [Header("Left Arm Chain")]
    [SerializeField]
    private Transform lUpperArm;
    [SerializeField]
    private Transform lLowerArm;
    [SerializeField]
    private Transform lHand;
    [SerializeField]
    private Transform lFingers;
    [Header("Right Leg Chain")]
    [SerializeField]
    private Transform rUpperLeg;
    [SerializeField]
    private Transform rLowerLeg;
    [SerializeField]
    private Transform rFoot;
    [SerializeField]
    private Transform rToes;
    [Header("Left Leg Chain")]
    [SerializeField]
    private Transform lUpperLeg;
    [SerializeField]
    private Transform lLowerLeg;
    [SerializeField]
    private Transform lFoot;
    [SerializeField]
    private Transform lToes;

    private List<ProxyMapper> mappings = new List<ProxyMapper>();

    // Start is called before the first frame update
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        //Spinal Chain
        mappings.Add(create(anim, HumanBodyBones.Hips, hips, spine));
        mappings.Add(new TriangleProxyMapper(anim.GetBoneTransform(HumanBodyBones.Hips), spine, rUpperLeg, lUpperLeg, secondaryBoneDirection.negate(), mainBoneDirection));
        mappings.Add(create(anim, HumanBodyBones.Spine, spine, neck));
        mappings.Add(new TriangleProxyMapper(anim.GetBoneTransform(HumanBodyBones.Spine), spine, rUpperArm, lUpperArm, secondaryBoneDirection, mainBoneDirection));
        mappings.Add(create(anim, HumanBodyBones.Head, neck, head));
        mappings.Add(new VectorFollowMapper(anim.GetBoneTransform(HumanBodyBones.Head), neck, nose, secondaryBoneDirection, mainBoneDirection));

        //Right Arm Chain
        mappings.Add(create(anim, HumanBodyBones.RightUpperArm, rUpperArm, rLowerArm, true));
        mappings.Add(create(anim, HumanBodyBones.RightLowerArm, rLowerArm, rHand, true));
        mappings.Add(create(anim, HumanBodyBones.RightHand, rHand, rFingers, true));

        //Left Arm Chain
        mappings.Add(create(anim, HumanBodyBones.LeftUpperArm, lUpperArm, lLowerArm, true));
        mappings.Add(create(anim, HumanBodyBones.LeftLowerArm, lLowerArm, lHand, true));
        mappings.Add(create(anim, HumanBodyBones.LeftHand, lHand, lFingers, true));

        //Right Leg Chain
        mappings.Add(create(anim, HumanBodyBones.RightUpperLeg, rUpperLeg, rLowerLeg, true));
        mappings.Add(create(anim, HumanBodyBones.RightLowerLeg, rLowerLeg, rFoot, true));
        mappings.Add(create(anim, HumanBodyBones.RightFoot, rFoot, rToes, secondaryBoneDirection, true));

        //Left Leg Chain
        mappings.Add(create(anim, HumanBodyBones.LeftUpperLeg, lUpperLeg, lLowerLeg, true));
        mappings.Add(create(anim, HumanBodyBones.LeftLowerLeg, lLowerLeg, lFoot, true));
        mappings.Add(create(anim, HumanBodyBones.LeftFoot, lFoot, lToes, secondaryBoneDirection, true));
    }

    // Update is called once per frame
    void Update()
    {
        foreach(ProxyMapper mapper in mappings)
        {
            mapper.Update();
        }
    }

    private ProxyMapper create(Animator anim, HumanBodyBones bone, Transform root, Transform dynamic,BoneForward forward, bool resetRotation = false)
    {
        return new LinearProxyMapper(anim.GetBoneTransform(bone), root, dynamic, forward, resetRotation);
    }

    private ProxyMapper create(Animator anim, HumanBodyBones bone, Transform root, Transform dynamic, bool resetRotation = false)
    {
        return create(anim, bone, root, dynamic, mainBoneDirection, resetRotation);
    }
}
