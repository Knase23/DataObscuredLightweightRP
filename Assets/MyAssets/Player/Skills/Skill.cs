using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public string Name = "Not Named Skill";

    /// <summary>
    /// Gives the desired element if it can
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public abstract T Effect<T>();
}
