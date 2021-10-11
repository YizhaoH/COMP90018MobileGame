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

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider>();
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    public void Move(Vector3 velocity)
    {
        transform.position += velocity * Time.deltaTime;
        //transform.forward = velocity;

        float step = rotateSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, velocity.normalized, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
