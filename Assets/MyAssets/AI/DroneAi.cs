using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAi : Commandable
{

    NavMeshAgent agent;

    [Header("Harvest mask")]
    public LayerMask mask;
    public float detectionRadius;


    private void Start()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 10, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            agent = gameObject.AddComponent<NavMeshAgent>();
            agent.radius = 0.2f;
            agent.height = 1;
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
            case CommandInfo.Command.Harvest:
                currentRoutine = StartCoroutine(Harvest());
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
                    if (Vector3.Distance(position, transform.position) <= 1)
                    {
                        transform.parent = currentInfo.target;
                        transform.localPosition = Vector3.zero;
                        agent.isStopped = true;
                    }
                    else
                    {
                        transform.parent = null;
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

    public IEnumerator Harvest()
    {
        yield return new WaitUntil(() => previousRoutine == null);
        StateStart();
        Vector3 positionBeforeHarvest = transform.position;
        VirusNode currentTarget = FindNearestVirusNode();
        if (agent)
        {
            while (currentInfo.command == CommandInfo.Command.Harvest)
            {
                if(currentTarget)
                {
                    currentTarget = FindNearestVirusNode();
                }
                
                Vector3 position = currentTarget.transform.position;
                if (!agent.isStopped) agent.SetDestination(position);


                if (Vector3.Distance(position, transform.position) <= 1)
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
        Debug.Log("End Harvest Mode");
        StateEnd();
        yield break;
    }

    VirusNode FindNearestVirusNode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, mask);

        VirusNode closest = null;
        float closestDistance = float.MaxValue;

        foreach (var item in colliders)
        {
            VirusNode node = item.GetComponent<VirusNode>();
            if(closest)
            {
                float distance = Vector3.Distance(transform.position, node.transform.position);
                if(distance < closestDistance)
                {
                    closest = node;
                    closestDistance = distance;
                }
            }
            else
            {
                closestDistance = Vector3.Distance(transform.position, node.transform.position);
                closest = node;
            }
        }

        return closest;
    }

}
