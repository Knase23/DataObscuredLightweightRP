using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public CustomValue maxHealth = new CustomValue(10);
    float current = 10;

    //Events
    public delegate void OnTakeDamage();
    public event OnTakeDamage EventTakeDamage;

    public delegate void OnDeath();
    public event OnDeath EventDeath;

    public delegate void OnReceiveHealth();
    public event OnReceiveHealth EventReciveHealth;


    private void Start()
    {
        current = maxHealth.Result();
    }

    /// <summary>
    /// Removes health
    /// </summary>
    /// <param name="damage">Takes in a Number. Does not care if its negativ</param>
    /// <returns>Returns true if Health is below 0. Basicly Dead</returns>
    public bool TakeDamage(float damage)
    {
        damage = Mathf.Abs(damage);
        current -= damage;
        EventTakeDamage();
        if (current <= 0)
        {
            
            current = 0;
            EventTakeDamage();
            EventDeath();
            return true;
        }
        return false;
    }
    /// <summary>
    /// Gives Health
    /// </summary>
    /// <param name="health">Takes in a Number. Does not care if its negativ</param>
    /// <returns>Returns true if it has Max Health </returns>
    public bool ReceiveHealth(float health)
    {
        health = Mathf.Abs(health);
        current += health;
        
        if (current >= maxHealth.Result())
        {
            current = maxHealth.Result();
            EventReciveHealth();
            return true;
        }
        EventReciveHealth();
        return false;
    }
    /// <summary>
    /// Gives the current Health;
    /// </summary>
    /// <returns>Current Health</returns>
    public float GetCurrent()
    {
        return current;
    }
    /// <summary>
    /// Recomend to use TakeDamage(float damage) to check if it is dead;
    /// </summary>
    /// <returns>If its Dead or not</returns>
    public bool CheckIfDead()
    {
        return current <= 0;
    }
    public override string ToString()
    {
        return current + "/" + maxHealth.Result() + " HP";
    }
    public void ApplyEffect(CustomValue amount)
    {
        maxHealth += amount;
        current += amount.Result();
        EventReciveHealth();
    }
}
