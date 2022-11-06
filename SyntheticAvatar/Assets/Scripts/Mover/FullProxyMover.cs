using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullProxyMover : MonoBehaviour
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

    private Dictionary<ProxyBone, ProxyMover> dynamicProxies = new Dictionary<ProxyBone, ProxyMover>();
    // Start is called before the first frame update
    void Start()
    {
        //Spinal chain
        addProxyMoverFor(ProxyBone.Hips, hips);
        addProxyMoverFor(ProxyBone.Spine, spine);
        addProxyMoverFor(ProxyBone.Neck, neck);
        addProxyMoverFor(ProxyBone.Head, head);
        addProxyMoverFor(ProxyBone.Nose, nose);

        //Right Arm Chain
        addProxyMoverFor(ProxyBone.RightUpperArm, rUpperArm);
        addProxyMoverFor(ProxyBone.RightLowerArm, rLowerArm);
        addProxyMoverFor(ProxyBone.RightHand, rHand);
        addProxyMoverFor(ProxyBone.RightFingers, rFingers);

        //Left Arm Chain
        addProxyMoverFor(ProxyBone.LeftUpperArm, lUpperArm);
        addProxyMoverFor(ProxyBone.LeftLowerArm, lLowerArm);
        addProxyMoverFor(ProxyBone.LeftHand, lHand);
        addProxyMoverFor(ProxyBone.LeftFingers, lFingers);

        //Right Leg Chain
        addProxyMoverFor(ProxyBone.RightUpperLeg, rUpperLeg);
        addProxyMoverFor(ProxyBone.RightLowerLeg, rLowerLeg);
        addProxyMoverFor(ProxyBone.RightFoot, rFoot);
        addProxyMoverFor(ProxyBone.RightToes, rToes);

        //Left Leg Chain
        addProxyMoverFor(ProxyBone.LeftUpperLeg, lUpperLeg);
        addProxyMoverFor(ProxyBone.LeftLowerLeg, lLowerLeg);
        addProxyMoverFor(ProxyBone.LeftFoot, lFoot);
        addProxyMoverFor(ProxyBone.LeftToes, lToes);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void move(ProxyBone bone, Vector3 pos)
    {
        if (dynamicProxies.ContainsKey(bone))
        {
            dynamicProxies[bone].move(pos);
        }
        else
        {
            Debug.LogWarning($"Requesting movement of bone {bone} which is not defined in ProxyMover");
        }
    }

    private void addProxyMoverFor(ProxyBone bone, Transform transform)
    {
        dynamicProxies.Add(bone, new ProxyMover(transform));
    }
}
