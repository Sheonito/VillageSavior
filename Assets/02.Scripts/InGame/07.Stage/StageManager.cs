/*
작성자: 최재호(cjh0798@gmail.com)
기능: FSM을 활용하여 스테이지 관리
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

    // 초기화
    public override void Setup()
    {
        base.Setup();

        CurLevelData = stageData.levelData[0];

        // StageManager State 인스턴트 생성
        states = new State<StageManager>[5];
        states[(int)StageManagerState.NormalStage] = new NormalStage();
        states[(int)StageManagerState.BossStage] = new BossStage();
        states[(int)StageManagerState.LastStage] = new LastStage();
        states[(int)StageManagerState.StageClear] = new StageClear();
        states[(int)StageManagerState.CheckStageLevel] = new CheckStageLevel();

        // StateMachine 인스턴트 생성 및 Setup
        stateMachine = new StateMachine<StageManager>();
        stateMachine.Setup(this, states[(int)StageManagerState.NormalStage]);
        stateMachine.SetGlobalState(states[(int)StageManagerState.CheckStageLevel]);

        // Player 생성
        playerManager.CreatePlayer(CurLevelData.playerData);
    }

    public override void OnMessage(EntityMessage entityMessage)
    {

    }

    // Update에서 매 프레임 마다 호출
    public override void Updated()
    {
        stateMachine.Excute();
    }

    // State 변경
    public void ChangeState(StageManagerState newState)
    {
        CurState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }

    // 다음 StageLevel로 변경
    public StageLevel ChangeNextLevel()
    {
        CurLevel = CurLevel + 1;
        CurLevelData = stageData.levelData[(int)CurLevel];

        EntityMessage message = EntityMessanger.Instance.CreateMessage((CurLevel + 1).ToString(), MessageType.StageLevelChange, ID, ID);
        EntityMessanger.Instance.SendMessage(0,message).Forget();
        return CurLevel;
    }

}
