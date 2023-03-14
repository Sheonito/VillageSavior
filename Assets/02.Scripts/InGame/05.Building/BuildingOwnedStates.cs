/*
작성자: 최재호(cjh0798@gmail.com)
기능: Building의 모든 State
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingOwnedStates
{
    // 추후 Building이 Player에게 기능을 제공
    // 컨디션 상태에 따라 Building이 Player에게 제공하는 기능 제한


    // 하이 컨디션 상태
    public class HighCondition : State<Building>
    {
        private Building ownerEntity;
        public override void Enter(Building entity)
        {
            ownerEntity = entity;
        }

        public override void Excute()
        {

        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage entitiyMessage)
        {
            
        }
    }
    // 미들 컨디션 상태
    public class MiddleCondition : State<Building>
    {
        private Building ownerEntity;
        public override void Enter(Building entity)
        {
            Log.PrintLogMiddleLevel("Building MiddleCondition Enter");
            ownerEntity = entity;
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
    // 로우 컨디션 상태
    public class LowCondition : State<Building>
    {
        private Building ownerEntity;
        public override void Enter(Building entity)
        {
            ownerEntity = entity;
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
    // Structure가 파괴되었을 때 상태
    public class DestroyStructure : State<Building>
    {
        private Building ownerEntity;
        public override void Enter(Building entity)
        {
            Log.PrintLogMiddleLevel("Building OnDestroytStructure Enter");
            ownerEntity = entity;
        }

        public override void Excute()
        {

        }

        public override void Exit()
        {

        }

        // Bulding이 가진 StructureCount 마이너스
        public override void OnMessage(EntityMessage entityMessage)
        {
            if (string.IsNullOrEmpty(entityMessage.message) != false && entityMessage.type == MessageType.DestroyedStructure)
            {
                if (ownerEntity.AliveStructureCount == 0)
                    return;

                ownerEntity.AliveStructureCount--;
                UpdateBuildingCondition();
            }
        }

        // Building이 가진 StructureCount에 따라 Condition 업데이트
        private void UpdateBuildingCondition()
        {
            if (ownerEntity.AliveStructureCount == 0)
                ownerEntity.ChangeState(BuildingStates.Die);

            else if (ownerEntity.AliveStructureCount > ownerEntity.ConditionStructureCount[BuildingStates.HighCondition])
            {
                if (ownerEntity.CurState == BuildingStates.HighCondition)
                    return;

                ownerEntity.ChangeState(BuildingStates.HighCondition);
            }
            else if (ownerEntity.AliveStructureCount > ownerEntity.ConditionStructureCount[BuildingStates.MiddleCondition])
            {
                if (ownerEntity.CurState == BuildingStates.MiddleCondition)
                    return;

                ownerEntity.ChangeState(BuildingStates.MiddleCondition);
            }
            else
            {
                if (ownerEntity.CurState == BuildingStates.LowCondition)
                    return;

                ownerEntity.ChangeState(BuildingStates.LowCondition);
            }
        }
    }
    // Building이 파괴되었을 때 상태
    public class Die : State<Building>
    {
        private Building ownerEntity;
        public override void Enter(Building entity)
        {
            Log.PrintLogMiddleLevel("Building Die Enter");
            ownerEntity = entity;
            ownerEntity.IsDead = true;
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
}
