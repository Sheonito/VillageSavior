/*
작성자: 최재호(cjh0798@gmail.com)
기능: BattleFSM에 사용되는 부모 전투 객체
 */
using UnityEngine;

public abstract class BattleEntity : Entity
{
    public int hp;
    public int attackPower;
    public bool IsDead { get; set; }

    // EntityMessage를 통해 외부 클래스에서 데이터 수신
    public override void OnMessage(EntityMessage entityMessage)
    {

    }

    // 유니티 Update에서 매 프레임 마다 호출
    public override void Updated()
    {

    }
    // Hitbox를 통해 데미지를 받았을 때 불리는 함수
    public abstract void OnDamaged(int damage, int senderID);
}
