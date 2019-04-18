using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public Rigidbody rigi;
    GameObject originator;
    float damage;

    private void Start()
    {
        tag = "Bullet";
        Destroy(gameObject, 5);
    }

    public bool isFromPlayer()
    {
        return originator.tag == "Player";
    }
    public void SetDamage(float value)
    {
        damage = value;
    }
    public void SetDamage(CustomValue value)
    {
        damage = value.Result();
    }
    public void SetOriginator(GameObject orogin)
    {
        originator = orogin;
    }
    public float GetDamage()
    {
        return damage;
    }
    public GameObject GetOriginator()
    {
        return originator;
    }

    public void SetVelocityDirection(Vector3 velocityDirection)
    {
        
        rigi.velocity = velocityDirection;
    }

    public void SetVelocityDirection(float speed, Vector3 target)
    {
        Vector3 directionVelocity = new Vector3();

        directionVelocity = transform.position - target;
        directionVelocity.Normalize();

        directionVelocity *= speed;
        rigi.velocity = directionVelocity;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enviroment")
        {
            Debug.Log("Hitting Enviroment");
            Destroy(gameObject);
        }
        
    }
}
