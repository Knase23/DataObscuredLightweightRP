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

    private void Update()
    {
        if(timer <= 0 && transform.childCount < 10)
        {
            SpawnEnemy(indexSpawnPosition: Random.Range(0, spawnPositions.Count));
            timer = timeToTake;
        }
        timer -= Time.deltaTime;
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
        if(spawnPositions.Count == 0)
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

}
