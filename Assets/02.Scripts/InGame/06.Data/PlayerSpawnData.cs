using UnityEngine;

[System.Serializable]
public class PlayerSpawnData
{
    public ObjectPoolTag poolTag;
    public string nickname;
    public int spawnCount;
    public bool isMine;
    public SpawnVector[] spawnPoints;
}
