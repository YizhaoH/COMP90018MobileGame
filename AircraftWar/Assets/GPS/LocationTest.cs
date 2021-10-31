using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Android;
public class LocationTest : MonoBehaviour
    {
        private string longitude;//����
        private string latitude;//γ��
        void Start() => StartCoroutine(StartGPS());
        IEnumerator StartGPS()
        {
            //Unity�������ṩ��һ�����Ȩ���� Permission�������жϵ�ǰ���Ȩ���Ƿ���
            #if UNITY_ANRDOID
            // Android code goes here
            if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                //���û�п�������ʾ����Ȩ��
                Permission.RequestUserPermission(Permission.FineLocation);
            }
            #endif
            Debug.LogError("��ʼ��ȡGPS��Ϣ");
            // ���λ�÷����Ƿ����  
            if (!Input.location.isEnabledByUser)
            {
                Debug.Log("λ�÷��񲻿���");
                yield break;
            }
            // ��ѯλ��֮ǰ�ȿ���λ�÷���
            Debug.Log("����λ�÷���");
            Input.location.Start();
            // �ȴ������ʼ��  
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                Debug.Log(Input.location.status.ToString() + ">>>" + maxWait.ToString());
                yield return new WaitForSeconds(1);
                maxWait--;
            }
            // �����ʼ����ʱ  
            if (maxWait < 1)
            {
                Debug.Log("�����ʼ����ʱ");
                yield break;
            }
            // ����ʧ��  
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("�޷�ȷ���豸λ��");
                yield break;
            }
            else
            {
                Debug.Log("Location:  \n" +
                    "γ�ȣ�" + Input.location.lastData.latitude + " \n" +
                           "���ȣ�" + Input.location.lastData.longitude + " \n" +
                           "���Σ�" + Input.location.lastData.altitude + " \n" +
                           "ˮƽ���ȣ�" + Input.location.lastData.horizontalAccuracy + " \n" +
                           "��ֱ���ȣ�" + Input.location.lastData.verticalAccuracy + " \n" +
                           "ʱ�����" + Input.location.lastData.timestamp);
                longitude = Input.location.lastData.longitude.ToString();
                latitude = Input.location.lastData.latitude.ToString();
                //StartCoroutine(GetRequest(
                //    "http://restapi.amap.com/v3/geocode/regeo?key=" + key + "&location=" + longitude + "," + latitude));
            }
            // ֹͣ�������û��Ҫ��������λ�ã���Ϊ��ʡ��
            Input.location.Stop();
        }
        string key = "";       //ȥ�ߵµ�ͼ����������
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

