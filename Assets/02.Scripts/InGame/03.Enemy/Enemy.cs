/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: FSM�� ����ϴ� Enemy
 */
using EnemyOwnedStates;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyStates
{
    Idle,
    RunBuilding,
    RunPlayer,
    Damaged,
    Attack,
    Die,
    SearchTarget,
    Cast,
    Stun,
}

public class Enemy : BattleEntity
{
    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    [HideInInspector] public EnemyAIManager aiManager;
    [HideInInspector] public AITarget curAITarget;
    [HideInInspector] public Transform curDestTrans;
    [HideInInspector] public AITargetTag curAITargetTag;

    public float attackCoolTime;
    public float curAttackCoolTime;
    [HideInInspector] public bool isAttacking;

    public EnemyStates CurState { get; private set; }
    private State<Enemy>[] states;
    private BattleStateMachine<Enemy> stateMachine;


    // �ʱ�ȭ
    public override void Setup()
    {
        base.Setup();

        gameObject.SetActive(true);
        gameObject.name = $"{GetType().Name}_{ID}";

        curAttackCoolTime = attackCoolTime;

        // Enemy �ν����Ϳ� �ִ� ������Ʈ ����
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();

        // Enemy State �ν���Ʈ ����
        states = new State<Enemy>[7];
        states[(int)EnemyStates.Idle] = new Idle();
        states[(int)EnemyStates.RunBuilding] = new RunBuilding();
        states[(int)EnemyStates.RunPlayer] = new RunPlayer();
        states[(int)EnemyStates.Damaged] = new Damaged();
        states[(int)EnemyStates.Attack] = new Attack();
        states[(int)EnemyStates.Die] = new Die();
        states[(int)EnemyStates.SearchTarget] = new SearchTarget();

        // StateMachine �ν���Ʈ ���� �� Setup
        stateMachine = new BattleStateMachine<Enemy>();
        stateMachine.Setup(this, states[(int)EnemyStates.RunBuilding]);
        stateMachine.SetGlobalState(states[(int)EnemyStates.SearchTarget]);
    }

    private void Update()
    {
        Updated();
    }

    // Update���� �� ������ ���� ȣ��
    public override void Updated()
    {
        if (curAttackCoolTime < attackCoolTime)
            curAttackCoolTime += Time.deltaTime;

        stateMachine.Excute();
    }

    // State ����
    public void ChangeState(EnemyStates newState)
    {
        CurState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }

    // EntityMessage�� ���� �ܺ� Ŭ�������� ������ ����
    public override void OnMessage(EntityMessage message)
    {
        stateMachine.OnMessage(message);
    }

    // ���� �޾��� �� ȣ��Ǵ� �Լ�
    public override void OnDamaged(int damage, int senderID)
    {
        EntityMessage message = EntityMessanger.Instance.CreateMessage(damage.ToString(),MessageType.Damaged, ID, senderID);
        ChangeState(EnemyStates.Damaged);
        OnMessage(message);
    }
}
