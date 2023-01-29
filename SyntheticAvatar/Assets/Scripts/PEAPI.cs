using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;
using UnityEditor;
using Proyecto26;

[RequireComponent(typeof(FullProxyMover))]
public class PEAPI : MonoBehaviour
{
    public event EventHandler AfterDataReceived;

    public string ip = "192.168.5.1";
    public int peServerPort = 5000;

    public bool fixedSpeed;
    [Range(5, 120)]
    public int fixedFPSSpeed;

    private FullProxyMover mover;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<FullProxyMover>();
        StartCoroutine(PE_Server());
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    private IEnumerator PE_Server()
    {
        while (true)
        {
            if (!EditorApplication.isPaused)
            {
                string url = $"http://{ip}:{peServerPort}/estimation";
                RestClient.Get(url)
                    .Then(response =>
                    {
                        JObject obj = JObject.Parse(response.Text);
                        ProxyBone[] values = Enum.GetValues(typeof(ProxyBone)) as ProxyBone[];
                        Array.Sort(values);
                        foreach (ProxyBone bone in values)
                        {
                            if (obj.ContainsKey(bone.ToString()))
                            {
                                float[] pos = obj.Value<JArray>(bone.ToString()).ToObject<float[]>();
                                Vector3 vec = new Vector3(pos[0], pos[1], pos[2]);
                                mover.move(bone, vec);
                            }
                        }
                        AfterDataReceived?.Invoke(this, null);
                    }).Catch(err =>
                    {
                        Debug.LogWarning($"Couldn't connect to {url} because of error: {err.Message}");
                    });
            }
            if (!fixedSpeed)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                yield return new WaitForSeconds(1.0f / fixedFPSSpeed);
            }
        }
    }
}
