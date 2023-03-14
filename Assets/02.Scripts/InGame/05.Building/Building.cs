/*
작성자: 최재호(cjh0798@gmail.com)
기능: FSM을 사용하는 Building
 */
using System.Collections.Generic;
using UnityEngine;
using BuildingOwnedStates;

public enum BuildingStates
{
    HighCondition,
    MiddleCondition,
    LowCondition,
    OnDestroyStructure,
    Die,
}

public class Building : BattleEntity
{
    public BuildingStates CurState;
    private State<Building>[] states;
    private BattleStateMachine<Building> stateMachine;
    public List<Structure> structures;
    public int AliveStructureCount { get; set; }

    public Dictionary<BuildingStates, int> ConditionStructureCount { get; private set; } // 각 건물 상태의 Structure 숫자

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        Updated();
    }

    // 초기화
    public override void Setup()
    {
        base.Setup();

        gameObject.name = $"{GetType().Name}_{ID}";

        // Building 상태에 따른 Structure 숫자 Setup
        ConditionStructureCount = new Dictionary<BuildingStates, int>();
        int restValue = structures.Count % 3;
        int highCondition = structures.Count - (structures.Count / 3 + restValue);
        int middleCondition = highCondition - (structures.Count / 3);
        int lowCondition = structures.Count / 3;
        AliveStructureCount = structures.Count;
        ConditionStructureCount.Add(BuildingStates.HighCondition, highCondition);
        ConditionStructureCount.Add(BuildingStates.MiddleCondition, middleCondition);
        ConditionStructureCount.Add(BuildingStates.LowCondition, lowCondition);

        // Building State 인스턴트 생성
        states = new State<Building>[5];
        states[(int)BuildingStates.HighCondition] = new HighCondition();
        states[(int)BuildingStates.MiddleCondition] = new MiddleCondition();
        states[(int)BuildingStates.LowCondition] = new LowCondition();
        states[(int)BuildingStates.OnDestroyStructure] = new DestroyStructure();
        states[(int)BuildingStates.Die] = new Die();

        // StateMachine 인스턴트 생성 및 Setup
        stateMachine = new BattleStateMachine<Building>();
        stateMachine.Setup(this, states[(int)EnemyStates.RunBuilding]);

    }

    // Update에서 매 프레임 마다 호출
    public override void Updated()
    {
        stateMachine.Excute();
    }

    // State 변경
    public void ChangeState(BuildingStates newState)
    {
        CurState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }

    // EntityMessage를 통해 외부 클래스에서 데이터 수신
    public override void OnMessage(EntityMessage entityMessage)
    {
        stateMachine.OnMessage(entityMessage);
    }

    public override void OnDamaged(int damage, int senderID)
    {

    }

    // Building이 가지고 있는 Structure가 파괴되면 호출되는 함수
    public void OnDestroyStructure(Structure destroyedStructure, int receiverID, int senderID)
    {
        int structureIndex = structures.IndexOf(destroyedStructure);
        EntityMessage message = EntityMessanger.Instance.CreateMessage(structureIndex.ToString(), MessageType.DestroyedStructure, receiverID, senderID);
        stateMachine.ChangeState(states[(int)BuildingStates.OnDestroyStructure]);
        OnMessage(message);
    }

}
