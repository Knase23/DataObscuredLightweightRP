using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusManager : MonoBehaviour
{

    public List<VirusNode> virusNodes = new List<VirusNode>();

    private float timeToTake = 2;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        // Get all Virus Nodes and Disable them;
        foreach (var item in GetComponentsInChildren<VirusNode>())
        {
            virusNodes.Add(item);
            item.SetHarvested(true);
        }
    }
    // Function that activates one virus node
    private void ActivateRandomNode()
    {
        int index = Random.Range(0, virusNodes.Count);
        virusNodes[index].SetHarvested(false);
        //Debug.Log("A node is ready to be harvested");
    }

    public void ActivateANode()
    {
        ActivateRandomNode();
    }

    //Function that returns the number of current activated
    public int GetNumberOfActiveViruses()
    {
        int counter = 0;
        foreach (var item in virusNodes)
        {
            if(!item.harvested)
            {
                counter++;
            }
        }
        return counter;
    }
}
