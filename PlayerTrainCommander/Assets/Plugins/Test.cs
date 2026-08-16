using UnityEngine;
using PTC.Core;
using PTC.Core.Loader;
using System;
using System.IO;

public class Test : MonoBehaviour
{

    [SerializeField]
    private string pass = @"C:\Users\yokus\Documents\GitHub\PlayerTrainCommander\SampleJson\Station\Station.json";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("動いた" + System.IO.File.Exists(pass));
        FileStream fileStream = new FileStream(pass, FileMode.Open, FileAccess.Read);

        Debug.Log(fileStream == null);

        StationFile statonFile = PTC.Core.Loader.StationFile.FromJson(fileStream);

        Debug.Log(statonFile.ToString());

        ReadOnlySpan<Station> stationlist = statonFile.GetStationList();

        Debug.Log(stationlist.Length);

        fileStream.Dispose();

        foreach (var item in stationlist)
        {
            Debug.Log(item.Id + ":" + item.Name);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
