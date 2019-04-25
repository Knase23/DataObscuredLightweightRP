using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerCharacter player;

    //Commands for buttons
    Command buttonSpace;
    Command buttonOne;
    Command buttonF;
    Command buttonLeftMouse;
    Command buttonTwo;
    Command buttonT;
    Command move;
    Command lookAround;

    // Start is called before the first frame update
    void Start()
    {
        buttonSpace = new JumpCommand();
        buttonOne = new FollowMeDroneCommand();
        buttonTwo = new DroneChangeModeCommand();
        buttonF = new MoveDroneCommand();
        buttonLeftMouse = new NormalAttackCommand();
        buttonT = new SkillTreeCommand();
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked && !Cursor.visible)
        {
            lookAround = new LookAroundCommand(Input.GetAxis("Look X"), Input.GetAxis("Look Y"));
        }
        else
        {
            lookAround = new LookAroundCommand();
        }
        lookAround.Execute(player);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttonSpace.Execute(player);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            buttonOne.Execute(player);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            buttonTwo.Execute(player);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            buttonF.Execute(player);
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            buttonT.Execute(player);
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            buttonLeftMouse.Execute(player);
        }
        move = new MoveCommand(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        move.Execute(player);
    }
}
//Command Pattern stuff, Works only for PlayerCharacters
#region Commands
class Command
{
    public virtual void Execute(PlayerCharacter actor)
    {

    }
}
class JumpCommand : Command
{
    public override void Execute(PlayerCharacter actor)
    {
        actor.Jump();
    }
}
class FollowMeDroneCommand : Command
{
    public override void Execute(PlayerCharacter actor)
    {
        actor.FollowMeCommand();
    }
}
class DroneChangeModeCommand : Command
{
    public override void Execute(PlayerCharacter actor)
    {
        actor.ChangeModeCommand();
    }
}
class MoveDroneCommand : Command
{
    public override void Execute(PlayerCharacter actor)
    {
        actor.MoveDrone();
    }
}
class NormalAttackCommand : Command
{
    public override void Execute(PlayerCharacter actor)
    {
        actor.NormalAttack();
    }
}
class MoveCommand : Command
{
    float x, y;
    public MoveCommand(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
    public override void Execute(PlayerCharacter actor)
    {
        actor.Move(x, y);
    }
}
class LookAroundCommand : Command
{
    float x, y;
    public LookAroundCommand(float x = 0, float y = 0)
    {
        this.x = x;
        this.y = y;
    }
    public override void Execute(PlayerCharacter actor)
    {
        actor.LookAround(x, y);
    }
}

class SkillTreeCommand : Command
{
    public override void Execute(PlayerCharacter actor)
    {
        actor.SkillTree();
    }
}
#endregion

