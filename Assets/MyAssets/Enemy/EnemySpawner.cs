using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Will use the children of the transform as points to spawn on")]
    public Transform pointsToRespawnOn;
    List<Vector3> spawnPositions = new List<Vector3>();

    public List<GameObject> optionsOfEnemies = new List<GameObject>();
    private float timeToTake = 1.5f;
    private float timer = 0;
    private void Start()
    {
        if (pointsToRespawnOn)
        {
            for (int i = 0; i < pointsToRespawnOn.childCount; i++)
            {
                spawnPositions.Add(pointsToRespawnOn.GetChild(i).position);
            }

        }
    }
    public void SpawnEnemy(int enemyOption = 0, int indexSpawnPosition = 0)
    {
        #region Paremeter_Fixing
        if (enemyOption > optionsOfEnemies.Count)
        {
            enemyOption = optionsOfEnemies.Count - 1;
        }
        if (enemyOption < 0)
        {
            enemyOption = 0;
        }
        if (indexSpawnPosition > spawnPositions.Count)
        {
            indexSpawnPosition = spawnPositions.Count - 1;
        }
        if (indexSpawnPosition < 0)
        {
            indexSpawnPosition = 0;
        }
        #endregion

        #region Handle_Error_Cases
        if (spawnPositions.Count == 0)
        {
            return;
        }
        if (optionsOfEnemies.Count == 0)
        {
            return;
        }
        #endregion

        Vector3 decidedPosition = spawnPositions[indexSpawnPosition];
        GameObject enemy = Instantiate(optionsOfEnemies[enemyOption], decidedPosition, Quaternion.identity, transform);
        AiMovement movment = enemy.GetComponent<AiMovement>();
        movment.transformParentForPointsToGoTo = pointsToRespawnOn;
        movment.CheckParentFotPointsToGoTo();

    }
    public List<Vector3> GetListOfAvalibleSpawnPositions()
    {
        return spawnPositions;
    }
    public List<GameObject> GetListOfAvalibleOptionsOfEnemies()
    {
        return optionsOfEnemies;
    }

    public List<int> GetListOfValideSpawnLocations(Transform player, float validDistance, bool checkLineOfSight)
    {
        bool[] validPositions = new bool[spawnPositions.Count];
        for (int i = 0; i < validPositions.Length; i++)
        {
            validPositions[i] = true;
        }
        if (validDistance > 0)
        {
            for (int i = 0; i < spawnPositions.Count; i++)
            {
                if (Vector3.Distance(spawnPositions[i], player.position) < validDistance)
                {
                    validPositions[i] = false;
                }
            }
        }

        if (checkLineOfSight)
        {
            for (int i = 0; i < spawnPositions.Count; i++)
            {
                if (validPositions[i])
                {
                    RaycastHit hit;
                    if (Physics.Linecast(spawnPositions[i] + Vector3.up * 0.25f, player.position + Vector3.up * 0.5f, out hit))
                    {
                        if (hit.collider.tag == player.tag)
                        {
                            validPositions[i] = false;
                        }
                    }
                    else
                    {
                        validPositions[i] = false;
                    }
                }
            }
        }

        List<int> theList = new List<int>();
        for (int i = 0; i < validPositions.Length; i++)
        {
            if (validPositions[i])
            {
                theList.Add(i);
            }
        }
        return theList;
    }


    public int GetNubmerOfEnemies()
    {
        return transform.childCount;
    }
}
