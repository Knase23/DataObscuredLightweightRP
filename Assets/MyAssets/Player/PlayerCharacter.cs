using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(Health))]
public class PlayerCharacter : MonoBehaviour 
{
    Health health;
    Attack normalAttack;
    FirstPersonCamera firstPersonCamera;
    Movement movement;
    PlayerJump jump;
    PlayerCommand command;


    public delegate void PlayerDied();
    public static event PlayerDied OnPlayerDeath;

    public TextMeshProUGUI HealthText;


    // Start is called before the first frame update
    void Start()
    {
        tag = "Player";
        health = GetComponent<Health>();
        health.EventTakeDamage += OnDamageTaken;
        health.EventDeath += OnDeath;

        firstPersonCamera = GetComponent<FirstPersonCamera>();
        normalAttack = GetComponentInChildren<Weapon>();
        movement = GetComponent<Movement>();
        jump = GetComponent<PlayerJump>();
        command = GetComponent<PlayerCommand>();

        HealthText.text = health.ToString();
    }

    #region STUFF_FOR_INPUT_MANAGER
    public void NormalAttack()
    {
        normalAttack.ExecuteAttack();
    }
    public void LookAround(float x, float y)
    {
        firstPersonCamera.LookAround(x, y);
    }
    public void Move(float x,float y)
    {
        movement.Move(x, y);
    }
    public void Jump()
    {
        jump.Jump();
    }
    public void MoveDrone()
    {
        command.MoveAgent();
    }
    public void FollowMeCommand()
    {
        command.FollowMe();
    }
    #endregion

    //Functions assosiated with Events/Delegates
    public void OnDamageTaken()
    {
        HealthText.text = health.ToString();
        //Debug.Log("Dead",gameObject);
    }
    public void OnDeath()
    {
        //Debug.Log("Damage Taken", gameObject);
        OnPlayerDeath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();

            if (!bullet.isFromPlayer())
            {
                health.TakeDamage(bullet.GetDamage());
                Destroy(other.gameObject);
            }

        }
    }
}
