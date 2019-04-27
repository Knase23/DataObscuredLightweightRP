using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : Movement
{ 
    protected NavMeshAgent agent;
    public Transform transformParentForPointsToGoTo;
    public List<Transform> pointsToMoveTo = new List<Transform>();
    private void Start()
    {
        if(transformParentForPointsToGoTo.childCount > 0)
        {
            for (int i = 0; i < transformParentForPointsToGoTo.childCount; i++)
            {
                pointsToMoveTo.Add(transformParentForPointsToGoTo.GetChild(i));
            }
        }
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 10, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            agent = gameObject.AddComponent<NavMeshAgent>();
            agent.radius = 0.2f;
            agent.height = height;
            agent.speed = speedValue.Result();
        }
        else
        {
            Debug.Log("Missed NavMesh");
        }
        OnStart();
    }

    public override bool Move()
    {        
        if (pointsToMoveTo.Count > 0)
        {
            agent.SetDestination(pointsToMoveTo[Random.Range(0, pointsToMoveTo.Count)].position);
        }
        else
        {
            return false;
        }
        return true;
    }

    public override bool Move(float x, float y)
    {
        return false;
    }

    public override bool Move(Vector3 targetPosition)
    {
        if (agent.remainingDistance >= 1)
            return false;
        agent.SetDestination(targetPosition);
        return true;
    }
    public virtual void OnStart()
    {

    }
    public void CheckParentFotPointsToGoTo()
    {
        if (transformParentForPointsToGoTo == null)
            return;

        for (int i = 0; i < transformParentForPointsToGoTo.childCount; i++)
        {
            pointsToMoveTo.Add(transformParentForPointsToGoTo.GetChild(i));
        }
    }

    public override bool IsMoving()
    {
        return !agent.isStopped;
    }
}
