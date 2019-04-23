using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Health))]
public class PlayerCharacter : MonoBehaviour 
{
    Health health;
    Weapon weaponMechanic;
    FirstPersonCamera firstPersonCamera;
    Movement movement;
    PlayerJump jump;
    PlayerCommand command;
    // Start is called before the first frame update
    void Start()
    {
        tag = "Player";
        health = GetComponent<Health>();
        firstPersonCamera = GetComponent<FirstPersonCamera>();
        weaponMechanic = GetComponentInChildren<Weapon>();
        movement = GetComponent<Movement>();
        jump = GetComponent<PlayerJump>();
        command = GetComponent<PlayerCommand>();
    }
    public void Shoot()
    {
        weaponMechanic.Shoot();
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
    public void Command()
    {
        command.OutsideCommand();
    }
    public void FollowMeCommand()
    {
        command.FollowMe();
    }


    public Transform GetFirstPersonCameraJoint()
    {
        return firstPersonCamera.playerCamera;
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

                    Debug.Log("You Died");
                    //Replace with something else;
                    gameObject.SetActive(false);

                }
                Destroy(other.gameObject);
            }

        }
    }
}
