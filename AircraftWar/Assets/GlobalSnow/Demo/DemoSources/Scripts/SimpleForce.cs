using UnityEngine;
using System.Collections;


namespace GlobalSnowEffect {


				public class SimpleForce : MonoBehaviour {

								public float speed = 5f;

								Rigidbody rb;

								void Start() {
												rb = GetComponent<Rigidbody>();
								}

								void FixedUpdate () {
												Vector3 dir = Vector3.zero;
												if (Input.GetKey(KeyCode.A)) {
																dir = -Camera.main.transform.right;
																dir.y = 0;
												} else if (Input.GetKey(KeyCode.D)) {
																dir = Camera.main.transform.right;
																dir.y = 0;
												} else if (Input.GetKey(KeyCode.W)) {
																dir = Camera.main.transform.forward;
																dir.y = 0;
												} else if (Input.GetKey(KeyCode.S)) {
																dir = -Camera.main.transform.forward;
																dir.y = 0;
												} else if (Input.GetKey(KeyCode.Space)) {
																dir = Vector3.up;
												}
												rb.AddForce(dir.normalized * speed, ForceMode.Impulse);
			
								}


				}
}