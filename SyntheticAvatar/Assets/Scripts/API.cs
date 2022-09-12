using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Proyecto26;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class API : MonoBehaviour
{
    public string ip = "192.168.5.1";
    public int port = 8080;

    public ProxyMover mover;

    private Thread serveThread;

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
        //src: https://developer.mozilla.org/en-US/docs/Web/API/WebSockets_API/Writing_WebSocket_server
        Debug.Log("Async Thread running");
        while (true)
        {
            RestClient.Get($"http://{ip}:{port}/test")
                .Then(response => {
                    JObject obj = JObject.Parse(response.Text);
                    int[] first = obj.Value<JArray>("1").ToObject<int[]>();
                    Vector3 f_vec = new Vector3(first[0], first[1], first[2]);
                    Debug.Log(f_vec);
                    mover.move(f_vec);
                });
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
