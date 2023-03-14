/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: FSM�� ����ϴ� Structure
 */
using StructureOwnedStates;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum StructureStates
{
    Idle,
    Damaged,
    Die
}

public class Structure : BattleEntity
{
    public Building ParentBuilding { get; set; }
    public AITarget aiTarget { get; private set; }

    public StructureStates CurState { get; private set; }
    private State<Structure>[] states;
    private BattleStateMachine<Structure> stateMachine;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        Updated();
    }

    // �ʱ�ȭ
    public void Setup(Building parentBuilding)
    {
        base.Setup();

        gameObject.name = $"{GetType().Name}_{ID}";

        this.ParentBuilding = parentBuilding;
        // Structure �ν����Ϳ� �ִ� ������Ʈ ����
        aiTarget = GetComponent<AITarget>();

        // Building State �ν���Ʈ ����
        states = new State<Structure>[3];
        states[(int)StructureStates.Idle] = new Idle();
        states[(int)StructureStates.Damaged] = new Damaged();
        states[(int)StructureStates.Die] = new Die();

        // StateMachine �ν���Ʈ ���� �� Setup
        stateMachine = new BattleStateMachine<Structure>();
        stateMachine.Setup(this, states[(int)StructureStates.Idle]);
    }


    // Update���� �� ������ ���� ȣ��
    public override void Updated()
    {
        stateMachine.Excute();
    }

    // State ����
    public void ChangeState(StructureStates newState)
    {
        CurState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }

    // EntityMessage�� ���� �ܺ� Ŭ�������� ������ ����
    public override void OnMessage(EntityMessage entityMessage)
    {
        stateMachine.OnMessage(entityMessage);
    }

    // ���� �޾��� �� ȣ��Ǵ� �Լ�
    public override void OnDamaged(int damage, int senderID)
    {
        EntityMessage message = EntityMessanger.Instance.CreateMessage(damage.ToString(),MessageType.Damaged, ID, senderID);
        stateMachine.ChangeState(states[(int)StructureStates.Damaged]);
        OnMessage(message);
    }
}
