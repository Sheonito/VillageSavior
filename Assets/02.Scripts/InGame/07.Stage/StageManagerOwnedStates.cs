/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: StageManager�� ��� State
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace StageManagerOwnedStates
{
    // �Ϲ� ���͸� ������ ��������
    public class NormalStage : State<StageManager>
    {
        private StageManager ownerEntity;
        private StageLevelData levelData;
        private float spawnTime;
        public override void Enter(StageManager entity)
        {
            Log.PrintLogMiddleLevel("StageManager NormalStage Enter");
            ownerEntity = entity;

            // Enemy ����
            levelData = ownerEntity.stageData.levelData[(int)ownerEntity.CurLevel];
            List<EnemySpawnData> enemySpawnData = levelData.enemyData;
            ownerEntity.enemyFactory.CreateEnemy(enemySpawnData);
        }

        public override void Excute()
        {
            // spawnTime���� Enemy ����
            spawnTime += Time.deltaTime;
            if (spawnTime >= levelData.spawnCoolTime)
            {
                spawnTime = 0;
                List<EnemySpawnData> enemySpawnData = levelData.enemyData;
                ownerEntity.enemyFactory.CreateEnemy(enemySpawnData);
            }
        }

        public override void Exit()
        {

        }

        // EntityMessage�� ���� levelData �ҷ�����
        public override void OnMessage(EntityMessage entityMessage)
        {
            if (entityMessage.type == MessageType.StageLevelChange)
            {
                int level = int.Parse(entityMessage.message);
                levelData = ownerEntity.stageData.levelData[level];

                bool isLastStage = levelData.level == StageLevel.LastStage ? true : false;
                bool isBoss = levelData.enemyData.Exists(x => x.poolTag.ToString().Contains("Boss"));

                if (isLastStage)
                {
                    ownerEntity.ChangeState(StageManagerState.LastStage);
                }
                else if (isBoss)
                {
                    ownerEntity.ChangeState(StageManagerState.BossStage);
                }
                else
                {

                }
            }

        }

    }

    // �Ϲ� ���Ϳ� ���� ���Ͱ� ������ ��������
    public class BossStage : State<StageManager>
    {
        public override void Enter(StageManager entity)
        {

        }

        public override void Excute()
        {

        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage entityMessage)
        {

        }
    }

    // ������ ��������
    public class LastStage : State<StageManager>
    {
        public override void Enter(StageManager entity)
        {

        }

        public override void Excute()
        {

        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage entityMessage)
        {

        }
    }

    // ���������� Ŭ���� ���� ��
    public class StageClear : State<StageManager>
    {
        public override void Enter(StageManager entity)
        {

        }

        public override void Excute()
        {

        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage entityMessage)
        {

        }
    }

    // StageLevel�� �˻��ϰ� �����ϴ� ����
    public class CheckStageLevel : State<StageManager>
    {
        private StageManager ownerEntity;
        private float playTime;
        public override void Enter(StageManager entity)
        {
            ownerEntity = entity;
        }

        public override void Excute()
        {
            playTime += Time.deltaTime;

            StageLevelData levelData = ownerEntity.CurLevelData;
            StageClearGoal clearGoal = levelData.clearGoal;

            // �������� Ŭ���� ����
            switch (clearGoal)
            {
                // �ð�
                case StageClearGoal.Time:
                    if (playTime >= levelData.clearTime)
                    {
                        if (IsLastStage() == true)
                        {
                            ownerEntity.ChangeState(StageManagerState.StageClear);
                        }
                        else
                        {
                            ownerEntity.ChangeNextLevel();
                        }
                    }
                    break;
                // ��� �� óġ
                case StageClearGoal.AllEnemy:
                    break;

                // ���� óġ
                case StageClearGoal.Boss:
                    break;
            }
        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage entityMessage)
        {

        }

        // ������ ������������ Ȯ��
        private bool IsLastStage()
        {
            if (ownerEntity.CurLevel == StageLevel.LastStage)
            {
                return true;
            }
            else
                return false;
        }
    }
}

