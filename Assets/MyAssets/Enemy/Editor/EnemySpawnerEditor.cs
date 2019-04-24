using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EnemySpawner enemySpawner = target as EnemySpawner;

        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("Spawn Enemy"))
            {
                enemySpawner.SpawnEnemy(indexSpawnPosition:Random.Range(0,enemySpawner.pointsToRespawnOn.childCount));
            }
        }
    }
}
