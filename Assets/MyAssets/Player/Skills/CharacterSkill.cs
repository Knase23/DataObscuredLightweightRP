using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSkill", menuName = "Skills/CharacterSkill")]
public class CharacterSkill : Skill
{
    public ApplyTo applyTo;
    public override CustomValue Effect()
    {
        return value;
    }

    public enum ApplyTo
    {
        Health,
        Speed
    }
}