using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Carried by the Human Character and can be affected by skills
/// </summary>
public class Weapon : MonoBehaviour
{

    public Transform shootOrigin;
    public Camera camera;
    public GameObject bulletPrefab;
    public CustomValue damage = new CustomValue(10);
    public CustomValue speed = new CustomValue(10);
    float shootTimer = 0;
    
    private void Start()
    {
        shootOrigin = GetComponentInChildren<Gun>().projectileSpawnPosition;
    }
    private void Update()
    {
        shootTimer -= Time.deltaTime;
    }
    /// <summary>
    /// Shoots forward determined by the data it has, fails if timer is not ready
    /// </summary>
    /// <returns></returns>
    public bool Shoot()
    {
        if(!shootOrigin)
        {
            return false;
        }
        if(!bulletPrefab)
        {
            return false;
        }
        if(shootTimer > 0)
        {
            return false;
        }

        Bullet createdBulletObject = Instantiate(bulletPrefab).GetComponent<Bullet>();
        createdBulletObject.transform.position = shootOrigin.transform.position;
        createdBulletObject.SetVelocityDirection(10 * camera.transform.forward);
        createdBulletObject.SetOriginator(gameObject);
        createdBulletObject.SetDamage(damage);


        shootTimer = speed.Result();
        return true;
    }
    /// <summary>
    /// Applies Effect on that is not accounted for
    /// </summary>
    /// <returns> If it suceeded </returns>
    public bool ApplyEffects(Skill effect)
    {
        switch (effect.Name)
        {
            case "Damage":
                damage += effect.Effect<CustomValue>();
            return true;
            case "Speed":
                speed += effect.Effect<CustomValue>();
                return true;
            default:
                break;
        }

        return false;
    }
  
}


