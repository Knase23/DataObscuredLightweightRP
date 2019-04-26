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


    public CustomValue InteractSpeed = new CustomValue(5);
    private float interactTimer;
    private bool interacting;
    public LayerMask interactLayers;
    private Vector3 interactDirection = new Vector3(0,0,1);
    private Vector3 interactOrign;
    private void Start()
    {
        interactOrign = transform.position + Vector3.up;
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
        Command(new CommandInfo(CommandInfo.Command.Follow,target:transform.parent));

    }

    public bool ApplyEffect(Skill effect)
    {
        DroneSkill skill = effect as DroneSkill;
        if (skill == null)
            return false;

        switch (skill.applyTo)
        {
            case DroneSkill.ApplyTo.InteractSpeed:
                InteractSpeed += skill.value;
                return true;
            case DroneSkill.ApplyTo.MovementSpeed:
                return true;
            case DroneSkill.ApplyTo.HarvestAmountBoost:
                return true;
            default:
                break;
        }

        return false;
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
        
        StateStart();

        if (agent)
        {
            while (currentInfo.command == CommandInfo.Command.Follow)
            {
                StateStart();
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
        StateStart();
        transform.parent = null;
        if (agent)
        {
            while (currentInfo.command == CommandInfo.Command.Move)
            {
                StateStart();
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
        StateStart();
        transform.parent = null;
        Vector3 positionBeforeHarvest = transform.position;
        VirusNode currentTarget = FindNearestVirusNode();
        if (agent)
        {
            while (currentInfo.command == CommandInfo.Command.Harvest)
            {
                StateStart();
                if (currentTarget)
                {
                    if (currentTarget.harvested)
                    {
                        currentTarget = FindNearestVirusNode();
                    }
                }

                if (currentTarget)
                {
                    Vector3 position = currentTarget.transform.position;
                    agent.SetDestination(position);

                    if (agent.remainingDistance <= 1)
                    {
                        if (interacting)
                        {
                            if (interactTimer <= 0)
                            {
                                interactOrign = transform.position + (Vector3.up * 0.61f);
                                interactDirection = currentTarget.transform.position - interactOrign;

                                Ray ray = new Ray(interactOrign, interactDirection);
                                RaycastHit hit;
                                if (Physics.Raycast(ray, out hit, 10, interactLayers))
                                {
                                    Debug.Log("Drone Interacting");
                                    if (hit.collider.gameObject == currentTarget.gameObject)
                                    {
                                        currentTarget.OnInteract();
                                    }
                                }
                                interacting = false;

                            }
                            interactTimer -= Time.deltaTime;
                        }
                        else
                        {
                            interacting = true;
                            interactTimer = InteractSpeed.Result();
                        }
                    }
                }
                else
                {
                    currentTarget = FindNearestVirusNode();
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
            if (!node.harvested)
            {
                if (closest)
                {
                    float distance = Vector3.Distance(transform.position, node.transform.position);
                    if (distance < closestDistance)
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
        }

        return closest;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(interactOrign, interactDirection);
    }

}
