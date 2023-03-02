using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerFactory : MonoBehaviour
{
    public GameObject warrierPrefab;

    [SerializeField]
    private PlayerCam playerCam;
    
    private List<Player> playerList;
    public void Setup()
    {
        playerList = new List<Player>();
    }

    private void Update()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].Updated();
        }
    }

    public void CreatePlayer(List<PlayerSpawnData> spawnData)
    {
        for (int i = 0; i < spawnData.Count; i++)
        {
            for (int j = 0; j < spawnData[i].spawnCount; j++)
            {
                Player player = ObjectPool.Instance.GetObject(spawnData[i].poolTag)?.GetComponent<Player>();

                int ranSpawnPoint = Random.Range(0, spawnData[i].spawnPoints.Length);
                SpawnVector spawnVector = spawnData[i].spawnPoints[ranSpawnPoint];
                Vector3 spawnPos = new Vector3(spawnVector.x, spawnVector.y, spawnVector.z);
                player.transform.position = spawnPos;

                if (player != null)
                    playerList.Add(player);

                if (spawnData[i].isMine)
                { 
                    player.IsMine = true;
                    playerCam.Setup(player);
                }

                playerList[j].Setup();
            }
        }
    }

}


