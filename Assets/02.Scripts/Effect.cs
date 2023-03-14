/*
작성자: 최재호(cjh0798@gmail.com)
기능: 이펙트 OnOff
 */
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] Hitbox hitbox; // 히트박스 콜라이더

    /// <summary>
    /// 이펙트 SetActive(true)
    /// <para>
    /// ex) 1초 = _destroyTime = 1000
    /// </para>
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

    // 이펙트 SetActive(Off)
    private async void DestroyEffect(int delayTime)
    {
        await UniTask.Delay(delayTime);
        gameObject.SetActive(false);
    }
}
