using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerCharacter))]
public class PlayerMovement : Movement
{

    Rigidbody body;
    PlayerCharacter player;
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        player = GetComponent<PlayerCharacter>();
    }

    public override bool Move()
    {
        throw new System.NotImplementedException();
    }

    public override bool Move(float x, float y)
    {
        if (body == null)
        {
            return false;
        }
        if (player == null)
        {
            return false;
        }

        Vector3 direction = new Vector3(x, 0, y);
        float yVelocity = body.velocity.y;

        direction = transform.forward * direction.z + transform.right * direction.x;
        if (direction.magnitude > 1) { direction.Normalize(); }
        direction *= GetSpeedValueResult();
        body.velocity = direction + (Vector3.up * yVelocity);
        body.angularVelocity = Vector3.zero;
       
        return true;
    }

    public override bool Move(Vector3 targetPosition)
    {
        throw new System.NotImplementedException();
    }

    public override bool IsMoving()
    {
        
        return body.velocity.sqrMagnitude != 0;
    }
}
