using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FullProxyMapper : MonoBehaviour
{
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
        //mappings.Add(new TriangleProxyMapper(anim.GetBoneTransform(HumanBodyBones.Hips), hips, rUpperLeg, lUpperLeg, ProxyMapper.BoneForward.LEFT));
        mappings.Add(create(anim, HumanBodyBones.Spine, spine, neck));
        mappings.Add(new TriangleProxyMapper(anim.GetBoneTransform(HumanBodyBones.Spine), spine, rUpperArm, lUpperArm, ProxyMapper.BoneForward.LEFT));
        mappings.Add(create(anim, HumanBodyBones.Head, neck, head));

        //Right Arm Chain
        mappings.Add(create(anim, HumanBodyBones.RightUpperArm, rUpperArm, rLowerArm));
        mappings.Add(create(anim, HumanBodyBones.RightLowerArm, rLowerArm, rHand));
        mappings.Add(create(anim, HumanBodyBones.RightHand, rHand, rFingers));

        //Left Arm Chain
        mappings.Add(create(anim, HumanBodyBones.LeftUpperArm, lUpperArm, lLowerArm));
        mappings.Add(create(anim, HumanBodyBones.LeftLowerArm, lLowerArm, lHand));
        mappings.Add(create(anim, HumanBodyBones.LeftHand, lHand, lFingers));

        //Right Leg Chain
        mappings.Add(create(anim, HumanBodyBones.RightUpperLeg, rUpperLeg, rLowerLeg));
        mappings.Add(create(anim, HumanBodyBones.RightLowerLeg, rLowerLeg, rFoot));
        mappings.Add(create(anim, HumanBodyBones.RightFoot, rFoot, rToes, ProxyMapper.BoneForward.UP));

        //Left Leg Chain
        mappings.Add(create(anim, HumanBodyBones.LeftUpperLeg, lUpperLeg, lLowerLeg));
        mappings.Add(create(anim, HumanBodyBones.LeftLowerLeg, lLowerLeg, lFoot));
        mappings.Add(create(anim, HumanBodyBones.LeftFoot, lFoot, lToes, ProxyMapper.BoneForward.UP));
    }

    // Update is called once per frame
    void Update()
    {
        foreach(ProxyMapper mapper in mappings)
        {
            mapper.Update();
        }
    }

    private ProxyMapper create(Animator anim, HumanBodyBones bone, Transform root, Transform dynamic, ProxyMapper.BoneForward forward)
    {
        return new LinearProxyMapper(anim.GetBoneTransform(bone), root, dynamic, forward);
    }

    private ProxyMapper create(Animator anim, HumanBodyBones bone, Transform root, Transform dynamic)
    {
        return create(anim, bone, root, dynamic, ProxyMapper.BoneForward.LEFT);
    }
}
