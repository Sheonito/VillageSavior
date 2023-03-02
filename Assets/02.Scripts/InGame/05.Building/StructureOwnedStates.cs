using UnityEngine;

namespace StructureOwnedStates
{
    public class Idle : State<Structure>
    {
        private Structure ownerEntity;
        public override void Enter(Structure entity)
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
    public class Damaged : State<Structure>
    {
        private int damage;
        private Structure ownerEntity;
        public override void Enter(Structure entity)
        {
            Log.PrintLogMiddleLevel("Structure Damaged Enter");
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
            if (string.IsNullOrEmpty(entityMessage.message) == false && entityMessage.type == MessageType.Damaged)
            {
                damage = int.Parse(entityMessage.message);
                OnDamaged();
            }
        }

        private void OnDamaged()
        {
            if (damage == 0)
            {
                Log.PrintLogLowLevel("데미지를 가져오지 못했습니다.");
                return;
            }

            ownerEntity.hp -= damage;
            //IngameUtil.ShowDmageText(damage,entity.damageText);
            Log.PrintLogMiddleLevel($"{ownerEntity.name} HP: " + ownerEntity.hp);

            if (ownerEntity.hp <= 0)
            {
                ownerEntity.ChangeState(StructureStates.Die);
            }
        }
    }
    public class Die : State<Structure>
    {
        private Structure ownerEntity;
        public override void Enter(Structure entity)
        {
            Log.PrintLogMiddleLevel("Structure Die Enter");
            ownerEntity = entity;

            ownerEntity.IsDead = true;
            ownerEntity.aiTarget.IsDead = true;

            int receiverID = ownerEntity.ParentBuilding.ID;
            int senderID = ownerEntity.ID;
            ownerEntity.ParentBuilding.OnDestroyStructure(ownerEntity, receiverID, senderID);
            ownerEntity.gameObject.SetActive(false);
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
