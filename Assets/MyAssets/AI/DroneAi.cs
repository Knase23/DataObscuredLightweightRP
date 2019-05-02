using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class DroneAi : Commandable
{

    NavMeshAgent agent;


    public Image ModeIcon;
    public Sprite HarvestModeSprite;
    public Sprite FollowModeSprite;
    public Sprite MoveToPointSprite;

    [Header("Harvest mask")]
    public LayerMask mask;
    public float detectionRadius;


    public CustomValue InteractSpeed = new CustomValue(5);
    public float interactTimer;
    private bool interacting;
    public LayerMask interactLayers;
    private Vector3 interactDirection = new Vector3(0, 0, 1);
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
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }
        else
        {
            Debug.Log("Missed NavMesh");
        }
        Command(new CommandInfo(CommandInfo.Command.Follow, target: GameObject.Find("FollowPosition").transform));

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
                agent.speed += skill.value.Result();
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


    }
    private void Update()
    {
        switch (currentInfo.command)
        {
            case CommandInfo.Command.Nothing:
                break;
            case CommandInfo.Command.Follow:
                interacting = false;
                Follow();
                break;
            case CommandInfo.Command.Move:
                interacting = false;
                Move();
                break;
            case CommandInfo.Command.Attack:
                //currentRoutine = StartCoroutine(Follow());
                break;
            case CommandInfo.Command.Harvest:
                Harvest();
                break;
            case CommandInfo.Command.Defend:
                //currentRoutine = StartCoroutine(Follow());
                break;
            default:
                break;
        }
    }
    public void Follow()
    {
        if (agent)
        {
            ModeIcon.sprite = FollowModeSprite;
            ModeIcon.color = Color.blue;
            Vector3 position = currentInfo.target.position;

            if (!agent.isStopped) agent.SetDestination(position);

            if (currentInfo.target?.tag == "FollowPosition")
            {
                if (Vector3.Distance(position, transform.position) <= 1)
                {
                    transform.parent = currentInfo.target;
                    agent.isStopped = true;
                    ModeIcon.color = Color.green;
                }
                else
                {
                    transform.parent = null;
                    agent.isStopped = false;
                    ModeIcon.color = Color.blue;
                }
            }
            transform.parent = null;
            agent.isStopped = false;
        }
    }

    public void Move()
    {
        if (agent)
        {
            ModeIcon.sprite = MoveToPointSprite;
            Vector3 position = currentInfo.point;

            if (!agent.isStopped) agent.SetDestination(position);


            if (Vector3.Distance(position, transform.position) <= 1)
            {
                ModeIcon.color = Color.green;
                agent.isStopped = true;
            }
            else
            {
                ModeIcon.color = Color.blue;
                agent.isStopped = false;
            }

        }
        agent.isStopped = false;
    }

    public void Harvest()
    {
        transform.parent = null;
        Vector3 positionBeforeHarvest = transform.position;
        VirusNode currentTarget = FindNearestVirusNode();
        if (agent)
        {
            ModeIcon.sprite = HarvestModeSprite;
            if (currentTarget && !currentTarget.harvested)
            {
                ModeIcon.color = Color.blue;
                Vector3 position = currentTarget.transform.position;
                agent.SetDestination(position);

                if (agent.remainingDistance <= 1)
                {

                    if (interacting)
                    {
                        ModeIcon.color = Color.blue + Color.red;
                        if (interactTimer <= 0)
                        {
                            interactOrign = transform.position + (Vector3.up * 0.61f);
                            interactDirection = currentTarget.transform.position - interactOrign;

                            Ray ray = new Ray(interactOrign, interactDirection);
                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit, 10, interactLayers))
                            {
                                if (hit.collider.gameObject == currentTarget.gameObject)
                                {
                                    currentTarget.OnInteract();
                                    interacting = false;
                                }
                            }


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
                ModeIcon.color = Color.red;
                currentTarget = FindNearestVirusNode();
                agent.SetDestination(currentInfo.target.position);
            }
        }
        agent.isStopped = false;
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
