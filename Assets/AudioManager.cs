using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager INSTANCE;
   
    public AudioClip ShootSFX;
    public AudioClip HitSFX;
    public AudioClip ChangeModeSFX;
    public AudioClip FollowMeSFX;
    public AudioClip MoveToSFX;
    public AudioClip KilledSFX;
    public AudioClip TakeDamageSfx;

    public AudioSource ShootSFXSource;
    int hitInterval = 0;
    int shootInterval = 0;
    private void Awake()
    {
        if (INSTANCE)
        {
            Destroy(this);
        }
        else
        {
            INSTANCE = this;
        }

    }
    private void Start()
    {
        Weapon weapon = FindObjectOfType<Weapon>();
        weapon.OnAttack += PlayShoot;

        Enemy.OnHit += Enemy_OnHit;
        Enemy.OnKilled += Enemy_OnHit;
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
        Health health = player.GetComponent<Health>();
        health.EventTakeDamage += PlayPlayerTakeDamage;
    }

    private void Enemy_OnHit(float amount)
    {
        if (hitInterval % 3 == 0)
        {
            ShootSFXSource.PlayOneShot(HitSFX);
        }
        hitInterval++;
    }
    private void Enemy_OnHit()
    {
        ShootSFXSource.PlayOneShot(KilledSFX);
    }

    public void PlayShoot()
    {
        ShootSFXSource.PlayOneShot(ShootSFX);

        shootInterval++;
    }
    public void PlayDroneChangeMode()
    {
        ShootSFXSource.PlayOneShot(ChangeModeSFX);
    }
    public void PlayFollowMe()
    {
        ShootSFXSource.PlayOneShot(FollowMeSFX);
    }
    public void PlayMoveTo()
    {
        ShootSFXSource.PlayOneShot(MoveToSFX);
    }
    public void PlayPlayerTakeDamage(float amount)
    {
        ShootSFXSource.PlayOneShot(TakeDamageSfx);
    }


}
