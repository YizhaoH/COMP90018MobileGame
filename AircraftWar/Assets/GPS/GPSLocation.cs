using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GPSLoc());
    }

    IEnumerator GPSLoc()
    {
        yield return new WaitForSeconds(3);
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
        }
    }

}
