using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingOwnedStates
{
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
    public class OnDestroyStructure : State<Building>
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

        private void UpdateBuildingCondition()
        {
            if (ownerEntity.AliveStructureCount == 0)
                ownerEntity.ChangeState(BuildingStates.Die);

            else if (ownerEntity.AliveStructureCount > ownerEntity.ConditionInfo[BuildingStates.HighCondition])
            {
                if (ownerEntity.CurState == BuildingStates.HighCondition)
                    return;

                ownerEntity.ChangeState(BuildingStates.HighCondition);
            }
            else if (ownerEntity.AliveStructureCount > ownerEntity.ConditionInfo[BuildingStates.MiddleCondition])
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
