using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyMover : MonoBehaviour
{
    public enum ProxyBone
    {
        Hips = 0, Spine = 9, Neck = 10, Head = 11,
        RightUpperArm = 13, RightLowerArm = 15, RightHand = 17, RightFingers = 19,
        LeftUpperArm = 12, LeftLowerArm = 14, LeftHand = 16, LeftFingers = 18,
        RightUpperLeg = 2, RightLowerLeg = 4, RightFoot = 6, RightToes = 8,
        LeftUpperLeg = 1, LeftLowerLeg = 3, LeftFoot = 5, LeftToes = 7
    }

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

    private Dictionary<ProxyBone, Transform> dynamicProxies = new Dictionary<ProxyBone, Transform>();
    // Start is called before the first frame update
    void Start()
    {
        //Spinal chain
        dynamicProxies.Add(ProxyBone.Hips, hips);
        dynamicProxies.Add(ProxyBone.Spine, spine);
        dynamicProxies.Add(ProxyBone.Neck, neck);
        dynamicProxies.Add(ProxyBone.Head, head);

        //Right Arm Chain
        dynamicProxies.Add(ProxyBone.RightUpperArm, rUpperArm);
        dynamicProxies.Add(ProxyBone.RightLowerArm, rLowerArm);
        dynamicProxies.Add(ProxyBone.RightHand, rHand);
        dynamicProxies.Add(ProxyBone.RightFingers, rFingers);

        //Left Arm Chain
        dynamicProxies.Add(ProxyBone.LeftUpperArm, lUpperArm);
        dynamicProxies.Add(ProxyBone.LeftLowerArm, lLowerArm);
        dynamicProxies.Add(ProxyBone.LeftHand, lHand);
        dynamicProxies.Add(ProxyBone.LeftFingers, lFingers);

        //Right Leg Chain
        dynamicProxies.Add(ProxyBone.RightUpperLeg, rUpperLeg);
        dynamicProxies.Add(ProxyBone.RightLowerLeg, rLowerLeg);
        dynamicProxies.Add(ProxyBone.RightFoot, rFoot);
        dynamicProxies.Add(ProxyBone.RightToes, rToes);

        //Left Leg Chain
        dynamicProxies.Add(ProxyBone.LeftUpperLeg, lUpperLeg);
        dynamicProxies.Add(ProxyBone.LeftLowerLeg, lLowerLeg);
        dynamicProxies.Add(ProxyBone.LeftFoot, lFoot);
        dynamicProxies.Add(ProxyBone.LeftToes, lToes);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void move(ProxyBone bone, Vector3 pos)
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
