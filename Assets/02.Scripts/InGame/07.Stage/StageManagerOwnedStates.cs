/*
작성자: 최재호(cjh0798@gmail.com)
기능: StageManager의 모든 State
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace StageManagerOwnedStates
{
    // 일반 몬스터만 나오는 스테이지
    public class NormalStage : State<StageManager>
    {
        private StageManager ownerEntity;
        private StageLevelData levelData;
        private float spawnTime;
        public override void Enter(StageManager entity)
        {
            Log.PrintLogMiddleLevel("StageManager NormalStage Enter");
            ownerEntity = entity;

            // Enemy 생성
            levelData = ownerEntity.stageData.levelData[(int)ownerEntity.CurLevel];
            List<EnemySpawnData> enemySpawnData = levelData.enemyData;
            ownerEntity.enemyFactory.CreateEnemy(enemySpawnData);
        }

        public override void Excute()
        {
            // spawnTime마다 Enemy 생성
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

        // EntityMessage를 통해 levelData 불러오기
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

    // 일반 몬스터와 보스 몬스터가 나오는 스테이지
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

    // 마지막 스테이지
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

    // 스테이지를 클리어 했을 때
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

    // StageLevel을 검사하고 변경하는 상태
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

            // 스테이지 클리어 조건
            switch (clearGoal)
            {
                // 시간
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
                // 모든 적 처치
                case StageClearGoal.AllEnemy:
                    break;

                // 보스 처치
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

        // 마지막 스테이지인지 확인
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

