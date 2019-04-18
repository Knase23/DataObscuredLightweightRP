using UnityEngine;

[System.Serializable]
public struct CustomValue
{
    public float Base;

    /// <summary>
    /// Adds on to Base
    /// </summary>
    public float Modifier;

    /// <summary>
    /// Is in procentage so it the value should never be negative
    /// </summary>
    public float Multiplier;

    public CustomValue(float value = 0, float modifier = 0, float multiplier = 1)
    {
        Base = value;
        Modifier = modifier;
        Multiplier = Mathf.Abs(multiplier);
    }
    /// <summary>
    /// Gives: (Base + Modifier) * |Multiplier| 
    /// </summary>
    /// <returns></returns>
    public float Result()
    {
        return (Base + Modifier) * Mathf.Abs(Multiplier);
    }
    /// <summary>
    /// Takes the a.base and adds the rest
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static CustomValue operator +(CustomValue a, CustomValue b)
    {
        return new CustomValue(a.Base, a.Modifier + b.Modifier, a.Multiplier + (b.Multiplier - 1));
    }
    /// <summary>
    /// Takes the a.base and subtract the rest
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static CustomValue operator -(CustomValue a, CustomValue b)
    {
        return new CustomValue(a.Base, a.Modifier - b.Modifier, a.Multiplier - (b.Multiplier - 1));
    }

}
