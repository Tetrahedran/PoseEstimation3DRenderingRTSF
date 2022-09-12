using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FullProxyMapper : MonoBehaviour
{

    [SerializeField]
    private Transform root;
    [SerializeField]
    private Transform lEllbowProxy;
    [SerializeField]
    private Transform lHandProxy;

    private List<ProxyMapper> mappings = new List<ProxyMapper>();

    // Start is called before the first frame update
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        mappings.Add(new ProxyMapper(anim.GetBoneTransform(HumanBodyBones.LeftUpperArm), root, lEllbowProxy, ProxyMapper.BoneForward.LEFT));
        mappings.Add(new ProxyMapper(anim.GetBoneTransform(HumanBodyBones.LeftLowerArm), lEllbowProxy, lHandProxy, ProxyMapper.BoneForward.LEFT));
    }

    // Update is called once per frame
    void Update()
    {
        foreach(ProxyMapper mapper in mappings)
        {
            mapper.Update();
        }
    }
}
