/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: BattleFSM�� ���Ǵ� �θ� ���� ��ü
 */
using UnityEngine;

public abstract class BattleEntity : Entity
{
    public int hp;
    public int attackPower;
    public bool IsDead { get; set; }

    // EntityMessage�� ���� �ܺ� Ŭ�������� ������ ����
    public override void OnMessage(EntityMessage entityMessage)
    {

    }

    // ����Ƽ Update���� �� ������ ���� ȣ��
    public override void Updated()
    {

    }
    // Hitbox�� ���� �������� �޾��� �� �Ҹ��� �Լ�
    public abstract void OnDamaged(int damage, int senderID);
}
