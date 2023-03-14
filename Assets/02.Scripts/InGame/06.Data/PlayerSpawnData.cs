/*
작성자: 최재호(cjh0798@gmail.com)
기능: Player 스폰에 필요한 데이터
 */
[System.Serializable]
public class PlayerSpawnData
{
    public ObjectPoolTag poolTag;
    public string nickname;
    public int spawnCount;
    public bool isMine;
    public SpawnVector[] spawnPoints;
}
