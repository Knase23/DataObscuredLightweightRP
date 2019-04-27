using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAIOverlord : MonoBehaviour
{

    VirusManager virus;
    EnemySpawner enemySpawner;
    Statistics stats;

    private void Start()
    {
        virus = FindObjectOfType<VirusManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        stats = FindObjectOfType<Statistics>();
    }
    // Will check Statistics of the Player
    // Depedning on the numbers make a action based on the state

    // HighState
    // If numberOfEnemys spawned this state is over the statistical
    // And Or player have gone to lower then 10 % of HP
    // Go To LowRiskState

    // LowRiskState
    //If player have gatherd some 


    // Will check 

}
