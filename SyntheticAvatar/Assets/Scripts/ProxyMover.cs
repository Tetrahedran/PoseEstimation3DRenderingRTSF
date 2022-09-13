using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyMover : MonoBehaviour
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

    private Dictionary<HumanBodyBones, Transform> dynamicProxies = new Dictionary<HumanBodyBones, Transform>();
    // Start is called before the first frame update
    void Start()
    {
        //Spinal chain
        dynamicProxies.Add(HumanBodyBones.Hips, spine);
        dynamicProxies.Add(HumanBodyBones.Spine, neck);
        dynamicProxies.Add(HumanBodyBones.Head, head);

        //Right Arm Chain
        dynamicProxies.Add(HumanBodyBones.RightUpperArm, rLowerArm);
        dynamicProxies.Add(HumanBodyBones.RightLowerArm, rHand);
        dynamicProxies.Add(HumanBodyBones.RightHand, rFingers);

        //Left Arm Chain
        dynamicProxies.Add(HumanBodyBones.LeftUpperArm, lLowerArm);
        dynamicProxies.Add(HumanBodyBones.LeftLowerArm, lHand);
        dynamicProxies.Add(HumanBodyBones.LeftHand, lFingers);

        //Right Leg Chain
        dynamicProxies.Add(HumanBodyBones.RightUpperLeg, rUpperLeg);
        dynamicProxies.Add(HumanBodyBones.RightLowerLeg, rFoot);
        dynamicProxies.Add(HumanBodyBones.RightFoot, rToes);

        //Left Leg Chain
        dynamicProxies.Add(HumanBodyBones.LeftUpperLeg, lLowerLeg);
        dynamicProxies.Add(HumanBodyBones.LeftLowerLeg, lFoot);
        dynamicProxies.Add(HumanBodyBones.LeftFoot, lToes);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void move(HumanBodyBones bone, Vector3 pos)
    {
        if (dynamicProxies.ContainsKey(bone))
        {
            dynamicProxies[bone].localPosition = pos;
        }
        else
        {
            Debug.LogWarning($"Requesting movement of bone {bone} which is not defined in ProxyMover");
        }
    }
}
