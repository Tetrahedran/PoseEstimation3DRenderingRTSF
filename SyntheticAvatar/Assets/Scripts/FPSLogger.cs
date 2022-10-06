using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FPSLogger : MonoBehaviour
{
    [Range(1, 30)]
    public int averageOverXFrames = 1;

    private List<float> fps;

    // Start is called before the first frame update
    void Start()
    {
        fps = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        float fps = 1.0f / Time.unscaledDeltaTime;
        this.fps.Add(fps);
        if(this.fps.Count >= averageOverXFrames)
        {
            Debug.Log($"Currently {fps} fps");
            float avg_fps = this.fps.Average();
            Debug.Log($"Average {avg_fps} fps");
            this.fps.Clear();
        }
    }
}
