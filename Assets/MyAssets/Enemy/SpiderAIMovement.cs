using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAIMovement : AiMovement
{
    SpiderMovmentPattern movmentPatternState;
    public SpiderMovmentPattern previousMoveState;

    public float timeWaiting = 2;
    float timer = 2;
    public override void OnStart()
    {
        timer = timeWaiting;
    }
    public override bool Move()
    {
        if (agent.remainingDistance >= 1)
            return false;

        if (movmentPatternState == SpiderMovmentPattern.waitForSeconds)
        {
            timer -= Time.deltaTime;
        }
        switch (movmentPatternState)
        {
            case SpiderMovmentPattern.GoToPoint:
                base.Move();
                previousMoveState = movmentPatternState;
                movmentPatternState = SpiderMovmentPattern.waitForSeconds;
                break;
            case SpiderMovmentPattern.MoveLeft:
                agent.SetDestination(transform.position + -transform.right);
                previousMoveState = movmentPatternState;
                movmentPatternState = SpiderMovmentPattern.waitForSeconds;
                break;
            case SpiderMovmentPattern.waitForSeconds:
                if (timer < 0)
                { 
                    timer = timeWaiting;
                    if(previousMoveState == SpiderMovmentPattern.GoToPoint)
                    {
                        movmentPatternState = SpiderMovmentPattern.MoveLeft;
                    }
                    if(previousMoveState == SpiderMovmentPattern.MoveLeft)
                    {
                        movmentPatternState = SpiderMovmentPattern.MoveRight;
                    }
                    if(previousMoveState == SpiderMovmentPattern.MoveRight || previousMoveState == SpiderMovmentPattern.FollowTarget)
                    {
                        movmentPatternState = SpiderMovmentPattern.GoToPoint;
                    }
                }
                else
                {
                    return false;
                }
                break;
            case SpiderMovmentPattern.MoveRight:
                agent.SetDestination(transform.position + transform.right);
                previousMoveState = movmentPatternState;
                movmentPatternState = SpiderMovmentPattern.waitForSeconds;
                break;
            case SpiderMovmentPattern.FollowTarget:
                previousMoveState = movmentPatternState;
                movmentPatternState = SpiderMovmentPattern.MoveRight;
                break;
            default:
                break;
        }
        return true;
    }
    public override bool Move(Vector3 targetPosition)
    {
        if (agent.remainingDistance >= 1 && movmentPatternState == SpiderMovmentPattern.FollowTarget)
            return false;
        agent.SetDestination(targetPosition - (transform.forward * 0.5f));
        previousMoveState = movmentPatternState;
        movmentPatternState = SpiderMovmentPattern.FollowTarget;
        return true;
    }
    public enum SpiderMovmentPattern
    {
        GoToPoint,
        MoveLeft,
        waitForSeconds,
        MoveRight,
        FollowTarget
    }
}
