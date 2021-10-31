/***************************************************************************************************************
This gist documents reverse geolocation by using Google Maps Geocoding API, assuming that the player has his/her
location enabled on the phone.
This is done by finding the player's longitude and latitude using:
1) Unity's LocationService (https://docs.unity3d.com/ScriptReference/LocationService.html)
2) Google Map's Geocoding API (https://developers.google.com/maps/documentation/geocoding/intro#reverse-example)
Reverse geolocation will use the player's longitude and latitude to extract relevant information, 	
one of which is the player's country, which will be demonstrated here.
Note that you will need a key/authentication to successfully request information.
Get your key through https://developers.google.com/maps/documentation/geocoding/get-api-key
****************************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Android;
//using UnityEditor.Json;		//Need this for JSON Deserialization

public class GeolocationManager : MonoBehaviour {
	//Get a Google API Key from https://developers.google.com/maps/documentation/geocoding/get-api-key
	public string GoogleAPIKey; 
	
	public string latitude;
	public string longitude;
	public string countryLocation;

	IEnumerator Start()
  	{
        // First, check if user has location service enabled
        yield return new WaitForSeconds(3);
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("222");
            yield break;
        }
		// Start service before querying location
		Input.location.Start();

		// Wait until service initializes
		int maxWait = 3;
		
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
		  yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			Debug.Log("Unable to determine device location");
		    	yield break;
		}
		else
		{
            
		  // Access granted and location value could be retrieve
			longitude = Input.location.lastData.longitude.ToString();
			latitude = Input.location.lastData.latitude.ToString();
            
            Debug.Log(longitude+ " "+ latitude);
		}
        yield return new WaitForSeconds(20);

        //Stop retrieving location
        Input.location.Stop();
		

    }
}
