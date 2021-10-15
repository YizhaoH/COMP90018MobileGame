using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    Flock agentFlock;
    public Flock AgentFlock { get{return agentFlock;} }

    public float rotateSpeed = 1.2f;
    Collider agentCollider;
    public Collider AgentCollider { get{return agentCollider;} }

    public Gun gun;
    public MissleGun missleGun;

    // Start is called before the first frame update
    void OnEnable()
    {
        agentCollider = GetComponent<Collider>();
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
        if(gun !=null)
        {
            gun = this.gameObject.GetComponent<Gun>();
            gun.maxDetectRange = agentFlock.playerDetectionRadius;
        }
        if(missleGun != null)
        {
            missleGun = this.gameObject.GetComponent<MissleGun>();
            missleGun.maxDetectRange = agentFlock.playerDetectionRadius;
        }
    }

    public void Move(Vector3 velocity)
    {
        transform.position += velocity * Time.deltaTime;
        //transform.forward = velocity;

        float step = rotateSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, velocity.normalized, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    public float DistanceToTarget(GameObject target)
    {
        float dis = Vector3.Distance(target.transform.position, this.transform.position);
        return dis;
    }

    public float AngleWithPlayer(GameObject target)
    {
        Vector3 vector = (target.transform.position - this.transform.position).normalized;
        float angle = Vector3.Angle(vector, transform.forward);
        return Mathf.Abs(angle);
    }
}

