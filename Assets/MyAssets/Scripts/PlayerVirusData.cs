using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVirusData : MonoBehaviour
{
    public static PlayerVirusData instance;

    public delegate void DataChanged();
    public static event DataChanged OnResourceChanged;

    public delegate void GivenData(float amount);
    public static event GivenData OnGatherData;

    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    float resources = 0;

    public float GetResources()
    {
        return resources;
    }

    public void AddResource(float amount)
    {
        resources += amount;
        OnGatherData(amount);
        OnResourceChanged();
    }

    public bool BuyItem(float cost)
    {
        if(resources - cost < 0)
        {
            return false;
        }
        resources -= cost;
        OnResourceChanged();
        return true;
    }
    public override string ToString()
    {
        return resources + " Data";
    }
}
