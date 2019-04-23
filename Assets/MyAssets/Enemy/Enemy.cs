using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    AiShoot gun;
    Movement movement;
    Health health;
    public States currentState;
    public float waitTime = 1;
    float idleTimer;
    // Start is called before the first frame update
    void Start()
    {
        tag = "Enemy";
        health = GetComponent<Health>();
        movement = GetComponent<Movement>();
        gun = GetComponent<AiShoot>();
    }
    private void Update()
    {
        switch (currentState)
        {
            case States.WalkToPoints:
                //Moves to a point determined by the Movement script
                movement.Move();
                gun.NeasestTarget();
                if (gun.LineOfSightToATarget())
                {
                    currentState = States.Combat;
                }
                break;
            case States.FoundOpponent:
                // Moves where it last saw a opponent
                // if it has line of sight after or during it will directly go to Combat State otherwise go to idle
                movement.Move(gun.lastSeenPosition);
                if (gun.LineOfSightToATarget())
                {
                    currentState = States.Combat;
                }
                break;
            case States.Combat:
                // Starts shooting corutine and keeps distance from player and drone. Maybe do some ADAD
                gun.shooting = gun.LineOfSightToATarget();
                if(!gun.shooting)
                {
                    currentState = States.FoundOpponent;
                }

                break;
            case States.Idle:
                // Standing still and does nothing and then goes to WalkToPoints state
                if(idleTimer < 0)
                {
                    currentState = States.WalkToPoints;
                    idleTimer = waitTime + Time.deltaTime;
                }
                idleTimer -= Time.deltaTime;
                break;
            default:
                break;
        }
    }

    public void SetState(States state)
    {
        currentState = state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();

            if(bullet.isFromPlayer())
            {
                Debug.Log(bullet.GetDamage());
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

    public enum States
    {
        WalkToPoints,
        FoundOpponent,
        Combat,
        Idle

    }
}
