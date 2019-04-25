﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DroneSkill", menuName = "Skills/DroneSkill")]
public class DroneSkill : Skill
{
    public ApplyTo applyTo;
    public override CustomValue Effect()
    {
        return value;
    }

    public enum ApplyTo
    {
        InteractSpeed,
        MovementSpeed,
        HarvestAmountBoost

    }

}
