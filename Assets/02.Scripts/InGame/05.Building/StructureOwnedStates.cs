/*
작성자: 최재호(cjh0798@gmail.com)
기능: Structure의 모든 State
 */
using UnityEngine;

namespace StructureOwnedStates
{
    // 가만히 있는 상태
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

    // 데미지를 받은 상태
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
            // 메세지를 통해 Hitbox로부터 데미지 수신
            if (string.IsNullOrEmpty(entityMessage.message) == false && entityMessage.type == MessageType.Damaged)
            {
                damage = int.Parse(entityMessage.message);
                OnDamaged();
            }
        }

        // 데미지 피해 처리
        private void OnDamaged()
        {
            if (damage == 0)
            {
                Log.PrintLogLowLevel("데미지를 가져오지 못했습니다.");
                return;
            }

            ownerEntity.hp -= damage;
            Log.PrintLogMiddleLevel($"{ownerEntity.name} HP: " + ownerEntity.hp);

            if (ownerEntity.hp <= 0)
            {
                ownerEntity.ChangeState(StructureStates.Die);
            }
        }
    }

    // 파괴된 상태
    public class Die : State<Structure>
    {
        private Structure ownerEntity;
        public override void Enter(Structure entity)
        {
            Log.PrintLogMiddleLevel("Structure Die Enter");
            ownerEntity = entity;

            ownerEntity.IsDead = true;
            ownerEntity.aiTarget.IsDead = true;

            // Structure의 부모 Building에서 OnDestroyStructure 호출
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
