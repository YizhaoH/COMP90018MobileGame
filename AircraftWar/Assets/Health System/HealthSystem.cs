using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public event EventHandler OnHealthChanged;


    public void getDamage(int dmg)
    {
        health -= dmg;
        if (health<0) health = 0;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
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
