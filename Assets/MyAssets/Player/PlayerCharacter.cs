using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Health))]
public class PlayerCharacter : MonoBehaviour 
{
    Health health;
    Weapon weaponMechanic;
    FirstPersonCamera firstPersonCamera;
    // Start is called before the first frame update
    void Start()
    {
        tag = "Player";
        health = GetComponent<Health>();
        firstPersonCamera = GetComponent<FirstPersonCamera>();
        weaponMechanic = GetComponentInChildren<Weapon>();
    }
    public void Shoot()
    {
        weaponMechanic.Shoot();
    }
    public void LookAround(float x, float y)
    {
        firstPersonCamera.LookAround(x, y);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();

            if (!bullet.isFromPlayer())
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
