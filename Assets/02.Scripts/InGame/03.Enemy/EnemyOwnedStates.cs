/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: Enemy�� ��� State
 */
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace EnemyOwnedStates
{
    // ������ �ִ� ����
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

    // Building���� �޷����� �ִ� ���� 
    public class RunBuilding : State<Enemy>
    {
        private Enemy ownerEntity;
        public override void Enter(Enemy entity)
        {
            Log.PrintLogMiddleLevel("Enemy RunBuilding Enter");

            ownerEntity = entity;

            // Enemy AITarget �ҷ�����
            AITarget aiTarget = ownerEntity.aiManager.GetTarget(ownerEntity.transform);

            if (aiTarget == null)
            {
                ownerEntity.ChangeState(EnemyStates.Idle);
                return;
            }

            // Enemy ������ ����
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

            // Agent SetDestination �� �Ͻ������� remainingDistance�� 0�� �Ǿ� �����ϴ� ���� ����ó��
            if (ownerEntity.Agent.remainingDistance == 0 || ownerEntity == null) 
                return;

            // ���ݹ��� �ȿ� �ְ� ���� ��Ÿ���� ���� ����
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

        // Enemy ������ ����
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

    // Player���� �޷����� �ִ� ����
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

            // ���ݹ��� �ȿ� �ְ� ���� ��Ÿ���� ���� ����
            if (isAttackRange && isCoolTime)
            {
                ownerEntity.ChangeState(EnemyStates.Attack);
            }
            else
            {
                // Enemy ������ ����
                SetDestination(ownerEntity);
            }
        }

        public override void Exit()
        {

        }

        public override void OnMessage(EntityMessage message)
        {

        }

        // Enemy ������ ����
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

    // �������� ���� ����
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
            // �޼����� ���� Hitbox�κ��� ������ ����
            if (string.IsNullOrEmpty(entityMessage.message) == false && entityMessage.type == MessageType.Damaged)
            {
                damage = int.Parse(entityMessage.message);
                Entity targetEntity = EntityDatabase.Instance.GetEntity(entityMessage.sender);
                ownerEntity.curDestTrans = targetEntity.transform;

                OnDamaged().Forget();
            }
        }

        // ������ ���� ó��
        private async UniTaskVoid OnDamaged()
        {
            if (damage == 0)
            {
                Log.PrintLogLowLevel("�������� �������� ���߽��ϴ�.");
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

    // �������� ����
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

            // ��Ÿ���� á�� ��
            if (isCoolTime)
            {
                // ���� Ÿ���� Player�� ��
                if (ownerEntity.curAITargetTag == AITargetTag.Player)
                {
                    AttackPlayer();
                }
                // ���� Ÿ���� Building�� ��
                else if (ownerEntity.curAITargetTag == AITargetTag.Building)
                {
                    AttackBuilding();
                }
                Vector3 lookAtPos = ownerEntity.curAITarget.transform.position;
                ownerEntity.transform.DOLookAt(lookAtPos, 0.5f);
                
                // ���� �� ��Ÿ�� �� Ÿ�� �ʱ�ȭ
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

    // ���� ����
    public class Die : State<Enemy>
    {
        private Enemy ownerEntity;
        public override async void Enter(Enemy entity)
        {
            Log.PrintLogMiddleLevel("Enemy Dead Enter");

            ownerEntity = entity;
            ownerEntity.IsDead = true;

            // �ִϸ��̼� ����� ������ SetActive(false)�� ���� ObjectPool�� ����������
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

    // ���� Ÿ�� �˻� ����
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
            // Enemy�� ���� ������� ���� return
            if (ownerEntity.isAttacking)
                return;

            // ���� ���� �� Player�� �ִ� �� �˻�
            AITarget player = ownerEntity.aiManager.SearchPlayer(ownerEntity.transform);

            // Player�� �˻��Ǹ� Player�� ���� Run
            if (player != null && ownerEntity.curAITargetTag != AITargetTag.Player)
            {
                Transform destTransform = player.transform;
                ownerEntity.curAITarget = player;
                ownerEntity.curDestTrans = destTransform;
                ownerEntity.Agent.isStopped = false;
                ownerEntity.ChangeState(EnemyStates.RunPlayer);
            }
            // Player�� �˻����� ������ Building�� ���� Run
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