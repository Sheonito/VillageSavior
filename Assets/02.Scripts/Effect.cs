/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: ����Ʈ OnOff
 */
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] Hitbox hitbox; // ��Ʈ�ڽ� �ݶ��̴�

    /// <summary>
    /// ����Ʈ SetActive(true)
    /// <para>
    /// ex) 1�� = _destroyTime = 1000
    /// </para>
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

    // ����Ʈ SetActive(Off)
    private async void DestroyEffect(int delayTime)
    {
        await UniTask.Delay(delayTime);
        gameObject.SetActive(false);
    }
}
