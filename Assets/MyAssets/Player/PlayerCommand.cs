using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommand : MonoBehaviour
{
    public float distance;
    public Commandable agent;

    public Transform forward;

    public Transform followPosition;

    public void ChangeMode()
    {
        agent.Command(new Commandable.CommandInfo(Commandable.CommandInfo.Command.Harvest));
    }
    public void MoveAgent()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(forward.position,forward.forward,out raycastHit,distance))
        {
            agent.Command(new Commandable.CommandInfo(Commandable.CommandInfo.Command.Move,raycastHit.point));
        }
    }
    public void FollowMe()
    {
        agent.Command(new Commandable.CommandInfo(Commandable.CommandInfo.Command.Follow, target: followPosition));
    }  
}

public interface IInteractable
{
    void OnInteract();

}
