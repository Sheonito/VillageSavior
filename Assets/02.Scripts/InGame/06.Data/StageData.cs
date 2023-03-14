/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: ���������� �ʿ��� ������
 */
using System;
using System.Collections.Generic;

// �������� ����
public enum StageLevel
{
    Stage1,
    Stage2,
    Stage3,
    LastStage
}

// �������� Ŭ���� ��ǥ
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
