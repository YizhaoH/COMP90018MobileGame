using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MoveBullet : MonoBehaviour
{
    public Vector3 spawnPos;
    public Vector3 hitPoint;
    public Vector3 direction;

    public GameObject dirt;

    public GameObject blood;
    public float bulletMaxDis = 1200f;
    float sqrMaxDis;

    public int speed;
    Rigidbody rb;
    public bool isPlayer = false;


    //public AudioSource myShot;
    // Start is called before the first frame update
    void Start()
    {
        spawnPos = this.transform.position;
        sqrMaxDis = bulletMaxDis * bulletMaxDis;
        rb = this.GetComponent<Rigidbody>();
        rb.AddForce((hitPoint - this.transform.position).normalized * speed );
    }

    
    void FixedUpdate()
    {
        Vector3 currDirection = hitPoint - this.transform.position;
        float sqrDistance = (this.transform.position - spawnPos).sqrMagnitude;
        if ( Vector3.Angle(direction, currDirection)>120)
        {
            Destroy(this.gameObject);
        }
        if( sqrDistance > sqrMaxDis)
        {
            Destroy(this.gameObject);
        }
    }

    
    void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.CompareTag("Player") && !isPlayer)
        {
            //col.gameObject.GetComponent<Health>().currentHealth -= 20;
            //GameObject newBlood = Instantiate(blood, this.transform.position, this.transform.rotation);
            //newBlood.transform.parent = col.transform;
            //Debug.Log("Bullet Hit player!");
            
            Destroy(this.gameObject);
        }

        if(other.gameObject.CompareTag("Enemy") && isPlayer)
        {
            //Debug.Log("hit enemy");
            //play hit sound effect
            Destroy(this.gameObject);
        }

        if(other.gameObject.CompareTag("Terrain") && isPlayer)
        {
            //Debug.Log("hit terrain");
            //play hit sound effect
            Destroy(this.gameObject);
        }

    }
    
}