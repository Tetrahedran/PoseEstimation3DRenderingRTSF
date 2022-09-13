using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Proyecto26;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;

public class API : MonoBehaviour
{
    public string ip = "192.168.5.1";
    public int port = 8080;

    public ProxyMover mover;

    // Start is called before the first frame update
    void Start()
    {
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
            RestClient.Get($"http://{ip}:{port}/test")
                .Then(response => {
                    JObject obj = JObject.Parse(response.Text);
                    ProxyMover.ProxyBone[] values = Enum.GetValues(typeof(ProxyMover.ProxyBone)) as ProxyMover.ProxyBone[];
                    Array.Sort(values);
                    foreach (ProxyMover.ProxyBone bone in values)
                    {
                        if (obj.ContainsKey(bone.ToString()))
                        {
                            float[] pos = obj.Value<JArray>(bone.ToString()).ToObject<float[]>();
                            Vector3 vec = new Vector3(pos[0], pos[1], pos[2]);
                            mover.move(bone, vec);
                        }
                    }
                });
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
