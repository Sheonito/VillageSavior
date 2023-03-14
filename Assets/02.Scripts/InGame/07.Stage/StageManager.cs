/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: FSM�� Ȱ���Ͽ� �������� ����
 */
using StageManagerOwnedStates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageManagerState
{
    NormalStage,
    BossStage,
    LastStage,
    StageClear,
    CheckStageLevel
}


public class StageManager : Entity
{
    public PlayerFactory playerManager;
    public EnemyFactory enemyFactory;
    public StageData stageData;

    public StageLevelData CurLevelData { get; private set; }
    public StageLevel CurLevel { get; private set; }
    public StageManagerState CurState { get; private set; }
    private State<StageManager>[] states;
    private StateMachine<StageManager> stateMachine;
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

        CurLevelData = stageData.levelData[0];

        // StageManager State �ν���Ʈ ����
        states = new State<StageManager>[5];
        states[(int)StageManagerState.NormalStage] = new NormalStage();
        states[(int)StageManagerState.BossStage] = new BossStage();
        states[(int)StageManagerState.LastStage] = new LastStage();
        states[(int)StageManagerState.StageClear] = new StageClear();
        states[(int)StageManagerState.CheckStageLevel] = new CheckStageLevel();

        // StateMachine �ν���Ʈ ���� �� Setup
        stateMachine = new StateMachine<StageManager>();
        stateMachine.Setup(this, states[(int)StageManagerState.NormalStage]);
        stateMachine.SetGlobalState(states[(int)StageManagerState.CheckStageLevel]);

        // Player ����
        playerManager.CreatePlayer(CurLevelData.playerData);
    }

    public override void OnMessage(EntityMessage entityMessage)
    {

    }

    // Update���� �� ������ ���� ȣ��
    public override void Updated()
    {
        stateMachine.Excute();
    }

    // State ����
    public void ChangeState(StageManagerState newState)
    {
        CurState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }

    // ���� StageLevel�� ����
    public StageLevel ChangeNextLevel()
    {
        CurLevel = CurLevel + 1;
        CurLevelData = stageData.levelData[(int)CurLevel];

        EntityMessage message = EntityMessanger.Instance.CreateMessage((CurLevel + 1).ToString(), MessageType.StageLevelChange, ID, ID);
        EntityMessanger.Instance.SendMessage(0,message).Forget();
        return CurLevel;
    }

}
