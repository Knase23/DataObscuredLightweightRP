using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SkillBuyButton : MonoBehaviour
{
    public delegate void AquiredSkill(Skill skill);
    public static event AquiredSkill aquiredSkill;


    public Skill buyableSkill;

    private TextMeshProUGUI text;
    private void Start()
    {
        buyableSkill = buyableSkill? buyableSkill : new NullSkill();
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = buyableSkill.ToString();
    }

    public void BuySkill()
    {
        if(PlayerVirusData.instance.BuyItem(buyableSkill.cost) && !(buyableSkill is NullSkill))
        {
            aquiredSkill(buyableSkill);
        }
        else
        {
            Debug.Log("Could not Buy");
        }

    }
}
