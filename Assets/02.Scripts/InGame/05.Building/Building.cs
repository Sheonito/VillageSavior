/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: FSM�� ����ϴ� Building
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

    public Dictionary<BuildingStates, int> ConditionStructureCount { get; private set; } // �� �ǹ� ������ Structure ����

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        Updated();
    }

    // �ʱ�ȭ
    public override void Setup()
    {
        base.Setup();

        gameObject.name = $"{GetType().Name}_{ID}";

        // Building ���¿� ���� Structure ���� Setup
        ConditionStructureCount = new Dictionary<BuildingStates, int>();
        int restValue = structures.Count % 3;
        int highCondition = structures.Count - (structures.Count / 3 + restValue);
        int middleCondition = highCondition - (structures.Count / 3);
        int lowCondition = structures.Count / 3;
        AliveStructureCount = structures.Count;
        ConditionStructureCount.Add(BuildingStates.HighCondition, highCondition);
        ConditionStructureCount.Add(BuildingStates.MiddleCondition, middleCondition);
        ConditionStructureCount.Add(BuildingStates.LowCondition, lowCondition);

        // Building State �ν���Ʈ ����
        states = new State<Building>[5];
        states[(int)BuildingStates.HighCondition] = new HighCondition();
        states[(int)BuildingStates.MiddleCondition] = new MiddleCondition();
        states[(int)BuildingStates.LowCondition] = new LowCondition();
        states[(int)BuildingStates.OnDestroyStructure] = new DestroyStructure();
        states[(int)BuildingStates.Die] = new Die();

        // StateMachine �ν���Ʈ ���� �� Setup
        stateMachine = new BattleStateMachine<Building>();
        stateMachine.Setup(this, states[(int)EnemyStates.RunBuilding]);

    }

    // Update���� �� ������ ���� ȣ��
    public override void Updated()
    {
        stateMachine.Excute();
    }

    // State ����
    public void ChangeState(BuildingStates newState)
    {
        CurState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }

    // EntityMessage�� ���� �ܺ� Ŭ�������� ������ ����
    public override void OnMessage(EntityMessage entityMessage)
    {
        stateMachine.OnMessage(entityMessage);
    }

    public override void OnDamaged(int damage, int senderID)
    {

    }

    // Building�� ������ �ִ� Structure�� �ı��Ǹ� ȣ��Ǵ� �Լ�
    public void OnDestroyStructure(Structure destroyedStructure, int receiverID, int senderID)
    {
        int structureIndex = structures.IndexOf(destroyedStructure);
        EntityMessage message = EntityMessanger.Instance.CreateMessage(structureIndex.ToString(), MessageType.DestroyedStructure, receiverID, senderID);
        stateMachine.ChangeState(states[(int)BuildingStates.OnDestroyStructure]);
        OnMessage(message);
    }

}
