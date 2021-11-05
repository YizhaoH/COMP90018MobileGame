using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public float cooldownSpeed=1f;

    public float fireRate=5f;

    public float recoilCooldown=1f;

    private float accuracy=0;

    public float maxSpreadAngle=15f;

    public float timeTillMaxSpread=5f;

    public GameObject bullet;

    public GameObject shootPointObj;
    Vector3 shootPoint;

    private float shootCooldown = 0.2f;
    private bool canShoot = true;

    public AudioSource gunshot;

    private void Start() {
        gunshot.volume = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownSpeed += Time.deltaTime * 60f;
        if(attack)
        {
            accuracy += Time.deltaTime * 2f;
            shootPoint = shootPointObj.transform.position;
            if (cooldownSpeed >= fireRate && canShoot)
            {
                Shoot();
                //gunshot.PlayOneShot(singleShot);
                cooldownSpeed = 0;
                recoilCooldown = 1;

                StartCoroutine(CoolDownFunction());
            }
        }
        else
        {
            recoilCooldown -= Time.deltaTime;
            recoilCooldown = Mathf.Max(recoilCooldown, 0);
            if(recoilCooldown <= 1)
            {
                accuracy = 0.0f;
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

        float currentSpread = Mathf.Lerp(0.0f, maxSpreadAngle, accuracy / timeTillMaxSpread);

        fireRotation = Quaternion.RotateTowards(fireRotation, Random.rotation, Random.Range(0.0f, currentSpread));
        
        //Debug.Log("Bullet Shoot Shoot Shoot!");
        if (Physics.Raycast(transform.position, fireRotation * Vector3.forward, out hit, maxDetectRange))
        {
            if(hit.transform.tag == "Enemy" || hit.transform.tag=="Boundary")
            {
                return;
            }
            GameObject tempBullet = Instantiate(bullet, shootPoint, fireRotation);

            tempBullet.transform.parent = this.transform;
            //Debug.Log("Hit tag: " + hit.transform.tag.ToString());
            gunshot.Play();

            if(hit.transform.tag=="Player")
            {
                tempBullet.GetComponent<MoveBullet>().hitPoint = hit.transform.position + hit.transform.forward * 35;
            }
            else
            {
                tempBullet.GetComponent<MoveBullet>().hitPoint = hit.point;
            }
            
            tempBullet.GetComponent<MoveBullet>().direction = hit.point - shootPoint;
        }
    }
}
