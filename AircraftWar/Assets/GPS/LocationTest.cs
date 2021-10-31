using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Android;
public class LocationTest : MonoBehaviour
    {
        private string longitude;//经度
        private string latitude;//纬度
        void Start() => StartCoroutine(StartGPS());
        IEnumerator StartGPS()
        {
            //Unity给我们提供的一个相关权限类 Permission，可以判断当前相关权限是否开启
            #if UNITY_ANRDOID
            // Android code goes here
            if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                //如果没有开启就提示开启权限
                Permission.RequestUserPermission(Permission.FineLocation);
            }
            #endif
            Debug.LogError("开始获取GPS信息");
            // 检查位置服务是否可用  
            if (!Input.location.isEnabledByUser)
            {
                Debug.Log("位置服务不可用");
                yield break;
            }
            // 查询位置之前先开启位置服务
            Debug.Log("启动位置服务");
            Input.location.Start();
            // 等待服务初始化  
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                Debug.Log(Input.location.status.ToString() + ">>>" + maxWait.ToString());
                yield return new WaitForSeconds(1);
                maxWait--;
            }
            // 服务初始化超时  
            if (maxWait < 1)
            {
                Debug.Log("服务初始化超时");
                yield break;
            }
            // 连接失败  
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("无法确定设备位置");
                yield break;
            }
            else
            {
                Debug.Log("Location:  \n" +
                    "纬度：" + Input.location.lastData.latitude + " \n" +
                           "经度：" + Input.location.lastData.longitude + " \n" +
                           "海拔：" + Input.location.lastData.altitude + " \n" +
                           "水平精度：" + Input.location.lastData.horizontalAccuracy + " \n" +
                           "垂直精度：" + Input.location.lastData.verticalAccuracy + " \n" +
                           "时间戳：" + Input.location.lastData.timestamp);
                longitude = Input.location.lastData.longitude.ToString();
                latitude = Input.location.lastData.latitude.ToString();
                //StartCoroutine(GetRequest(
                //    "http://restapi.amap.com/v3/geocode/regeo?key=" + key + "&location=" + longitude + "," + latitude));
            }
            // 停止服务，如果没必要继续更新位置，（为了省电
            Input.location.Stop();
        }
        string key = "";       //去高德地图开发者申请
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

