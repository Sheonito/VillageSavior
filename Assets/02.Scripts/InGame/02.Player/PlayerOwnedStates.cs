/*
작성자: 최재호(cjh0798@gmail.com)
기능: Player의 모든 State
 */
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayerOwnedStates
{
    // 가만히 있는 상태
    public class Idle : State<Player>
    {
        private Player ownerEntity;
        public override void Enter(Player entity)
        {
            Log.PrintLogMiddleLevel("Player Idle Enter 진입");

            ownerEntity = entity;
        }

        public override void Excute()
        {
            PlayAnimation();
        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage message)
        {

        }

        private void PlayAnimation()
        {
            ownerEntity.Animator.Play("Idle_C");
        }
    }

    // 달리고 있는 상태 
    public class Run : State<Player>
    {
        private Player ownerEntity;
        public override void Enter(Player entity)
        {
            Log.PrintLogMiddleLevel("Player Run Enter");

            ownerEntity = entity;
        }

        public override void Excute()
        {
            if (ownerEntity.MoveDir == Vector3.zero)
                ownerEntity.ChangeState(PlayerStates.Idle);

            Move();
            Rotate();
            PlayAnimation();
        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage message)
        {

        }

        private void Move()
        {
            Vector3 moveDir = ownerEntity.MoveDir;
            ownerEntity.Controller.Move(moveDir * ownerEntity.moveSpeed * Time.deltaTime);
        }

        private void Rotate()
        {
            Vector3 moveDir = ownerEntity.MoveDir;
            Vector3 dirVector = Vector3.ClampMagnitude(new Vector3(moveDir.x, 0f, moveDir.z), 1f);
            Vector3 moveVector = dirVector.normalized;

            if (moveVector == Vector3.zero)
                return;

            Quaternion rotation = Quaternion.LookRotation(moveVector);
            ownerEntity.transform.rotation = Quaternion.Slerp(ownerEntity.transform.rotation, rotation, Time.deltaTime * 10f);
        }
        private void PlayAnimation()
        {
            ownerEntity.Animator.Play("Run");
        }
    }

    // 데미지를 받은 상태
    public class Damaged : State<Player>
    {
        private Player ownerEntity;
        private int damage;
        public override void Enter(Player entity)
        {
            Log.PrintLogMiddleLevel("Player Damaged Enter");
            ownerEntity = entity;
        }

        public override void Excute()
        {

        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage message)
        {
            // 메세지를 통해 Hitbox로부터 데미지 수신
            if (string.IsNullOrEmpty(message.message) == false && message.type == MessageType.Damaged)
            {
                damage = int.Parse(message.message);
                OnDamaged();
            }
        }

        // 데미지 피해 처리
        private void OnDamaged()
        {
            ownerEntity.hp -= damage;
            Log.PrintLogMiddleLevel("player HP: " + ownerEntity.hp);

            if (ownerEntity.hp <= 0)
            {
                ownerEntity.ChangeState(PlayerStates.Die);
            }
            else
            {
                PlayAnimation();
            }
        }

        private void PlayAnimation()
        {
            ownerEntity.Animator.Play(PlayerAnimClips.Damaged.ToString());
        }
    }

    // 공격중인 상태
    public class Attack : State<Player>
    {
        private Player ownerEntity;
        public async override void Enter(Player entity)
        {
            Log.PrintLogMiddleLevel("Player Attack Enter");

            ownerEntity = entity;
            ownerEntity.IsAttacking = true;
            await PlayAnimation();
            ownerEntity.ChangeState(PlayerStates.Idle);
        }

        public override void Excute()
        {

        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage telegram)
        {

        }

        private async UniTask PlayAnimation()
        {
            int attackIndex = Random.Range(1, 3);
            PlayerAnimClips clipName = default;

            switch (attackIndex)
            {
                case 1:
                    clipName = PlayerAnimClips.Atk1;
                    break;

                case 2:
                    clipName = PlayerAnimClips.Atk2;
                    break;
            }


            ownerEntity.Animator.Play(clipName.ToString());

            // 애니메이션 동작 시간 가져오기
            int delayTime = AnimationUtil.GetAnimationDelay(ownerEntity.Animator, clipName.ToString());

            CreateEffect(delayTime);

            await UniTask.Delay(delayTime);
            ownerEntity.IsAttacking = false;
        }

        private void CreateEffect(int destroyTime)
        {
            Effect effect = ObjectPool.Instance.GetObject(ObjectPoolTag.warriorEffect_01)?.GetComponent<Effect>();
            Vector3 spawnPos = ownerEntity.transform.position + new Vector3(0, 1, 0);
            Quaternion spawnRot = ownerEntity.transform.rotation;
            effect.Setup(ownerEntity, destroyTime, spawnPos, spawnRot);
        }
    }

    // 죽은 상태
    public class Die : State<Player>
    {
        private Player ownerEntity;
        public override void Enter(Player entity)
        {
            Log.PrintLogMiddleLevel("Player Dead Enter");

            ownerEntity = entity;
            ownerEntity.IsDead = true;
            ownerEntity.GetComponent<AITarget>().IsDead = true;
            PlayAnimation(ownerEntity);
        }

        public override void Excute()
        {

        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage message)
        {

        }

        private void PlayAnimation(Player ownerEntity)
        {
            ownerEntity.Animator.Play(PlayerAnimClips.Die.ToString());
        }
    }
}