using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerCharacter))]
public class PlayerJump : MonoBehaviour
{
    CustomValue jumpPower = new CustomValue(4.5f);
    Rigidbody body;
    PlayerCharacter player;
    float totalJumpsBeforeHittingGround = 1;
    float numberOfJumps = 0;
    float jumpTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        player = GetComponent<PlayerCharacter>();
    }

    public bool Jump()
    {
        if(numberOfJumps > totalJumpsBeforeHittingGround)
        {
            return false;
        }
        if(jumpTimer > 0)
        {
            return false;
        }
        bool onGround = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit,1))
        {

            Debug.Log(hit.collider.gameObject);
            onGround = true;
            Vector3 bodyVelocity = body.velocity;
            body.velocity += Vector3.up * jumpPower.Result();
        }

        return true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position + Vector3.up * 0.5f, Vector3.down);
    }
}
