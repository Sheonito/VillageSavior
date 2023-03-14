/*
작성자: 최재호(cjh0798@gmail.com)
기능: FSM을 사용하는 Structure
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

    // 초기화
    public void Setup(Building parentBuilding)
    {
        base.Setup();

        gameObject.name = $"{GetType().Name}_{ID}";

        this.ParentBuilding = parentBuilding;
        // Structure 인스펙터에 있는 컴포넌트 주입
        aiTarget = GetComponent<AITarget>();

        // Building State 인스턴트 생성
        states = new State<Structure>[3];
        states[(int)StructureStates.Idle] = new Idle();
        states[(int)StructureStates.Damaged] = new Damaged();
        states[(int)StructureStates.Die] = new Die();

        // StateMachine 인스턴트 생성 및 Setup
        stateMachine = new BattleStateMachine<Structure>();
        stateMachine.Setup(this, states[(int)StructureStates.Idle]);
    }


    // Update에서 매 프레임 마다 호출
    public override void Updated()
    {
        stateMachine.Excute();
    }

    // State 변경
    public void ChangeState(StructureStates newState)
    {
        CurState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }

    // EntityMessage를 통해 외부 클래스에서 데이터 수신
    public override void OnMessage(EntityMessage entityMessage)
    {
        stateMachine.OnMessage(entityMessage);
    }

    // 공격 받았을 때 호출되는 함수
    public override void OnDamaged(int damage, int senderID)
    {
        EntityMessage message = EntityMessanger.Instance.CreateMessage(damage.ToString(),MessageType.Damaged, ID, senderID);
        stateMachine.ChangeState(states[(int)StructureStates.Damaged]);
        OnMessage(message);
    }
}
