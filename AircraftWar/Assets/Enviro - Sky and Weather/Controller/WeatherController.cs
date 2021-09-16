using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YizhaoCode.EnviroExtensions
{
    public class WeatherController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other) 
        {
            Debug.Log("Collision with" + other);
            EnviroSkyMgr.instance.ChangeWeather(7);
        }
    }
}

