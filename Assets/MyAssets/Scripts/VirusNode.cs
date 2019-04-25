using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusNode : MonoBehaviour, IInteractable
{
    public float amountOfData = 10;
    public bool harvested = false;
    public Material harvestedMaterial;
    Material NotHarvestedMaterial;
    MeshRenderer meshRenderer;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        NotHarvestedMaterial = meshRenderer.material;
    }
    public void OnInteract()
    {
        if(harvested)
        {
            meshRenderer.material = NotHarvestedMaterial;
        }
        else
        {
            Debug.Log("Harvested",gameObject);
            harvested = true;
            PlayerVirusData.instance.AddResource(amountOfData);
            meshRenderer.material = harvestedMaterial;
        }
    }
    public void SetHarvested(bool state)
    {
        harvested = state;
        if (harvested)
        {
            meshRenderer.material = harvestedMaterial;
        }
        else
        {
            meshRenderer.material = NotHarvestedMaterial;
        }
    }
}
