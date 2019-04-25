using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public CustomValue attackDamage = new CustomValue(10);
    public CustomValue attackSpeed = new CustomValue(10);
    public bool ableToAttack;
    public bool AI;
    protected float attackSpeedTimer;

    internal Vector3 lastSeenPosition;
    protected Transform lastTarget;
    protected Transform currentTarget;
    protected Collider[] targetsInRange;

    public float maxDistanceForDetection = 10;
    public LayerMask detectionLayers;
    public LayerMask lineOfSightLayers;

    public delegate void Attacking();
    public event Attacking OnAttack;

    public abstract bool ExecuteAttack();

    public virtual bool ApplyEffects(Skill effect)
    {
        AttackSkill skill = effect as AttackSkill;
        if(skill == null)
        {
            return false;
        }
        switch (skill.applyTo)
        {
            case AttackSkill.ApplyTo.Attack:
                attackDamage += effect.Effect();
                return true;
            case AttackSkill.ApplyTo.Speed:
                attackSpeed += effect.Effect();
                return true;
            default:
                break;
        }
        return false;
    }
    protected void TriggerOnAttack()
    {
        OnAttack();
    }
    public virtual bool NeasestTarget()
    {
        float targetDistance = float.MaxValue;
        currentTarget = null;
        CheckForOpponentsInArea();
        if (targetsInRange.Length > 0)
        {
            foreach (var item in targetsInRange)
            {
                float itemDistance = Vector3.Distance(transform.position, item.transform.position);

                if (itemDistance < targetDistance)
                {
                    targetDistance = itemDistance;
                    currentTarget = item.transform;
                    lastSeenPosition = item.transform.position;
                    lastTarget = item.transform;
                }

            }
            return true;
        }
        return false;
    }
    public virtual bool LineOfSightToATarget()
    {
        NeasestTarget();
        if (currentTarget == null)
        {
            return false;
        }
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up * 0.25f, currentTarget.position + Vector3.up *0.5f , out hit, lineOfSightLayers))
        {
            if (hit.collider.tag == "Enviroment")
            {
                return false;
            }
            
            if (hit.collider.tag == currentTarget.tag)
            {
                lastSeenPosition = currentTarget.transform.position;
                return true;
            }
            return false;
        }
        lastSeenPosition = currentTarget.transform.position;
        return true;
    }
    public virtual void CheckForOpponentsInArea()
    {
        targetsInRange = Physics.OverlapSphere(transform.position, maxDistanceForDetection, detectionLayers);
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawSphere(transform.position, maxDistanceForDetection);

    //    if (lastTarget)
    //    {
    //        Gizmos.DrawLine(transform.position + Vector3.up * 0.25f, lastTarget.position + Vector3.up * 0.5f);
    //    }
    //}
}
