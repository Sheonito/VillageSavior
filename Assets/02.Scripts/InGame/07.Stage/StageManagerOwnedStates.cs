using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace StageManagerOwnedStates
{
    public class NormalStage : State<StageManager>
    {
        private StageManager ownerEntity;
        private StageLevelData levelData;
        private float spawnTime;
        public override void Enter(StageManager entity)
        {
            Log.PrintLogMiddleLevel("StageManager NormalStage Enter");
            ownerEntity = entity;
            ownerEntity.playerManager.Setup();
            ownerEntity.enemyFactory.Setup();

            levelData = ownerEntity.stageData.levelData[(int)ownerEntity.CurLevel];
            List<PlayerSpawnData> playerSpawnData = levelData.playerData;
            List<EnemySpawnData> enemySpawnData = levelData.enemyData;
            ownerEntity.playerManager.CreatePlayer(playerSpawnData);
            ownerEntity.enemyFactory.CreateEnemy(enemySpawnData);
        }

        public override void Excute()
        {
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


            switch (clearGoal)
            {
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

                case StageClearGoal.AllEnemy:
                    break;

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

