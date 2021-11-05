using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleGun : Weapon
{
    public float cooldownSpeed=5f;

    public float fireRate=5f;

    private float shootCooldown = 3;
    private bool canShoot = true;

    public GameObject missle;

    public GameObject shootPointObj01;
    Vector3 shootPoint01;
    public GameObject shootPointObj02;
    Vector3 shootPoint02;
    public AudioSource missleAudio;

    void Start() {
        missleAudio.volume = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownSpeed += Time.deltaTime * 60f;
        if(attack)
        {
            shootPoint01 = shootPointObj01.transform.position;
            shootPoint02 = shootPointObj02.transform.position;
            if (cooldownSpeed >= fireRate && canShoot)
            {
                
                Shoot();
                cooldownSpeed = 0f;
                    
                //gunshot.PlayOneShot(singleShot);
                StartCoroutine(CoolDownFunction());
            }
        }
    }

    IEnumerator CoolDownFunction()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
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
            //Debug.Log("Missle Shoot Shoot Shoot!");
            
            GameObject tempMissle01 = Instantiate(missle, shootPoint01, fireRotation);
            tempMissle01.transform.parent = this.transform;
            GameObject tempMissle02 = Instantiate(missle, shootPoint01, fireRotation);
            tempMissle02.transform.parent = this.transform;

            missleAudio.Play();
        }
    }
}
