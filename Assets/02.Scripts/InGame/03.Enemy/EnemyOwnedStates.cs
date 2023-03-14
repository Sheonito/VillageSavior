/*
작성자: 최재호(cjh0798@gmail.com)
기능: Enemy의 모든 State
 */
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace EnemyOwnedStates
{
    // 가만히 있는 상태
    public class Idle : State<Enemy>
    {
        private Enemy ownerEntity;
        public override void Enter(Enemy entity)
        {
            Log.PrintLogMiddleLevel("Enemy Idle Enter");
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

        }
    }

    // Building으로 달려가고 있는 상태 
    public class RunBuilding : State<Enemy>
    {
        private Enemy ownerEntity;
        public override void Enter(Enemy entity)
        {
            Log.PrintLogMiddleLevel("Enemy RunBuilding Enter");

            ownerEntity = entity;

            // Enemy AITarget 불러오기
            AITarget aiTarget = ownerEntity.aiManager.GetTarget(ownerEntity.transform);

            if (aiTarget == null)
            {
                ownerEntity.ChangeState(EnemyStates.Idle);
                return;
            }

            // Enemy 목적지 설정
            ownerEntity.curAITarget = aiTarget;
            Transform destTransform = aiTarget.transform;
            float stoppedDistance = aiTarget.stoppingDistance;
            SetDestination(ownerEntity, stoppedDistance, destTransform);

            PlayAnimation(ownerEntity);
        }

        public override async void Excute()
        {
            await UniTask.Yield();
            if (ownerEntity.Agent == null)
                return;

            bool isAttackRange = ownerEntity.Agent.remainingDistance <= ownerEntity.Agent.stoppingDistance;
            bool isCoolTime = ownerEntity.curAttackCoolTime >= ownerEntity.attackCoolTime;

            // Agent SetDestination 시 일시적으로 remainingDistance가 0이 되어 공격하는 버그 예외처리
            if (ownerEntity.Agent.remainingDistance == 0 || ownerEntity == null) 
                return;

            // 공격범위 안에 있고 공격 쿨타임이 차면 공격
            else if (isAttackRange && isCoolTime)
            {
                ownerEntity.ChangeState(EnemyStates.Attack);
            }
            else
            {
                return;
            }
        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage message)
        {

        }

        // Enemy 목적지 설정
        private void SetDestination(Enemy enemy, float stoppingDinstance, Transform destTransform)
        {
            enemy.Agent.isStopped = false;
            enemy.Agent.stoppingDistance = stoppingDinstance;
            enemy.curDestTrans = destTransform;
            enemy.Agent.SetDestination(enemy.curDestTrans.position);
            enemy.curAITargetTag = AITargetTag.Building;

        }

        private void PlayAnimation(Enemy enemy)
        {
            enemy.Animator.Play(EnemyAnimClips.Run.ToString());

        }
    }

    // Player에게 달려가고 있는 상태
    public class RunPlayer : State<Enemy>
    {
        private Enemy ownerEntity;
        public override void Enter(Enemy entity)
        {
            ownerEntity = entity;
            Log.PrintLogMiddleLevel("Enemy RunPlayer Enter");

            SetDestination(ownerEntity);
        }

        public override void Excute()
        {
            bool isAttackRange = ownerEntity.Agent.remainingDistance <= ownerEntity.Agent.stoppingDistance;
            bool isCoolTime = ownerEntity.curAttackCoolTime >= ownerEntity.attackCoolTime;

            // 공격범위 안에 있고 공격 쿨타임이 차면 공격
            if (isAttackRange && isCoolTime)
            {
                ownerEntity.ChangeState(EnemyStates.Attack);
            }
            else
            {
                // Enemy 목적지 설정
                SetDestination(ownerEntity);
            }
        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage message)
        {

        }

        // Enemy 목적지 설정
        private void SetDestination(Enemy entity)
        {
            AITarget aiTarget = entity.curDestTrans.GetComponent<AITarget>();
            entity.curAITarget = aiTarget;
            entity.Agent.isStopped = false;
            entity.Agent.stoppingDistance = aiTarget.stoppingDistance;
            entity.Agent.SetDestination(entity.curDestTrans.position);
            entity.curAITargetTag = AITargetTag.Player;
        }
    }

    // 데미지를 받은 상태
    public class Damaged : State<Enemy>
    {
        private Enemy ownerEntity;
        private int damage;
        public override void Enter(Enemy entity)
        {
            Log.PrintLogMiddleLevel("Enemy Damaged Enter");
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
                Entity targetEntity = EntityDatabase.Instance.GetEntity(entityMessage.sender);
                ownerEntity.curDestTrans = targetEntity.transform;

                OnDamaged().Forget();
            }
        }

        // 데미지 피해 처리
        private async UniTaskVoid OnDamaged()
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
                ownerEntity.ChangeState(EnemyStates.Die);
            }
            else
            {
                await PlayAnimation();
                ownerEntity.ChangeState(EnemyStates.RunPlayer);
            }
        }

        private async UniTask PlayAnimation()
        {
            ownerEntity.Animator.Play(EnemyAnimClips.Damaged.ToString());
            int delayTime = AnimationUtil.GetAnimationDelay(ownerEntity.Animator, EnemyAnimClips.Damaged.ToString());
            await UniTask.Delay(delayTime);
        }
    }

    // 공격중인 상태
    public class Attack : State<Enemy>
    {
        private Enemy ownerEntity;
        public override void Enter(Enemy entity)
        {
            Log.PrintLogMiddleLevel("Enemy Attack Enter");
            ownerEntity = entity;
        }

        public override void Excute()
        {
            bool isCoolTime = ownerEntity.curAttackCoolTime >= ownerEntity.attackCoolTime;

            // 쿨타임이 찼을 때
            if (isCoolTime)
            {
                // 공격 타겟이 Player일 때
                if (ownerEntity.curAITargetTag == AITargetTag.Player)
                {
                    AttackPlayer();
                }
                // 공격 타겟이 Building일 때
                else if (ownerEntity.curAITargetTag == AITargetTag.Building)
                {
                    AttackBuilding();
                }
                Vector3 lookAtPos = ownerEntity.curAITarget.transform.position;
                ownerEntity.transform.DOLookAt(lookAtPos, 0.5f);
                
                // 공격 후 쿨타임 및 타겟 초기화
                ownerEntity.curAttackCoolTime = 0;
                ownerEntity.curAITargetTag = AITargetTag.None;
            }
        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage message)
        {

        }

        private async UniTask PlayAnimation(EnemyAnimClips animClip)
        {
            ownerEntity.Animator.Play(animClip.ToString());
            string clipName = animClip.ToString();
            int delayTime = AnimationUtil.GetAnimationDelay(ownerEntity.Animator, clipName);
            await UniTask.Delay(delayTime);
        }

        private void AttackPlayer()
        {
            Player player = ownerEntity.curAITarget.GetComponent<Player>();
            player.OnDamaged(ownerEntity.attackPower, ownerEntity.ID);
        }

        private void AttackBuilding()
        {
            Structure structure = ownerEntity.curAITarget.GetComponent<Structure>();
            structure.OnDamaged(ownerEntity.attackPower, ownerEntity.ID);
        }
    }

    // 죽은 상태
    public class Die : State<Enemy>
    {
        private Enemy ownerEntity;
        public override async void Enter(Enemy entity)
        {
            Log.PrintLogMiddleLevel("Enemy Dead Enter");

            ownerEntity = entity;
            ownerEntity.IsDead = true;

            // 애니메이션 모션이 끝나면 SetActive(false)를 통해 ObjectPool로 돌려보내기
            await PlayAnimation();
            ownerEntity.gameObject.SetActive(false);
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

        private async UniTask PlayAnimation()
        {
            string clipName = EnemyAnimClips.Die.ToString();
            ownerEntity.Animator.Play(clipName);

            int delayTime = AnimationUtil.GetAnimationDelay(ownerEntity.Animator, clipName);
            await UniTask.Delay(delayTime);
        }
    }

    // 공격 타겟 검색 상태
    public class SearchTarget : State<Enemy>
    {
        private Enemy ownerEntity;
        public override void Enter(Enemy entity)
        {
            Log.PrintLogMiddleLevel("Enemy SearchTarget Enter");
            ownerEntity = entity;
        }

        public override void Excute()
        {
            // Enemy가 공격 모션중일 때는 return
            if (ownerEntity.isAttacking)
                return;

            // 일정 범위 내 Player가 있는 지 검색
            AITarget player = ownerEntity.aiManager.SearchPlayer(ownerEntity.transform);

            // Player가 검색되면 Player를 향해 Run
            if (player != null && ownerEntity.curAITargetTag != AITargetTag.Player)
            {
                Transform destTransform = player.transform;
                ownerEntity.curAITarget = player;
                ownerEntity.curDestTrans = destTransform;
                ownerEntity.Agent.isStopped = false;
                ownerEntity.ChangeState(EnemyStates.RunPlayer);
            }
            // Player가 검색되지 않으면 Building을 향해 Run
            else if (player == null && ownerEntity.curAITargetTag != AITargetTag.Building)
            {
                ownerEntity.Agent.isStopped = false;
                ownerEntity.ChangeState(EnemyStates.RunBuilding);
            }
        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage message)
        {

        }
    }
}