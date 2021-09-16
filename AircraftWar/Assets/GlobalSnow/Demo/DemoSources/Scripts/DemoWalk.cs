using UnityEngine;
using System.Collections;

namespace GlobalSnowEffect {
    public class DemoWalk : MonoBehaviour {
        GlobalSnow snow;

        void Start() {
            snow = GlobalSnow.instance;
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.T)) {
                snow.enabled = !snow.enabled;
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                Camera cam = Camera.main;
                Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {
                    //GlobalSnow.instance.MarkSnowAt(hit.point, 3f);
                    //Debug.Log(Vector3.Distance(cam.transform.position, hit.point) + " " + hit.distance);
                    GlobalSnow.instance.FootprintAt(hit.point, cam.transform.forward);
                }
            }


        }
    }
}