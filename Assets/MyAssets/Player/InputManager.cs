using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerCharacter player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            player.Shoot();
        }
        player.LookAround(Input.GetAxis("Look X"), Input.GetAxis("Look Y"));
    }
}
