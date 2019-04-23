using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAi : Commandable, IInteractable
{

    NavMeshAgent agent;
    private void Start()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 10, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            agent = gameObject.AddComponent<NavMeshAgent>();
        }
        else
        {
            Debug.Log("Missed NavMesh");
        }
    }

    public override void Command(CommandInfo info)
    {
        currentInfo = info;

        switch (currentInfo.command)
        {
            case CommandInfo.Command.Nothing:
                StateEnd();
                currentRoutine = null;
                StateStart();
                break;
            case CommandInfo.Command.Follow:
                currentRoutine = StartCoroutine(Follow());
                break;
            case CommandInfo.Command.Move:
                currentRoutine = StartCoroutine(Move());
                break;
            case CommandInfo.Command.Attack:
                //currentRoutine = StartCoroutine(Follow());
                break;
            case CommandInfo.Command.Interact:
                //currentRoutine = StartCoroutine(Follow());
                break;
            case CommandInfo.Command.Defend:
                //currentRoutine = StartCoroutine(Follow());
                break;
            default:
                break;
        }
    }

    public IEnumerator Follow()
    {
        yield return new WaitUntil(() => previousRoutine == null);
        StateStart();

        if (agent)
        {
            while (currentInfo.command == CommandInfo.Command.Follow)
            {
                Vector3 position = currentInfo.target.position;

                if (!agent.isStopped) agent.SetDestination(position);


                yield return null;
                if (currentInfo.target?.tag == "FollowPosition")
                {
                    if (Vector3.Distance(position, transform.position) <= 1 && transform.parent != currentInfo.target)
                    {
                        transform.parent = currentInfo.target;
                        transform.localPosition = Vector3.zero;
                        agent.isStopped = true;
                    }
                    else
                    {
                        agent.isStopped = false;
                    }

                }
                yield return null;
            }
            transform.parent = null;
            agent.isStopped = false;
        }
        Debug.Log("End Follow Command");
        StateEnd();
        yield break;
    }

    public IEnumerator Move()
    {
        yield return new WaitUntil(() => previousRoutine == null);
        StateStart();

        if (agent)
        {
            while (currentInfo.command == CommandInfo.Command.Move)
            {
                
                Vector3 position = currentInfo.point;

                if (!agent.isStopped) agent.SetDestination(position);


                if(Vector3.Distance(position, transform.position) <= 1)
                {
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                }

                yield return null;
            }
        }
        agent.isStopped = false;
        Debug.Log("End Move Command");
        StateEnd();
        yield break;
    }

    public void OnInteract()
    {

    }
}
