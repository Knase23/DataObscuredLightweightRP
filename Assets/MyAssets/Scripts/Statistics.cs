using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    public static Statistics instance;


    [Header("Stats to track")]
    [ReadOnly] [SerializeField] private float Kills = 0;
    [ReadOnly] [SerializeField] private float BoughtUpgrades = 0;
    [ReadOnly] [SerializeField] private float numberOfTimesPlayerBeenHurt = 0;
    [ReadOnly] [SerializeField] private float PlayerDamageDone = 0;
    [ReadOnly] [SerializeField] private float PlayerDamageTaken = 0;
    [ReadOnly] [SerializeField] private float Healing = 0;
    [ReadOnly] [SerializeField] private float TotalOfDataGatherd = 0;
    [ReadOnly] [SerializeField] private float PlayerDeathTime = 0;
    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Subscribe to all events
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
        Health playerHealth = player.GetComponent<Health>();

        playerHealth.EventDeath += OnPlayerDeath;
        playerHealth.EventReciveHealth += OnPlayerReciveHealth;
        playerHealth.EventTakeDamage += OnPlayerTakeDamage;

        Enemy.OnKilled += OnKill;
        Enemy.OnHit += OnPlayerDealtDamage;

        PlayerVirusData.OnGatherData += OnPlayerGettingData;

        SkillTree.OnBoughtSkill += OnBoughtUpgrade;
    }
    //All Functions for each event
    void OnPlayerDeath()
    {
        PlayerDeathTime = Time.timeSinceLevelLoad;
    }
    void OnPlayerReciveHealth(float amount)
    {
        Healing += amount;
    }
    void OnPlayerTakeDamage(float amount)
    {
        if (amount > 0)
        {
            numberOfTimesPlayerBeenHurt++;
            PlayerDamageTaken += amount;
        }
    }
    void OnKill()
    {
        Kills++;
    }
    void OnPlayerDealtDamage(float amount)
    {
        PlayerDamageDone += amount;
    }
    void OnPlayerGettingData(float amount)
    {
        TotalOfDataGatherd += amount;
    }
    void OnBoughtUpgrade()
    {
        BoughtUpgrades++;
    }
    
    
    
    


}
