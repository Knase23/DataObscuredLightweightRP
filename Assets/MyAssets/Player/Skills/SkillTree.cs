using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public GameObject skillTreeCanvas;

    private List<Skill> boughtSkills = new List<Skill>();
    private List<bool> skilledApply = new List<bool>();
    PlayerCharacter player;

    public delegate void BoughtSkill();
    public static event BoughtSkill OnBoughtSkill;

    private void Start()
    {
        player = GetComponent<PlayerCharacter>();
        SkillBuyButton.aquiredSkill += AddSkill;
    }

    void AddSkill(Skill skill)
    {
        boughtSkills.Add(skill);
        skilledApply.Add(false);
        OnBoughtSkill();
        ApplySkills();
    }
    void RemoveSkill(int index)
    {
        boughtSkills.RemoveAt(index);
        skilledApply.RemoveAt(index);
    }
    public bool UpdateSkillTreeCanvas()
    {
        skillTreeCanvas.SetActive(!skillTreeCanvas.activeInHierarchy);
        return skillTreeCanvas.activeInHierarchy;
    }
    void ApplySkills()
    {
        for (int i = 0; i < boughtSkills.Count; i++)
        {
            if(!skilledApply[i])
            {
                skilledApply[i] = player.ApplyEffect(boughtSkills[i]);
            }
        }
    }


}
