using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PEAPI))]
public class Calibration : MonoBehaviour
{
    [SerializeField]
    [Range(10, 250)]
    private int Samples;

    [SerializeField]
    private Transform HipProxy;

    [SerializeField]
    private Transform SpineProxy;

    [SerializeField]
    private Transform NeckProxy;

    [SerializeField]
    private Transform HeadProxy;

    [SerializeField]
    private Transform ProxyObjectWrapper;

    [SerializeField]
    [Range(0, 90)]
    private float AngularTPoseConstraint;

    [SerializeField]
    [Range(0,60)]
    private int FirstCalibrationAfter;

    private bool PerformCalibration;

    private List<List<Vector3>> data;

    private Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        data = new List<List<Vector3>>();
        TriggerCalibration(FirstCalibrationAfter);
        PEAPI api = GetComponent<PEAPI>();
        api.AfterDataReceived += this.AfterDataReceived;
    }

    // Update is called once per frame
    void AfterDataReceived(object sender, EventArgs args)
    {
        if (PerformCalibration)
        {
            if(data.Count < Samples)
            {
                List<Vector3> SingleStepData = GetCalibrationVectorsForSingleStep();
                if (DataSatisfyAngularConstraint(SingleStepData))
                {
                    data.Add(SingleStepData);
                }
            }
            else if(data.Count == Samples)
            {
                Vector3 AverageRotation = GetAverageRotation(Vector3.up);
                Debug.Log("Adjusting character proxy by following angles: " + AverageRotation);

                Vector3 ProxyWrapperRotation = ProxyObjectWrapper.rotation.eulerAngles;
                ProxyWrapperRotation += AverageRotation;
                Quaternion NewRotation = Quaternion.Euler(ProxyWrapperRotation);
                ProxyObjectWrapper.rotation = NewRotation;

                PerformCalibration = false;
                data = new List<List<Vector3>>();
            }
        }
    }

    public List<Vector3> GetCalibrationVectorsForSingleStep()
    {
        List<Vector3> results = new List<Vector3>();

        Vector3 Hip = HipProxy.position;
        Vector3 Spine = SpineProxy.position;
        Vector3 Neck = NeckProxy.position;
        Vector3 Head = HeadProxy.position;

        results.Add(Spine - Hip);
        results.Add(Neck - Spine);
        results.Add(Head - Neck);

        return results;
    }

    public bool DataSatisfyAngularConstraint(List<Vector3> Data)
    {
        for(int i = 1; i < Data.Count; i++)
        {
            if(Data[i - 1].Equals(Vector3.zero) || Data[i].Equals(Vector3.zero))
            {
                Debug.LogWarning("Character not fully tracked");
                return false;
            }
            float angle = Vector3.Angle(Data[i - 1], Data[i]);
            if(angle > AngularTPoseConstraint)
            {
                Debug.LogWarning("Character not satisfying angular constraint");
                return false;
            }
        }

        return true;
    }

    public Vector3 GetAverageRotation(Vector3 reference)
    {
        Vector3 Sum = Vector3.zero;
        int Length = 0;

        foreach(List<Vector3> SingleCycleData in data)
        {
            foreach(Vector3 Date in SingleCycleData)
            {
                Vector3 Rotation = Quaternion.FromToRotation(Date.normalized, reference.normalized).eulerAngles;
                if (Rotation.x > 180) Rotation.x -= 360;
                if (Rotation.y > 180) Rotation.y -= 360;
                if (Rotation.z > 180) Rotation.z -= 360;
                Sum += Rotation;
                Length++;
            }
        }

        return Sum / Length;
    }

    public void TriggerCalibration(int InSeconds)
    {
        if(coroutine == null)
        {
            coroutine = StartCoroutine(TriggerCalibrationCoroutine(InSeconds));
        }
        else
        {
            Debug.LogWarning("Calibration already requested");
        }
    }

    private IEnumerator TriggerCalibrationCoroutine(int InSeconds)
    {
        yield return new WaitForSeconds(InSeconds);
        Debug.Log("Starting Calibration");
        PerformCalibration = true;
        coroutine = null;
    }
}
