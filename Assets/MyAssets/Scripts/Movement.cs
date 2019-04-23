using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public CustomValue speedValue = new CustomValue(10);

    public void SetSpeedValue(CustomValue value)
    {
        speedValue = value;
    }
    public CustomValue GetSpeedValue()
    {
        return speedValue;
    }
    public float GetSpeedValueResult()
    {
        return speedValue.Result();
    }
    /// <summary>
    /// General Movment if it does not need any params
    /// </summary>
    /// <returns>Gives if it succeeded</returns>
    public abstract bool Move();
    /// <summary>
    /// Movement if needed direct input
    /// </summary>
    /// <param name="x"> Raw Input for X axis</param>
    /// <param name="y"> Raw Input for Y axis</param>
    /// <returns>Gives true if it can move</returns>
    public abstract bool Move(float x, float y);
    /// <summary>
    /// Movment if it needs to move to a specified position
    /// </summary>
    /// <param name="targetPosition">Position it needs to get near or to</param>
    /// <returns> Gives true if it can move </returns>
    public abstract bool Move(Vector3 targetPosition);
}
