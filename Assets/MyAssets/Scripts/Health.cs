using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public bool invincible;
    public CustomValue maxHealth = new CustomValue(10);
    float current = 10;

    //Events
    public delegate void OnTakeDamage(float amount);
    public event OnTakeDamage EventTakeDamage;

    public delegate void OnDeath();
    public event OnDeath EventDeath;

    public delegate void OnReceiveHealth(float amount);
    public event OnReceiveHealth EventReciveHealth;
    private void Awake()
    {
        current = maxHealth.Result();
    }

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
        if(!invincible)
            current -= damage;

        EventTakeDamage(damage);
        if (current <= 0)
        {
            
            current = 0;
            EventTakeDamage(-1);
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
        EventReciveHealth(health);
        if (current >= maxHealth.Result())
        {
            current = maxHealth.Result();
            EventReciveHealth(0);
            return true;
        }
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
        EventReciveHealth(amount.Result());
    }
}
