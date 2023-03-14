/*
작성자: 최재호(cjh0798@gmail.com)
기능: Enemy 생성 및 Enemy Setup
 */
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField]
    private EnemyAIManager aiManager;

    // Enemy 생성
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
                enemy.aiManager = aiManager;
                enemy.Setup();
            }
        }
    }
}
