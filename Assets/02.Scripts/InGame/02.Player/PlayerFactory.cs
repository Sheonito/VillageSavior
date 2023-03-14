/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: Player ���� �� Player Setup
 */
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{
    public GameObject warrierPrefab;

    [SerializeField]
    private PlayerCam playerCam;


    // Player ����
    public void CreatePlayer(List<PlayerSpawnData> spawnData)
    {
        // ���� ���� Count ��ŭ �ݺ�
        for (int i = 0; i < spawnData.Count; i++)
        {
            // �ش� ������ Count ��ŭ �ݺ�
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