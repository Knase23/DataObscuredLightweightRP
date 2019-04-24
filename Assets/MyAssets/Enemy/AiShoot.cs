using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiShoot : Attack
{
    public List<Transform> opponets;
    public GameObject bulletPrefab;
    public float minimumDistanceBeforeShootingAtTarget;
    public CustomValue bulletSpeed = new CustomValue(10);
    
    float timer;

    private void Start()
    {
        if (bulletPrefab == null)
        {
            bulletPrefab = Resources.Load<GameObject>("Bullet");
        }
        AI = true;
    }

    private void Update()
    {
        if (AI)
        {
            if (ableToAttack)
            {
                ExecuteAttack();
            }
        }
    }
    public override bool NeasestTarget()
    {
        float targetDistance = float.MaxValue;
        currentTarget = null;
        CheckForOpponentsInArea();
        if (targetsInRange.Length > 0)
        {
            foreach (var item in targetsInRange)
            {
                float itemDistance = Vector3.Distance(transform.position, item.transform.position);
                if (itemDistance > minimumDistanceBeforeShootingAtTarget)
                {
                    if (itemDistance < targetDistance)
                    {
                        targetDistance = itemDistance;
                        currentTarget = item.transform;
                        lastSeenPosition = item.transform.position;
                        lastTarget = item.transform;
                    }
                }
            }
            return true;
        }
        return false;
    }

    public override bool ExecuteAttack()
    {
        NeasestTarget();
        if (timer <= 0 && currentTarget)
        {
            Bullet createdBulletObject = Instantiate(bulletPrefab).GetComponent<Bullet>();
            createdBulletObject.transform.position = transform.position + Vector3.up * 0.5f;
            createdBulletObject.SetVelocityDirection(bulletSpeed.Result(), currentTarget.position);
            createdBulletObject.SetOriginator(gameObject);
            createdBulletObject.SetDamage(attackDamage);
            timer = attackSpeed.Result();
        }
        timer -= Time.deltaTime;
        return true;
    }
}
