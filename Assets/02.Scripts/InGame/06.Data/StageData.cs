/*
작성자: 최재호(cjh0798@gmail.com)
기능: 스테이지에 필요한 데이터
 */
using System;
using System.Collections.Generic;

// 스테이지 레벨
public enum StageLevel
{
    Stage1,
    Stage2,
    Stage3,
    LastStage
}

// 스테이지 클리어 목표
public enum StageClearGoal
{
    Time,
    AllEnemy,
    Boss
}

[Serializable]
public class StageData
{
    public StageLevelData[] levelData;
}

[Serializable]
public class StageLevelData
{
    public StageLevel level;
    public StageClearGoal clearGoal;
    public List<PlayerSpawnData> playerData;
    public List<EnemySpawnData> enemyData;
    public float spawnCoolTime;
    public float clearTime;
}
