using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FrameExporter : MonoBehaviour
{
    [SerializeField]
    private Camera screenCaptureCamera;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public byte[] SaveCameraView()
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
        RenderTexture target = screenCaptureCamera.targetTexture;
        screenCaptureCamera.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        screenCaptureCamera.Render();
        Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        RenderTexture.active = active;
        screenCaptureCamera.targetTexture = target;
        byte[] image = renderedTexture.EncodeToPNG();
        Destroy(screenTexture);
        Destroy(renderedTexture);
        return image;
    }
}
