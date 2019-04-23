using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : Movement
{ 
    NavMeshAgent agent;
    public List<GameObject> pointsToMoveTo = new List<GameObject>();
    private void Start()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 10, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            agent = gameObject.AddComponent<NavMeshAgent>();
            agent.radius = 0.2f;
            agent.height = 1;
            agent.speed = speedValue.Result();
        }
        else
        {
            Debug.Log("Missed NavMesh");
        }
    }

    public override bool Move()
    {
        if (agent.remainingDistance >= 1)
            return false;
        if (pointsToMoveTo.Count == 0)
            return false;

        agent.SetDestination(pointsToMoveTo[Random.Range(0, pointsToMoveTo.Count)].transform.position);
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
}
