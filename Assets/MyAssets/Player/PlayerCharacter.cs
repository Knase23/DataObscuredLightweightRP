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

    public Slider HealthSlider;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI ResourceText;

    public Image flashImage;

    // Start is called before the first frame update
    void Start()
    {
        tag = "Player";
        health = GetComponent<Health>();
        health.EventTakeDamage += OnTakeDamage;
        health.EventReciveHealth += OnReciveHealth;
        health.EventDeath += OnDeath;

        firstPersonCamera = GetComponent<FirstPersonCamera>();
        normalAttack = GetComponentInChildren<Weapon>();
        movement = GetComponent<Movement>();
        jump = GetComponent<PlayerJump>();
        command = GetComponent<PlayerCommand>();
        skillTree = GetComponent<SkillTree>();

        drone = FindObjectOfType<DroneAi>();

        virusData = GetComponent<PlayerVirusData>();

        PlayerVirusData.OnResourceChanged += PlayerVirusData_OnResourceChanged;

        UpdateHealthText();
        ResourceText.text = virusData.ToString();
        flashBaseColor = flashImage.color;
    }

    private void PlayerVirusData_OnResourceChanged()
    {
        if (ResourceText)
        {
            ResourceText.text = virusData.ToString();
            iTween.PunchScale(ResourceText.gameObject, Vector3.up, 0.5f);
        }
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
                movement.ApplyEffect(cSkill.value);
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

    public void OnReciveHealth(float amount)
    {
        UpdateHealthText();
        iTween.PunchScale(HealthSlider.gameObject, Vector3.right * 0.01f, 0.5f);
    }
    public void OnTakeDamage(float amount)
    {
        UpdateHealthText();
        iTween.PunchScale(HealthSlider.gameObject, Vector3.up, 0.5f);
        //Flash the sceen for a milisecond or two. 
        FlashScreen();
    }

    private Color flashBaseColor;
    private Coroutine coroutine;
    public float flashDuration;
    public void FlashScreen()
    {

            coroutine = StartCoroutine(Flash());

    }
    public IEnumerator Flash()
    {
        float maxAlpha = 0.1f;
        float converstion = maxAlpha * 2 / flashDuration;
        // Takes about 1 second
        Color currentColor = flashImage.color;
        float alpha = currentColor.a;
        while (alpha < maxAlpha)
        {
            alpha += converstion * Time.deltaTime;
            currentColor.a = alpha;
            flashImage.color = currentColor;
            yield return null;
        }

        while (alpha > 0)
        {
            alpha -= converstion *  Time.deltaTime;
            currentColor.a = alpha;
            flashImage.color = currentColor;
            yield return null;
        }
        coroutine = null;
    }
    //Functions assosiated with Events/Delegates
    public void UpdateHealthText()
    {
        HealthSlider.value = health.GetCurrent();
        HealthSlider.maxValue = health.maxHealth.Result();
        HealthText.text = health.ToString();
        
        //Debug.Log("Dead",gameObject);
    }
    public void OnDeath()
    {
        //Debug.Log("Damage Taken", gameObject);
            OnPlayerDeath();
    }
    public void Update()
    {
        if(transform.position.y <= -3 || transform.position.x >= 25 || transform.position.x <= - 14 || transform.position.z <= -24 || transform.position.z >= 24)
        {
            transform.position = Vector3.zero;
        }
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
