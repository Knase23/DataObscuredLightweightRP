using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    Attack attack;
    Movement movement;
    Health health;
    Renderer renderer;
    Color baseColor;
    protected Animator animator;
    public States currentState;
    public float waitTime = 1;
    float idleTimer;


    public delegate void Killed();
    public static event Killed OnKilled;
    public delegate void DamageTaken(float amount);
    public static event DamageTaken OnHit;


    // Start is called before the first frame update
    void Start()
    {
        tag = "Enemy";
        health = GetComponent<Health>();
        health.EventTakeDamage += OnDamageTaken;
        health.EventDeath += OnDeath;
        renderer = GetComponentInChildren<Renderer>();
        baseColor = renderer.material.GetColor("_EmissionColor");


        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        attack.OnAttack += TriggerAttack;
    }
    private void Update()
    {
        switch (currentState)
        {
            case States.WalkToPoints:
                //Moves to a point determined by the Movement script
                movement.Move();
                attack.NeasestTarget();
                if (attack.LineOfSightToATarget())
                {
                    currentState = States.Combat;
                }
                break;
            case States.FoundOpponent:
                // Moves where it last saw a opponent
                // if it has line of sight after or during it will directly go to Combat State otherwise go to idle
                movement.Move(attack.lastSeenPosition);
                if (attack.LineOfSightToATarget())
                {
                    currentState = States.Combat;
                }
                if(!attack.NeasestTarget())
                {
                    currentState = States.WalkToPoints;
                }
                break;
            case States.Combat:
                // Starts shooting corutine and keeps distance from player and drone. Maybe do some ADAD
                movement.Move(attack.lastSeenPosition);
                if (!attack.LineOfSightToATarget())
                {
                    currentState = States.FoundOpponent;
                }
                break;
            case States.Idle:
                // Standing still and does nothing and then goes to WalkToPoints state
                if (idleTimer < 0)
                {
                    currentState = States.WalkToPoints;
                    idleTimer = waitTime + Time.deltaTime;
                }
                idleTimer -= Time.deltaTime;
                break;
            default:
                break;
        }
        SetWalking(movement.IsMoving());
    }
    //Functions assosiated with Events/Delegates
    public void OnDamageTaken(float amount)
    {
        animator.SetTrigger("TakeDamage");
        StartCoroutine(BlinkEmissionColor());
    }
    IEnumerator BlinkEmissionColor()
    {
        Material mat = renderer.material;
        mat.SetColor("_EmissionColor", Color.grey);
        yield return new WaitForSeconds(0.1f);
        mat.SetColor("_EmissionColor", baseColor);

    }
    public void OnDeath()
    {
        animator.SetBool("Dead", true);
        OnKilled();
        //Debug.Log("Dead", gameObject);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        PlayerVirusData.instance.AddResource(1);
    }

    public void SetWalking(bool state)
    {
        animator.SetBool("Walking", state);
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }
    public void SetState(States state)
    {
        currentState = state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();

            if (bullet.isFromPlayer())
            {
                health.TakeDamage(bullet.GetDamage());
                OnHit(bullet.GetDamage());
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
