using System.Collections;
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

    public Dictionary<BuildingStates, int> ConditionInfo { get; private set; }
    public override void Setup()
    {
        base.Setup();

        gameObject.name = $"{GetType().Name}_{ID}";

        ConditionInfo = new Dictionary<BuildingStates, int>();
        int restValue = structures.Count % 3;
        int highCondition = structures.Count - (structures.Count / 3 + restValue);
        int middleCondition = highCondition - (structures.Count / 3);
        int lowCondition = structures.Count / 3;
        AliveStructureCount = structures.Count;
        ConditionInfo.Add(BuildingStates.HighCondition, highCondition);
        ConditionInfo.Add(BuildingStates.MiddleCondition, middleCondition);
        ConditionInfo.Add(BuildingStates.LowCondition, lowCondition);

        states = new State<Building>[5];
        states[(int)BuildingStates.HighCondition] = new HighCondition();
        states[(int)BuildingStates.MiddleCondition] = new MiddleCondition();
        states[(int)BuildingStates.LowCondition] = new LowCondition();
        states[(int)BuildingStates.OnDestroyStructure] = new OnDestroyStructure();
        states[(int)BuildingStates.Die] = new Die();

        stateMachine = new BattleStateMachine<Building>();
        stateMachine.Setup(this, states[(int)EnemyStates.RunBuilding]);

    }

    public override void Updated()
    {
        stateMachine.Excute();
    }

    public void ChangeState(BuildingStates newState)
    {
        CurState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }

    public override void OnMessage(EntityMessage entityMessage)
    {
        stateMachine.OnMessage(entityMessage);
    }

    public override void OnDamaged(int damage, int senderID)
    {

    }

    public void OnDestroyStructure(Structure destroyedStructure, int receiverID, int senderID)
    {
        int structureIndex = structures.IndexOf(destroyedStructure);
        EntityMessage message = EntityMessanger.Instance.CreateMessage(structureIndex.ToString(), MessageType.DestroyedStructure, receiverID, senderID);
        stateMachine.ChangeState(states[(int)BuildingStates.OnDestroyStructure]);
        OnMessage(message);
    }

}
