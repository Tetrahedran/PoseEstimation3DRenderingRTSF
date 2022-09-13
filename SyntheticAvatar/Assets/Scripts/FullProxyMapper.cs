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
        mappings.Add(create(anim, HumanBodyBones.Hips, hips, spine, ProxyMapper.BoneForward.UP));
        mappings.Add(create(anim, HumanBodyBones.Spine, spine, neck, ProxyMapper.BoneForward.UP));
        mappings.Add(create(anim, HumanBodyBones.Head, neck, head, ProxyMapper.BoneForward.UP));

        //Right Arm Chain
        mappings.Add(create(anim, HumanBodyBones.RightUpperArm, rUpperArm, rLowerArm, ProxyMapper.BoneForward.RIGHT));
        mappings.Add(create(anim, HumanBodyBones.RightLowerArm, rLowerArm, rHand, ProxyMapper.BoneForward.RIGHT));
        mappings.Add(create(anim, HumanBodyBones.RightHand, rHand, rFingers, ProxyMapper.BoneForward.RIGHT));

        //Left Arm Chain
        mappings.Add(create(anim, HumanBodyBones.LeftUpperArm, lUpperArm, lLowerArm, ProxyMapper.BoneForward.LEFT));
        mappings.Add(create(anim, HumanBodyBones.LeftLowerArm, lLowerArm, lHand, ProxyMapper.BoneForward.LEFT));
        mappings.Add(create(anim, HumanBodyBones.LeftHand, lHand, lFingers, ProxyMapper.BoneForward.LEFT));

        //Right Leg Chain
        mappings.Add(create(anim, HumanBodyBones.RightUpperLeg, rUpperLeg, rLowerLeg, ProxyMapper.BoneForward.DOWN));
        mappings.Add(create(anim, HumanBodyBones.RightLowerLeg, rLowerLeg, rFoot, ProxyMapper.BoneForward.DOWN));
        mappings.Add(create(anim, HumanBodyBones.RightFoot, rFoot, rToes, ProxyMapper.BoneForward.DOWN));

        //Left Leg Chain
        mappings.Add(create(anim, HumanBodyBones.LeftUpperLeg, lUpperLeg, lLowerLeg, ProxyMapper.BoneForward.DOWN));
        mappings.Add(create(anim, HumanBodyBones.LeftLowerLeg, lLowerLeg, lFoot, ProxyMapper.BoneForward.DOWN));
        mappings.Add(create(anim, HumanBodyBones.LeftFoot, lFoot, lToes, ProxyMapper.BoneForward.DOWN));
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
        return new ProxyMapper(anim.GetBoneTransform(bone), root, dynamic, forward);
    }
}
