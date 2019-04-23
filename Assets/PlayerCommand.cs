using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommand : MonoBehaviour
{
    public float distance;
    public Commandable agent;

    public Transform forward;

    public Transform followPosition;

    public void OutsideCommand()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(forward.position,forward.forward,out raycastHit,distance))
        {
            if(raycastHit.collider.gameObject == agent.gameObject)
            {
                FollowMe();
                return;
            }

            //IInteractable interactable = raycastHit.collider.GetComponent<IInteractable>();
            //if(interactable != null)
            //{
            //    agent.Command(new Commandable.CommandInfo(Commandable.CommandInfo.Command.Follow, raycastHit.point, raycastHit.collider.transform));
            // return;
            //}

            //Enemy enemy = raycastHit.collider.GetComponent<Enemy>();
            //if (enemy)
            //{
            //    agent.Command(new Commandable.CommandInfo(Commandable.CommandInfo.Command.Attack, raycastHit.point, raycastHit.collider.transform));
            // return;
            //}

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
