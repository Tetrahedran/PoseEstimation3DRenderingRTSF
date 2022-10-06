using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Proyecto26;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;
using UnityEditor;

[RequireComponent(typeof(FullProxyMover))]
public class API : MonoBehaviour
{
    public string ip = "192.168.5.1";
    public int port = 8080;

    public bool fixedSpeed;
    [Range(5, 120)]
    public int fixedFPSSpeed;

    private FullProxyMover mover;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<FullProxyMover>();
        StartCoroutine(Serve());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Serve()
    {
        Debug.Log("Async Thread running");
        while (true)
        {
            if (!EditorApplication.isPaused)
            {
                RestClient.Get($"http://{ip}:{port}/test")
                    .Then(response =>
                    {
                        JObject obj = JObject.Parse(response.Text);
                        FullProxyMover.ProxyBone[] values = Enum.GetValues(typeof(FullProxyMover.ProxyBone)) as FullProxyMover.ProxyBone[];
                        Array.Sort(values);
                        foreach (FullProxyMover.ProxyBone bone in values)
                        {
                            if (obj.ContainsKey(bone.ToString()))
                            {
                                float[] pos = obj.Value<JArray>(bone.ToString()).ToObject<float[]>();
                                Vector3 vec = new Vector3(pos[0], pos[1], pos[2]);
                                mover.move(bone, vec);
                            }
                        }
                    });
            }
            if (!fixedSpeed)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                yield return new WaitForSecondsRealtime(1.0f / fixedFPSSpeed);
            }
        }
    }
}
