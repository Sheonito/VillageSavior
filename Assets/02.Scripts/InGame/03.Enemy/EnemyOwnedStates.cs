using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

namespace EnemyOwnedStates
{
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
    public class RunBuilding : State<Enemy>
    {
        private Enemy ownerEntity;
        public override async void Enter(Enemy entity)
        {
            Log.PrintLogMiddleLevel("Enemy RunBuilding Enter");

            ownerEntity = entity;
            AITarget aiTarget = ownerEntity.aiManager.GetTarget(ownerEntity.transform);

            if (aiTarget == null)
            {
                ownerEntity.ChangeState(EnemyStates.Idle);
                return;
            }

            ownerEntity.CurAITarget = aiTarget;
            Transform destTransform = aiTarget.transform;
            float stoppedDistance = aiTarget.stoppingDistance;
            SetDestination(ownerEntity, stoppedDistance, destTransform);

            await UniTask.WaitUntil(() => ownerEntity.Agent.remainingDistance != 0);
            PlayAnimation(ownerEntity);
        }

        public override async void Excute()
        {
            await UniTask.Yield();
            if (ownerEntity.Agent == null)
                return;

            bool isAttackRange = ownerEntity.Agent.remainingDistance <= ownerEntity.Agent.stoppingDistance;
            bool isCoolTime = ownerEntity.curAttackCoolTime >= ownerEntity.attackCoolTime;
            if (ownerEntity.Agent.remainingDistance == 0 || ownerEntity == null)
                return;

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

        private void SetDestination(Enemy enemy, float stoppingDinstance, Transform destTransform)
        {
            enemy.Agent.isStopped = false;
            enemy.Agent.stoppingDistance = stoppingDinstance;
            enemy.CurDestTrans = destTransform;
            enemy.Agent.SetDestination(enemy.CurDestTrans.position);
            enemy.CurAITargetTag = AITargetTag.Building;

        }

        private void PlayAnimation(Enemy enemy)
        {
            if (enemy.Agent.remainingDistance > enemy.Agent.stoppingDistance)
            {
                enemy.Animator.Play(EnemyAnimClips.Run.ToString());
            }

        }
    }
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
            if (isAttackRange && isCoolTime)
            {
                ownerEntity.ChangeState(EnemyStates.Attack);
            }
            else
            {
                SetDestination(ownerEntity);
            }
        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage message)
        {

        }

        private void SetDestination(Enemy entity)
        {
            AITarget aiTarget = entity.CurDestTrans.GetComponent<AITarget>();
            entity.CurAITarget = aiTarget;
            entity.Agent.isStopped = false;
            entity.Agent.stoppingDistance = aiTarget.stoppingDistance;
            entity.Agent.SetDestination(entity.CurDestTrans.position);
            entity.CurAITargetTag = AITargetTag.Player;
        }
    }
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
            if (string.IsNullOrEmpty(entityMessage.message) == false && entityMessage.type == MessageType.Damaged)
            {
                damage = int.Parse(entityMessage.message);
                Entity targetEntity = EntityDatabase.Instance.GetEntity(entityMessage.sender);
                ownerEntity.CurDestTrans = targetEntity.transform;

                OnDamaged().Forget();
            }
        }

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
    public class Attack : State<Enemy>
    {
        private Enemy ownerEntity;
        public override void Enter(Enemy entity)
        {
            Log.PrintLogMiddleLevel("Enemy Attack Enter");
            ownerEntity = entity;
        }

        public override async void Excute()
        {
            if (ownerEntity.curAttackCoolTime >= ownerEntity.attackCoolTime)
            {
                if (ownerEntity.CurAITargetTag == AITargetTag.Player)
                {
                    AttackPlayer();
                }
                else if (ownerEntity.CurAITargetTag == AITargetTag.Building)
                {
                    AttackBuilding();
                }

                // 공격 전
                Vector3 lookAtPos = ownerEntity.CurAITarget.transform.position;
                ownerEntity.transform.DOLookAt(lookAtPos, 0.5f);
                ownerEntity.curAttackCoolTime = 0;
                ownerEntity.IsAttacking = true;
                //await PlayAnimation(EnemyAnimClips.Atk);
                // 공격 후
                ownerEntity.CurAITargetTag = AITargetTag.None;
                ownerEntity.IsAttacking = false;
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
            Player player = ownerEntity.CurAITarget.GetComponent<Player>();
            player.OnDamaged(ownerEntity.attackPower, ownerEntity.ID);
        }

        private void AttackBuilding()
        {
            Structure structure = ownerEntity.CurAITarget.GetComponent<Structure>();
            structure.OnDamaged(ownerEntity.attackPower, ownerEntity.ID);
        }
    }
    public class Die : State<Enemy>
    {
        private Enemy ownerEntity;
        public override async void Enter(Enemy entity)
        {
            Log.PrintLogMiddleLevel("Enemy Dead Enter");

            ownerEntity = entity;
            ownerEntity.IsDead = true;
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
            if (ownerEntity.IsAttacking)
                return;

            AITarget player = ownerEntity.aiManager.SearchPlayer(ownerEntity.transform);
            if (player != null && ownerEntity.CurAITargetTag != AITargetTag.Player)
            {
                Transform destTransform = player.transform;
                ownerEntity.CurAITarget = player;
                ownerEntity.CurDestTrans = destTransform;
                ownerEntity.Agent.isStopped = false;
                ownerEntity.ChangeState(EnemyStates.RunPlayer);
            }
            else if (player == null && ownerEntity.CurAITargetTag != AITargetTag.Building)
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