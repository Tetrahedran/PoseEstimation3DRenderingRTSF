using UnityEngine;
using System.Collections;

/**
 * Implementation according to https://answers.unity.com/questions/20337/flipmirror-camera.html
 */
[RequireComponent(typeof(Camera))]
public class CameraFlip : MonoBehaviour
{
    new Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    void OnPreCull()
    {
        Matrix4x4 scale;
        if (camera.aspect > 2)
        {
            scale = Matrix4x4.Scale(new Vector3(-1, 1, 1));
        }
        else
        {
            scale = Matrix4x4.Scale(new Vector3(-1, 1, 1));
        }
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        camera.projectionMatrix = camera.projectionMatrix * scale;
    }
    void OnPreRender()
    {
        GL.invertCulling = true;
    }
    void OnPostRender()
    {
        GL.invertCulling = false;
    }
}