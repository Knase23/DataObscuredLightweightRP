using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSkill", menuName = "Skills/WeaponSkill")]
public class WeaponSkill : Skill
{

    public ApplyTo applyTo;
    public override CustomValue Effect()
    {
        return value;
    }

    public enum ApplyTo
    {
        BulletSpeed
    }

}
