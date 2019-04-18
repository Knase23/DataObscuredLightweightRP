using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{

    Health health;
    // Start is called before the first frame update
    void Start()
    {
        tag = "Enemy";
        health = GetComponent<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();

            if(bullet.isFromPlayer())
            {
                if (health.TakeDamage(bullet.GetDamage()))
                {
                    // Enemy dies


                    //Replace with something else;
                    gameObject.SetActive(false);

                }
                Destroy(other.gameObject);
            }
            
        }
    }
}
