using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{

    public Transform pfhealthBar;
    public int startHealth = 100;
    public HealthSystem healthSystem;
    public Transform cam;
    public FlockAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").transform;
        //healthSystem = new HealthSystem();
        healthSystem.maxHealth = startHealth;
        healthSystem.health = startHealth;
        Transform healthBarTransform;
        if (this.transform.parent.gameObject.CompareTag("Player"))
        {
            healthBarTransform = Instantiate(pfhealthBar, new Vector3(0, -7 ,0) + this.transform.position, this.transform.parent.rotation).transform;
            healthBarTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else
            healthBarTransform = Instantiate(pfhealthBar, new Vector3(0, 15,0) + this.transform.position, this.transform.parent.rotation).transform;
        healthBarTransform.parent = this.transform.parent;
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.Setup(healthSystem);
        if(agent!=null)
            agent.Setup(healthSystem);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate() 
    {
        if (this.transform.parent.gameObject.CompareTag("Player"))
        {
            transform.LookAt(cam.transform.forward + transform.position);
        }
    }

    public HealthSystem GetHealthSystem() {
        return healthSystem;
    }
}
