using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] Hitbox hitbox;

    /// <summary>
    /// ex) 1초 = _destroyTime = 1000
    /// </summary>
    public void Setup(BattleEntity _ownerEntity,int _destroyTime,Vector3 pos,Quaternion rot)
    {
        if (_destroyTime < 50)
            Log.PrintLogLowLevel("_destroyTime 값을 잘못 입력하셨습니다. 입력한 값에 1000을 곱해야 합니다.");

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
