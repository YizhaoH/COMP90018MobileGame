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

    private HealthSystem healthSystem;
    public bool isDead = false;

    public ScoreManager scoreManager;

    public void Setup(HealthSystem hs)
    {
        this.healthSystem = hs;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        Debug.Log("flock agent health change!");
        if(healthSystem.getHealth()<=0)
        {
            transform.Find("Model").gameObject.SetActive(false);
            EnalbeParticleSystem("PS_Destory");
            if (!isDead)
            {
                scoreManager.score += 1;
            }
            Dead();
            
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        agentCollider = GetComponent<Collider>();

        scoreManager = GameObject.FindWithTag("Score").GetComponent<ScoreManager>();
        if (scoreManager == null)
         {
            Debug.Log("null");
         }
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

    public void EnalbeParticleSystem(string name)
    {
        Transform child = this.transform.Find(name);
        ParticleSystem ps = child.GetComponent<ParticleSystem>();
        if(ps)
        {
            ps.Play();
        }
    }

    public void Dead()
    {
        isDead = true;
        Destroy(this.gameObject, 1f);
    }


}

