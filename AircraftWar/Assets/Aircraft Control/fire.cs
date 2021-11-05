using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire : MonoBehaviour
{
    public GameObject theBullet;
    public Transform theMuzzle1;
    public Transform theMuzzle2;

    public AudioSource shootAudio;

    // Start is called before the first frame update
    void Start()
    {
        shootAudio.volume = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void onFire() {
        GameObject bulletClone1;
        GameObject bulletClone2;
        
        bulletClone1 = Instantiate(theBullet, theMuzzle1.position, theMuzzle1.rotation);
        //bulletClone1.velocity = transform.TransformDirection(Vector3.forward * speed);
        //Debug.Log("BulletCLone01 is null? " + (bulletClone1 == null).ToString());
        MoveBullet bullet01 = bulletClone1.GetComponent<MoveBullet>();
        bullet01.hitPoint = this.transform.forward + theMuzzle1.transform.position;
        bullet01.isPlayer = true;
        bullet01.speed = 50000;
        bullet01.transform.parent = this.transform;

        bulletClone2 = Instantiate(theBullet, theMuzzle2.position, theMuzzle1.rotation);
        //bulletClone2.velocity = transform.TransformDirection(Vector3.forward * speed);
        //Debug.Log("BulletCLone02 is null? " + (bulletClone1 == null).ToString());
        MoveBullet bullet02 = bulletClone2.GetComponent<MoveBullet>();
        //Debug.Log("BulletCLone02 is null? " + (bulletClone1 == null).ToString());
        bullet02.hitPoint =  this.transform.forward + theMuzzle2.transform.position;
        bullet02.isPlayer = true;
        bullet02.speed = 50000;
        bullet02.transform.parent = this.transform;

        shootAudio.Play();

    }
}
