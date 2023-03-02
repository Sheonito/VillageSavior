/*
작성자: 최재호(cjh0798@gmail.com)
기능: BattleFSM의 State를 관리
 */
using UnityEngine;

public class BattleStateMachine<T> : StateMachine<T> where T : BattleEntity // 코드의 재사용을 위해 T를 BattleEntity로 제한
{
    // 유니티 Update에서 매 프레임 호출
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
