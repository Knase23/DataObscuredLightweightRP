using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Carried by the Human Character and can be affected by skills
/// </summary>
public class Weapon : Attack
{
    public Transform shootOrigin;
    public Camera playerCamera;
    public GameObject bulletPrefab;
    public CustomValue bulletSpeed = new CustomValue(10);
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
    /// Applies Effect on that is not accounted for
    /// </summary>
    /// <returns> If it suceeded </returns>
    public override bool ApplyEffects(Skill effect)
    {
        if(base.ApplyEffects(effect))
        {
            return true;
        }

        WeaponSkill skill = effect as WeaponSkill;
        if (skill == null)
        {
            return false;
        }


        switch (skill.applyTo)
        {
            case WeaponSkill.ApplyTo.BulletSpeed:
                bulletSpeed += effect.Effect();
            return true;
            default:
                break;
        }
        return false;
    }

    public override bool ExecuteAttack()
    {
        if (!shootOrigin)
        {
            return false;
        }
        if (!bulletPrefab)
        {
            return false;
        }
        if (shootTimer > 0)
        {
            return false;
        }
        
        iTween.PunchPosition(gameObject, Vector3.forward *  0.01f * attackDamage.Result(), attackSpeed.Result());
        Bullet createdBulletObject = Instantiate(bulletPrefab).GetComponent<Bullet>();
        createdBulletObject.transform.position = shootOrigin.transform.position;
        createdBulletObject.SetVelocityDirection(bulletSpeed.Result() * playerCamera.transform.forward);
        createdBulletObject.SetOriginator(gameObject);
        createdBulletObject.SetDamage(attackDamage);
        shootTimer = attackSpeed.Result();
        return true;
    }
}


