using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] Hitbox hitbox;

    /// <summary>
    /// ex) 1�� = _destroyTime = 1000
    /// </summary>
    public void Setup(BattleEntity _ownerEntity,int _destroyTime,Vector3 pos,Quaternion rot)
    {
        if (_destroyTime < 50)
            Log.PrintLogLowLevel("_destroyTime ���� �߸� �Է��ϼ̽��ϴ�. �Է��� ���� 1000�� ���ؾ� �մϴ�.");

        gameObject.transform.SetPositionAndRotation(pos, rot);
        gameObject.SetActive(true);
        hitbox.ownerEntity = _ownerEntity;
        DestroyEffect(_destroyTime);
    }

    private async void DestroyEffect(int delayTime)
    {
        await UniTask.Delay(delayTime);
        gameObject.SetActive(false);
    }
}
