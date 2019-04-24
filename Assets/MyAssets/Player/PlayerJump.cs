using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerCharacter))]
public class PlayerJump : MonoBehaviour
{
    public CustomValue jumpPower = new CustomValue(4.5f);
    Rigidbody body;
    PlayerCharacter player;
    public float totalJumpsBeforeHittingGround = 1;
    float numberOfJumps = 0;
    float RaycastMaxDistance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        player = GetComponent<PlayerCharacter>();
    }

    public bool Jump()
    {
        if (numberOfJumps > totalJumpsBeforeHittingGround)
            return false;

        Vector3 bodyVelocity = body.velocity;
        numberOfJumps++;
        bodyVelocity.y = jumpPower.Result();
        body.velocity = bodyVelocity;        
        return true;
    }
    private void Update()
    {
        RaycastHit hit;   
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, RaycastMaxDistance))
        {
            if (hit.collider.tag == "Enviroment")
            {
                numberOfJumps = 0;
            }
        }
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position + Vector3.up * 0.5f, Vector3.down* RaycastMaxDistance);
    }
}
