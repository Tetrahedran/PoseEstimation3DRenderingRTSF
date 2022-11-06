using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAfter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Pause());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Pause()
    {
        yield return new WaitForSeconds(10);
        Debug.Break();
    }
}
