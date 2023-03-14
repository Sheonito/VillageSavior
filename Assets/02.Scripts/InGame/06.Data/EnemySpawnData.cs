/*
작성자: 최재호(cjh0798@gmail.com)
기능: Enemy의 스폰에 필요한 데이터
 */
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public ObjectPoolTag poolTag;
    public int spawnCount;
    public SpawnVector[] spawnPoints;
}
