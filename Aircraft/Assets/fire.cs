using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire : MonoBehaviour
{
    public Rigidbody theBullet;
    public Transform theMuzzle1;
    public Transform theMuzzle2;

    private int speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 200;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void onFire() {
        //for (int i = 0; i < Input.touchCount; i++)
        //{
        // Vector3 touch = Input.GetTouch(i).position;
        //if (touch.x > Screen.width / 2)
        //if (Input.GetKeyDown(KeyCode.J))
        //if (Input.GetKeyDown(KeyCode.J))
        //{
            //Debug.Log("Fire ");
            Rigidbody bulletClone1;
            Rigidbody bulletClone2;

            bulletClone1 = (Rigidbody)Instantiate(theBullet, theMuzzle1.position, theMuzzle1.rotation);
            bulletClone1.velocity = transform.TransformDirection(Vector3.forward * speed);

            bulletClone2 = (Rigidbody)Instantiate(theBullet, theMuzzle2.position, theMuzzle2.rotation);
            bulletClone2.velocity = transform.TransformDirection(Vector3.forward * speed);
            //if (bulletClone1.position.z - theMuzzle1.position.z  > 500)
            //{
            //    Destroy(bulletClone1);
            //}

            Debug.Log(bulletClone1.position.z);
       //}

        //}
    }
}
