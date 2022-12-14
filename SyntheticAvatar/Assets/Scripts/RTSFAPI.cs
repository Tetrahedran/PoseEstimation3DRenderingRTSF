using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;
using UnityEditor;
using Proyecto26;

[RequireComponent(typeof(FrameExporter))]
[RequireComponent(typeof(FullProxyMover))]
public class RTSFAPI : MonoBehaviour
{
    public string ip = "192.168.5.1";
    public int rtsfServerPort = 5001;

    private FrameExporter exporter;

    // Start is called before the first frame update
    void Start()
    {
        exporter = GetComponent<FrameExporter>();
        StartCoroutine(RTSF_Server());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator RTSF_Server()
    {
        while (true)
        {
            if (!EditorApplication.isPaused)
            {
                byte[] img = exporter.SaveCameraView();
                string url = $"http://{ip}:{rtsfServerPort}/img";
                RestClient.Request(new RequestHelper
                {
                    Uri = url,
                    Method = "POST",
                    UploadHandler = new UploadHandlerRaw(img)
                }).Then(response => { }).Catch(err =>
                {
                    Debug.LogWarning($"Couldn't connect to {url} because of error: {err.Message}");
                });
                yield return new WaitForEndOfFrame();

                /*using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
                {
                    www.disposeUploadHandlerOnDispose = true;
                    www.disposeDownloadHandlerOnDispose = true;
                    UploadHandler handler = new UploadHandlerRaw(img);
                    www.uploadHandler = handler;
                    yield return www.SendWebRequest();
                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogWarning($"Couldn't connect to {url} because of error: {www.error}");
                    }
                    www.Dispose();
                    yield return new WaitForEndOfFrame();
                }*/
            }
        }
    }
}
