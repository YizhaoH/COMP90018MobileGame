using UnityEngine;
using System.Collections;


namespace GlobalSnowEffect {

				public class SimpleCameraMove : MonoBehaviour {
								public float cameraSensitivity = 150;
								public float climbSpeed = 20;
								public float normalMoveSpeed = 20;
								public float slowMoveFactor = 0.25f;
								public float fastMoveFactor = 3;
		
								private float rotationX = 0.0f;
								private float rotationY = 0.0f;

								public Transform target;
								public float maxCameraDistance = 50f;

								Quaternion startingRotation;
								bool freeCamera;

								void Start () {
												startingRotation = transform.rotation;
								}

								void FixedUpdate () {
												if (freeCamera) {
																rotationX += Input.GetAxis ("Mouse X") * cameraSensitivity * Time.deltaTime;
																rotationY += Input.GetAxis ("Mouse Y") * cameraSensitivity * Time.deltaTime;
																rotationY = Mathf.Clamp (rotationY, -90, 90);
			
																transform.localRotation = Quaternion.AngleAxis (rotationX, Vector3.up) * startingRotation;
																transform.localRotation *= Quaternion.AngleAxis (rotationY, Vector3.left);
												} else {
																if (target != null) {
																				Camera.main.transform.LookAt (target.transform.position);
																				if (Vector3.Distance (target.transform.position, Camera.main.transform.position) > maxCameraDistance) {
																								Camera.main.transform.position = target.transform.position - Camera.main.transform.forward * maxCameraDistance;
																				}
																}
												}
			
												if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
																transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis ("Vertical") * Time.deltaTime;
																transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis ("Horizontal") * Time.deltaTime;
												} else if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl)) {
																transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis ("Vertical") * Time.deltaTime;
																transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis ("Horizontal") * Time.deltaTime;
												} else {
																transform.position += transform.forward * normalMoveSpeed * Input.GetAxis ("Vertical") * Time.deltaTime;
																transform.position += transform.right * normalMoveSpeed * Input.GetAxis ("Horizontal") * Time.deltaTime;
												}
			
												if (Input.GetKey (KeyCode.Q)) {
																transform.position -= transform.up * climbSpeed * Time.deltaTime;
												}
												if (Input.GetKey (KeyCode.E)) {
																transform.position += transform.up * climbSpeed * Time.deltaTime;
												}

												if (transform.position.y < 1f) {
																transform.position = new Vector3 (transform.position.x, 1f, transform.position.z);
												}
			
								}

								void Update () {
												if (Input.GetKeyDown (KeyCode.Escape)) {
																freeCamera = !freeCamera;
																startingRotation = transform.rotation;
												}
								}


				}
}