using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiShoot : MonoBehaviour
{
    public List<Transform> opponets;
    public GameObject bulletPrefab;
    public float minimumDistanceBeforeShootingAtTarget;
    public float timeBeforeShoot = 0.5f;
    public CustomValue damage = new CustomValue(10);
    public Vector3 lastSeenPosition;
    Transform lastTarget;
    public bool shooting;
    Transform currentTarget;
    float timer;

    private void Start()
    {
        if (bulletPrefab == null)
        {
            bulletPrefab = Resources.Load<GameObject>("Bullet");
        }
    }

    private void Update()
    {
        if (shooting)
        {
            NeasestTarget();
            if (timer <= 0 && currentTarget)
            {
                Bullet createdBulletObject = Instantiate(bulletPrefab).GetComponent<Bullet>();
                createdBulletObject.transform.position = transform.position + Vector3.up * 0.5f;
                createdBulletObject.SetVelocityDirection(10, currentTarget.position);
                createdBulletObject.SetOriginator(gameObject);
                createdBulletObject.SetDamage(damage);
                timer = timeBeforeShoot;
                
            }
            timer -= Time.deltaTime;
        }
    }

    public bool LineOfSightToATarget()
    {
        if(lastTarget == null)
        {
            return false;
        }
        NeasestTarget();
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up * 0.5f, lastTarget.position, out hit))
        {
            if(hit.collider.tag == "Enviroment")
            {
                return false;
            }

            if(hit.collider.tag == lastTarget.tag)
            {
                return true;
            }
            return false;
        }
        return true;
    }

    public bool NeasestTarget()
    {
        float targetDistance = float.MaxValue;
        currentTarget = null;
        foreach (var item in opponets)
        {
            float itemDistance = Vector3.Distance(transform.position, item.position);
            if (itemDistance < minimumDistanceBeforeShootingAtTarget)
            {
                if (itemDistance < targetDistance)
                {
                    targetDistance = itemDistance;
                    currentTarget = item;
                    lastSeenPosition = item.position;
                    lastTarget = item;
                }
            }
        }
        return false;
    }



}
