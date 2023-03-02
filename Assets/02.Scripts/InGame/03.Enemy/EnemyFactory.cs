using EnemyOwnedStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField]
    private enemyAIManager aiManager;
    private List<Enemy> enemyList;

    public void Setup()
    {
        enemyList = new List<Enemy>();
    }

    private void Update()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].Updated();
        }
    }

    public void CreateEnemy(List<EnemySpawnData> spawnData)
    {
        for (int i = 0; i < spawnData.Count; i++)
        {
            for (int j = 0; j < spawnData[i].spawnCount; j++)
            {
                Enemy enemy = ObjectPool.Instance.GetObject(spawnData[i].poolTag).GetComponent<Enemy>();

                int ranSpawnPoint = Random.Range(0, spawnData[i].spawnPoints.Length);
                SpawnVector spawnVector = spawnData[i].spawnPoints[ranSpawnPoint];
                Vector3 spawnPos = new Vector3(spawnVector.x, spawnVector.y, spawnVector.z);
                enemy.transform.position = spawnPos;

                enemyList.Add(enemy);
                enemy.aiManager = aiManager;
                enemy.Setup();
            }
        }
    }
}
