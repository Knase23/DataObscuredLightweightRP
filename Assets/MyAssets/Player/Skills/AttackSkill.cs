using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSkill", menuName = "Skills/AttackSkill")]
public class AttackSkill : Skill
{

    public ApplyTo applyTo;
    public override CustomValue Effect()
    {
        return value;
    }

    public enum ApplyTo
    {
        Attack,
        Speed
    }

}
