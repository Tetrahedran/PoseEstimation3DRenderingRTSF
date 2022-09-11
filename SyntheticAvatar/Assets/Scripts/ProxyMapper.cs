using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ProxyMapper : MonoBehaviour
{
    [SerializeField]
    private Transform staticProxy;
    [SerializeField]
    private Transform dynamicProxy;
    [SerializeField]
    private HumanBodyBones bone;

    private Transform joint;


    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<Animator>().GetBoneTransform(bone);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = dynamicProxy.localPosition - staticProxy.localPosition;
        if (dir.sqrMagnitude != 0)
        {
            Vector3 boneDir = (joint.right * -1).normalized;
            Quaternion boneRot = joint.rotation;
            Debug.DrawRay(joint.position, boneDir * 5, Color.red);
            Debug.DrawRay(joint.position, dir, Color.black);
            Quaternion rot = Quaternion.FromToRotation(boneDir, dir);
            Quaternion localRot = rot * boneRot;
            joint.rotation = localRot;
        }
    }
}
