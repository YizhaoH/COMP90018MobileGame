using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class GPSLocation : MonoBehaviour
{
    //Get a Google API Key from https://developers.google.com/maps/documentation/geocoding/get-api-key
    public string GPSStatus;
    public float latitudeValue;
    public float longitudeValue;
    public float altitudeValue;
    public float horizontalAccuracyValue;
    public double timeStampValue;
    public string key = "61660ffdadcdcb5184827cbd78634d92";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GPSLoc());
    }

    IEnumerator GPSLoc()
    {
        yield return new WaitForSeconds(20);
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("222");
            yield break;
        }
        //start service before querying location
        Input.location.Start();

        //wait until service initialize
        int maxWait = 15;
        while(Input.location.status == LocationServiceStatus.Initializing && maxWait>0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        //service didnt init in 15 sec
        if(maxWait<1)
        {
            GPSStatus = "Time out";
            yield break;
        }
        if(Input.location.status == LocationServiceStatus.Failed)
        {
            GPSStatus = "Connnection failed";
            yield break;
        }
        GPSStatus = "Running";
        //InvokeRepeating("UpdateGPSData", 0.5f, 1f);
        Invoke("UpdateGPSData", 0.5f);
    }
    private void UpdateGPSData()
    {
        if(Input.location.status == LocationServiceStatus.Running)
        {
            GPSStatus = "Running";
            latitudeValue = Input.location.lastData.latitude;
            longitudeValue = Input.location.lastData.longitude;
            altitudeValue = Input.location.lastData.altitude;
            horizontalAccuracyValue = Input.location.lastData.horizontalAccuracy;
            timeStampValue = Input.location.lastData.timestamp;

            print(latitudeValue.ToString() + " " + longitudeValue.ToString());
            StartCoroutine(GetRequest(
                    "http://restapi.amap.com/v3/geocode/regeo?key=" + key + "&location=" + latitudeValue.ToString() + "," + longitudeValue.ToString()));
        }
    }
    IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                string[] pages = uri.Split('/');
                int page = pages.Length - 1;
                if (webRequest.isNetworkError)
                {
                    // Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else
                {
                    Debug.Log(webRequest.downloadHandler.text);
                    //Debug.LogError("rn" + jd["regeocode"]["formatted_address"].ToString());
                }
            }
        }
}
