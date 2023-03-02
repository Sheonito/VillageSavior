/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: BattleFSM�� State�� ����
 */
using UnityEngine;

public class BattleStateMachine<T> : StateMachine<T> where T : BattleEntity // �ڵ��� ������ ���� T�� BattleEntity�� ����
{
    // ����Ƽ Update���� �� ������ ȣ��
    public override void Excute()
    {
        if (ownerEntity.IsDead)
            return;

        base.Excute();
    }

    public override void ChangeState(State<T> newState)
    {
        if (newState == null || ownerEntity.IsDead)
            return;

        base.ChangeState(newState);
    }
}
