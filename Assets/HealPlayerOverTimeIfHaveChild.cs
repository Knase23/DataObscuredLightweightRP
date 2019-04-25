using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayerOverTimeIfHaveChild : MonoBehaviour
{
    Health playerHealth;
    float timerToTake = 1;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GetComponentInParent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount > 0)
        {
            if (timer <= 0)
            {

                playerHealth.ReceiveHealth(1);
            }
            timer -= Time.deltaTime;
        }

    }
}
