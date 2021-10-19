using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Missle : MonoBehaviour
{
    [Header("Setup")]
    public GameObject RocketTarget;
    public Rigidbody RocketRgb;

    public float duration=5;

    public float turnSpeed = 3f;
    public float rocketFlySpeed = 100f;

    private PlayerControl playerControl;
    private Transform rocketLocalTrans;

    // Start is called before the first frame update
    void Start()
    {
        RocketTarget = GameObject.FindWithTag("Player");
        if (!RocketTarget)
            Debug.Log("Please set the Rocket Target");
        
        rocketLocalTrans = GetComponent<Transform>();
        playerControl = RocketTarget.GetComponent<PlayerControl>();
        if(!playerControl)
            Debug.Log("Can not find playerControl");
    }


    private void FixedUpdate()
    {
        if (!RocketRgb) //If we have not set the Rigidbody, do nothing..
            return;

        //RocketRgb.velocity = rocketLocalTrans.forward * rocketFlySpeed;

        //Now Turn the Rocket towards the Target
        //var rocketTargetRot = Quaternion.LookRotation(RocketTarget.transform.position - rocketLocalTrans.position);

        //RocketRgb.MoveRotation(Quaternion.RotateTowards(rocketLocalTrans.rotation, rocketTargetRot, turnSpeed));

        float dis = (RocketTarget.transform.position - this.transform.position).sqrMagnitude;
        if(dis>100)
        {
            this.transform.LookAt(RocketTarget.transform);
            this.transform.position += (RocketTarget.transform.position - this.transform.position).normalized * rocketFlySpeed * Time.deltaTime;
        }
        else
        {
            RocketRgb.velocity = rocketLocalTrans.forward * rocketFlySpeed;
            var rocketTargetRot = Quaternion.LookRotation(RocketTarget.transform.position - rocketLocalTrans.position);
            RocketRgb.MoveRotation(Quaternion.RotateTowards(rocketLocalTrans.rotation, rocketTargetRot, turnSpeed));
            Destroy(this.gameObject, 0.5f);
        }

        Destroy(this.gameObject, duration);
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Missle hit player");
            Rigidbody plRgb = collision.gameObject.GetComponent<Rigidbody>();
            if (plRgb)
                plRgb.AddForceAtPosition(this.transform.forward * 15f, plRgb.position);

            //Deactivate Rocket..
            //this.gameObject.SetActive(false);
            if(Random.Range(0,1000) < 500)
                playerControl.EnalbeParticleSystem("getHitLeft");
            else
                playerControl.EnalbeParticleSystem("getHitRight");
            Destroy(this.gameObject);
        }
    }

}
