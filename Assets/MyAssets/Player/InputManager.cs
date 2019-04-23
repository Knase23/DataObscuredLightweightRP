using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerCharacter player;
    // Start is called before the first frame update


    public Vector3 movement;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked && !Cursor.visible)
        {
            player.LookAround(Input.GetAxis("Look X"), Input.GetAxis("Look Y"));
        }
        else
        {
            player.LookAround(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Jump();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.FollowMeCommand();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            player.Command();
        }

    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            player.Shoot();
        }

        player.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical"));
    }
}
