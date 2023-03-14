/*
작성자: 최재호(cjh0798@gmail.com)
기능: Player 생성 및 Player Setup
 */
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{
    public GameObject warrierPrefab;

    [SerializeField]
    private PlayerCam playerCam;


    // Player 생성
    public void CreatePlayer(List<PlayerSpawnData> spawnData)
    {
        // 직업 종류 Count 만큼 반복
        for (int i = 0; i < spawnData.Count; i++)
        {
            // 해당 직업의 Count 만큼 반복
            for (int j = 0; j < spawnData[i].spawnCount; j++)
            {
                Player player = ObjectPool.Instance.GetObject(spawnData[i].poolTag)?.GetComponent<Player>();

                int ranSpawnPoint = Random.Range(0, spawnData[i].spawnPoints.Length);
                SpawnVector spawnVector = spawnData[i].spawnPoints[ranSpawnPoint];
                Vector3 spawnPos = new Vector3(spawnVector.x, spawnVector.y, spawnVector.z);
                player.transform.position = spawnPos;

                if (spawnData[i].isMine)
                {
                    player.IsMine = true;
                    playerCam.Setup(player);
                }
                player.Setup();
            }
        }
    }

}