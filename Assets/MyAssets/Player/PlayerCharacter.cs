using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(Health))]
public class PlayerCharacter : MonoBehaviour 
{
    Health health;
    Attack normalAttack;
    FirstPersonCamera firstPersonCamera;
    Movement movement;
    PlayerJump jump;
    PlayerCommand command;
    PlayerVirusData virusData;
    SkillTree skillTree;
    DroneAi drone;

    public delegate void PlayerDied();
    public static event PlayerDied OnPlayerDeath;

    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI ResourceText;
   

    // Start is called before the first frame update
    void Start()
    {
        tag = "Player";
        health = GetComponent<Health>();
        health.EventTakeDamage += UpdateHealthText;
        health.EventReciveHealth += UpdateHealthText;
        health.EventDeath += OnDeath;

        firstPersonCamera = GetComponent<FirstPersonCamera>();
        normalAttack = GetComponentInChildren<Weapon>();
        movement = GetComponent<Movement>();
        jump = GetComponent<PlayerJump>();
        command = GetComponent<PlayerCommand>();
        skillTree = GetComponent<SkillTree>();

        drone = FindObjectOfType<DroneAi>();

        virusData = GetComponent<PlayerVirusData>();
        Debug.Log(virusData);


        PlayerVirusData.OnResourceChanged += PlayerVirusData_OnResourceChanged;

        HealthText.text = health.ToString();
        ResourceText.text = virusData.ToString();
    }

    private void PlayerVirusData_OnResourceChanged()
    {
        ResourceText.text = virusData.ToString();
    }

    public bool ApplyEffect(Skill skill)
    {
        //Weapon
        if(normalAttack.ApplyEffects(skill))
        {
            return true;
        }
        

        //Drone
        if(drone.ApplyEffect(skill))
        {
            return true;
        }

        //Character
        CharacterSkill cSkill = skill as CharacterSkill;
        if(cSkill == null)
        {
            return false;
        }
        switch (cSkill.applyTo)
        {
            case CharacterSkill.ApplyTo.Health:
                health.ApplyEffect(cSkill.value);
                return true;
            case CharacterSkill.ApplyTo.Speed:

                return true;
            default:
                break;
        }
        return false;
    }

    #region STUFF_FOR_INPUT_MANAGER
    public void NormalAttack()
    {
        normalAttack.ExecuteAttack();
    }
    public void LookAround(float x, float y)
    {
        firstPersonCamera.LookAround(x, y);
    }
    public void Move(float x,float y)
    {
        movement.Move(x, y);
    }
    public void Jump()
    {
        jump.Jump();
    }
    public void MoveDrone()
    {
        command.MoveAgent();
    }
    public void FollowMeCommand()
    {
        command.FollowMe();
    }
    public void ChangeModeCommand()
    {
        command.ChangeMode();
    }
    public void SkillTree()
    {
        if (skillTree.UpdateSkillTreeCanvas())
        {
            FirstPersonCamera.UnlockCursor();
            //LockMovement if playing with controller

        }
        else
        {
            FirstPersonCamera.LockCursor();
            //UnlockMovement if playing with controller
        }
    }
    #endregion

    //Functions assosiated with Events/Delegates
    public void UpdateHealthText()
    {
        HealthText.text = health.ToString();
        //Debug.Log("Dead",gameObject);
    }
    public void OnDeath()
    {
        //Debug.Log("Damage Taken", gameObject);
        OnPlayerDeath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();

            if (!bullet.isFromPlayer())
            {
                health.TakeDamage(bullet.GetDamage());
                Destroy(other.gameObject);
            }

        }
    }
}
