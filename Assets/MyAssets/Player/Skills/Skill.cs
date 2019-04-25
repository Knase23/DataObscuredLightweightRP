using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public string Name = "Not Named Skill";
    public float cost = 0;
    public CustomValue value = new CustomValue(multiplier:1);
    /// <summary>
    /// Gives the desired element if it can
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public abstract CustomValue Effect();
    public override string ToString()
    {
        return Name + "\n" + cost + " Data";
    }
}

public class NullSkill : Skill
{
    public override CustomValue Effect()
    {
        throw new System.NotImplementedException();
    }
}
