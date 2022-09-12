using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RootProxyFollower : MonoBehaviour
{
    [SerializeField]
    private Transform rootProxy;
    [SerializeField]
    private HumanBodyBones rootBone;

    private Transform rootBoneTransform;
    // Start is called before the first frame update
    void Start()
    {
        rootBoneTransform = GetComponent<Animator>().GetBoneTransform(rootBone);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 translation = rootProxy.localPosition - rootBoneTransform.position;
        transform.Translate(translation);
    }
}
