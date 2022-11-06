using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class FullProxyMapper : MonoBehaviour
{
    [Header("Required bone proxies")]
    [Header("Spinal chain")]
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes hips;
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes spine;
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes head;
    [SerializeField]
    private TriangleProxyMapper.TriangleProxyMapperAttributes hipRotation;
    [SerializeField]
    private TriangleProxyMapper.TriangleProxyMapperAttributes shoulderRotation;
    [SerializeField]
    private VectorFollowMapper.VectorFollowMapperAttributes headRotation;
    [Header("Right Arm Chain")]
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes rUpperArm;
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes rLowerArm;
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes rHand;
    [Header("Left Arm Chain")]
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes lUpperArm;
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes lLowerArm;
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes lHand;
    [Header("Right Leg Chain")]
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes rUpperLeg;
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes rLowerLeg;
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes rFoot;
    [Header("Left Leg Chain")]
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes lUpperLeg;
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes lLowerLeg;
    [SerializeField]
    private LinearProxyMapper.LinearProxyMapperAttributes lFoot;

    private List<ProxyMapper> mappings = new List<ProxyMapper>();

    // Start is called before the first frame update
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        //Spinal Chain
        mappings.Add(create(anim, HumanBodyBones.Hips, hips));
        mappings.Add(new TriangleProxyMapper(anim.GetBoneTransform(HumanBodyBones.Hips), hipRotation));
        mappings.Add(create(anim, HumanBodyBones.Spine, spine));
        mappings.Add(new TriangleProxyMapper(anim.GetBoneTransform(HumanBodyBones.Spine), shoulderRotation));
        mappings.Add(create(anim, HumanBodyBones.Head, head));
        mappings.Add(new VectorFollowMapper(anim.GetBoneTransform(HumanBodyBones.Head), headRotation));

        //Right Arm Chain
        mappings.Add(create(anim, HumanBodyBones.RightUpperArm, rUpperArm));
        mappings.Add(create(anim, HumanBodyBones.RightLowerArm, rLowerArm));
        mappings.Add(create(anim, HumanBodyBones.RightHand, rHand));

        //Left Arm Chain
        mappings.Add(create(anim, HumanBodyBones.LeftUpperArm, lUpperArm));
        mappings.Add(create(anim, HumanBodyBones.LeftLowerArm, lLowerArm));
        mappings.Add(create(anim, HumanBodyBones.LeftHand, lHand));

        //Right Leg Chain
        mappings.Add(create(anim, HumanBodyBones.RightUpperLeg, rUpperLeg));
        mappings.Add(create(anim, HumanBodyBones.RightLowerLeg, rLowerLeg));
        mappings.Add(create(anim, HumanBodyBones.RightFoot, rFoot));

        //Left Leg Chain
        mappings.Add(create(anim, HumanBodyBones.LeftUpperLeg, lUpperLeg));
        mappings.Add(create(anim, HumanBodyBones.LeftLowerLeg, lLowerLeg));
        mappings.Add(create(anim, HumanBodyBones.LeftFoot, lFoot));
    }

    // Update is called once per frame
    void Update()
    {
        foreach(ProxyMapper mapper in mappings)
        {
            mapper.Update();
        }
    }

    private ProxyMapper create(Animator anim, HumanBodyBones bone, LinearProxyMapper.LinearProxyMapperAttributes attr)
    {
        return new LinearProxyMapper(anim.GetBoneTransform(bone), attr);
    }
}
