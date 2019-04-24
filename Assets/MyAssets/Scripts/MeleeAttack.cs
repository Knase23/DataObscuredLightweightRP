using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{
    public Transform box;
    public LayerMask layerMask;

    public override bool ExecuteAttack()
    {
        Collider[] colliders = Physics.OverlapBox(box.position, box.localScale, box.rotation, layerMask);    
        if (attackSpeedTimer <= 0)
        {
            foreach (var item in colliders)
            {
                Health itemHP;
                if (itemHP = item.GetComponent<Health>())
                {
                    itemHP.TakeDamage(attackDamage.Result());
                }
            }

            TriggerOnAttack();
            attackSpeedTimer = attackSpeed.Result();
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if(AI)
        {
            if(ableToAttack)
            {
                if(currentTarget != null && Vector3.Distance(currentTarget.position,transform.position) <= 1)
                {
                    ExecuteAttack();
                }
            }
        }
        attackSpeedTimer -= Time.deltaTime;
    }
    //private void OnDrawGizmosSelected()
    //{
        
    //    if(transform)
    //    {
    //        Gizmos.DrawCube(box.position, box.localScale);
    //    }
    //    //Gizmos.DrawSphere(transform.position, maxDistanceForDetection);
    //    if (lastTarget)
    //    {
    //        Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, lastTarget.position + Vector3.up * 0.5f);
    //    }

    //}
}
