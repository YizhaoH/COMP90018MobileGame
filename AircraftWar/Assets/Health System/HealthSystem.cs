using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public event EventHandler OnHealthChanged;
    public EndGame endGameMenu;
    public ScoreManager scoreManager;
    void Start()
    {
        
        scoreManager = GameObject.FindWithTag("Score").GetComponent<ScoreManager>();
    }
    public void getDamage(int dmg)
    {
        health -= dmg;
        if (health<0) health = 0;
        if (health <= 0)
        {
            //Debug.Log(this.transform.tag);
            checkGameOver();
        }
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
    public void checkGameOver()
    {
        if (this.transform.parent.tag == "Player")
        {
            //Debug.Log("over");
            endGameMenu.Setup(scoreManager.score);
        }
    }
    public float getHealthPercent()
    {
        return (float)health / maxHealth;
    }

    public float getHealth()
    {
        return (float)health;
    }

}
