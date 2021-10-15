using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleGun : Weapon
{
    public float cooldownSpeed=5f;

    public float fireRate=5f;

    public float recoilCooldown=1f;

    public GameObject missle;

    public GameObject shootPointObj01;
    Vector3 shootPoint01;
    public GameObject shootPointObj02;
    Vector3 shootPoint02;
    public AudioSource gunshot;

    public AudioClip singleShot;


    // Update is called once per frame
    void Update()
    {
        cooldownSpeed += Time.deltaTime * 30f;
        if(attack)
        {
            shootPoint01 = shootPointObj01.transform.position;
            shootPoint02 = shootPointObj02.transform.position;
            if (cooldownSpeed >= fireRate)
            {
                Shoot();
                //gunshot.PlayOneShot(singleShot);
                cooldownSpeed = 0;
                recoilCooldown = 1;
            }
        }
    }


    protected override void Shoot()
    {
        RaycastHit hit;

        Quaternion fireRotation = Quaternion.LookRotation(transform.forward);

        fireRotation = Quaternion.identity;
    
        if (Physics.Raycast(transform.position, fireRotation * Vector3.forward, out hit, maxDetectRange))
        {
            if(hit.transform.tag == "Enemy" || hit.transform.tag=="Boundary")
            {
                return;
            }
            Debug.Log("Missle Shoot Shoot Shoot!");
            GameObject tempMissle = Instantiate(missle, shootPoint01, fireRotation);
            tempMissle.transform.parent = this.transform;
        
        }
    }
}
