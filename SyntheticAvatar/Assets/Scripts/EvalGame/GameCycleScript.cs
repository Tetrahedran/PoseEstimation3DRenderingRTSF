using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System;
using System.IO;
using Proyecto26;
using Newtonsoft.Json.Linq;

[RequireComponent(typeof(SpawnerScript))]
public class GameCycleScript : MonoBehaviour
{
    public TextMeshProUGUI CenterText;
    public TextMeshProUGUI RightText;
    public TextMeshProUGUI LeftText;
    public GameObject panel;
    public int numberOfObjects;
    [Range(1,25)]
    public int breakEvery;

    [SerializeField]
    private HandController leftHand;
    [SerializeField]
    private HandController rightHand;

    private float idealDistance;

    private bool listenForKeyInput;
    private SpawnerScript spawner;
    private int instanceCount;
    private int networksCount;
    private List<DistTime> deltas;
    private int[] networks;

    struct DistTime
    {
        public int network;
        public float dT;
        public float dS;
        public float idealdS;
    }

    // Start is called before the first frame update
    void Start()
    {
        string url = $"http://127.0.0.1:5000/available";
        RestClient.Get(url).Then(response =>
        {
            JArray available = JArray.Parse(response.Text);
            networks = available.ToObject<int[]>();
        }).Then(() =>
        {
            displayStartMessage();
        }).Catch(err =>
        {
            Debug.LogWarning($"Couldn't connect to {url} because of error: {err.Message}");
        }); ;
        listenForKeyInput = false;
        networksCount = 0;
        deltas = new List<DistTime>();
        spawner = GetComponent<SpawnerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (listenForKeyInput)
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                PlayGame(false);
            }
        }
    }

    public void Hit(float delta, float dist, GameObject calling)
    {
        rightHand.ResetMeassurement();
        leftHand.ResetMeassurement();
        int networkId = networks[networksCount];
        DistTime dsdt = new DistTime();
        dsdt.dT = delta;
        dsdt.dS = dist;
        dsdt.network = networkId;
        dsdt.idealdS = idealDistance;

        Debug.Log("Time: " + delta);
        Debug.Log("Distance: " + dist);
        deltas.Add(dsdt);
        Destroy(calling);
        GameLoop();
    }

    private void displayStartMessage()
    {
        panel.SetActive(true);
        RightText.gameObject.SetActive(false);
        LeftText.gameObject.SetActive(false);
        CenterText.text = "Press Enter to start";
        listenForKeyInput = true;
    }

    private void PlayGame(bool pause = true)
    {
        instanceCount = 0;
        selectNetwork();
        if(pause) Debug.Break();
        panel.SetActive(true);
        StartCoroutine(countDown(5));
    }

    private IEnumerator countDown(int seconds)
    {
        listenForKeyInput = false;
        for(int i = seconds; i >= 0; i--)
        {
            CenterText.text = "" + i;
            yield return new WaitForSeconds(1);
        }
        panel.SetActive(false);
        RightText.gameObject.SetActive(true);
        LeftText.gameObject.SetActive(true);
        GameLoop();
    }

    private void GameLoop()
    {
        if(instanceCount < numberOfObjects)
        {
            StartCoroutine(SpawnOne());
            instanceCount++;
        }
        else
        {
            networksCount++;

            if (networksCount >= networks.Length)
            {
                if (!Directory.Exists(Application.dataPath + "/Meassurements/"))
                {
                    Directory.CreateDirectory(Application.dataPath + "/Meassurements/");
                }
                Hash128 hash = new Hash128();
                hash.Append(DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
                string fileName = hash.ToString();
                String path = Application.dataPath + "/Meassurements/" + fileName + ".csv";
                using (StreamWriter writer = File.AppendText(path))
                {
                    writer.WriteLine("Network;dT;dS;Ideal dS");

                    foreach (DistTime delta in deltas)
                    {
                        writer.WriteLine(delta.network + ";" + delta.dT + ";" + delta.dS + ";" + delta.idealdS);
                    }

                    writer.Flush();
                }
                networksCount = 0;
                Debug.Log($"Saved to {fileName}.csv");
                Debug.Log("GameDone");
                deltas = new List<DistTime>();
                displayStartMessage();
            }
            else
            {
                PlayGame(networksCount % breakEvery == 0);
            }
        }
    }

    private void selectNetwork()
    {
        int networkID = networks[networksCount];
        string url = $"http://127.0.0.1:5000/switch/{networkID}";
        RestClient.Post(url, null).Then(response => 
        {
            LeftText.text = "" + networkID;
        }).Catch(err =>
        {
            Debug.LogWarning($"Couldn't connect to {url} because of error: {err.Message}");
        }); ;
    }

    private IEnumerator SpawnOne()
    {
        yield return new WaitForSeconds(.5f);
        Vector2 spawnPos = spawner.SpawnOne();
        RightText.text = instanceCount + "/" + numberOfObjects;
        rightHand.StartMeassurement();
        leftHand.StartMeassurement();
        Vector2 rHandPos = rightHand.transform.position;
        Vector2 lHandPos = leftHand.transform.position;

        float rHandDist = (spawnPos - rHandPos).magnitude;
        float lHandDist = (spawnPos - lHandPos).magnitude;

        idealDistance = rHandDist < lHandDist ? rHandDist : lHandDist;
        Debug.Log($"Ideal Distance is {idealDistance}");
    }
}
