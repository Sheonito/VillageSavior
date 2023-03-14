/*
작성자: 최재호(cjh0798@gmail.com)
기능: FSM을 사용하는 Enemy
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


    // 초기화
    public override void Setup()
    {
        base.Setup();

        gameObject.SetActive(true);
        gameObject.name = $"{GetType().Name}_{ID}";

        curAttackCoolTime = attackCoolTime;

        // Enemy 인스펙터에 있는 컴포넌트 주입
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();

        // Enemy State 인스턴트 생성
        states = new State<Enemy>[7];
        states[(int)EnemyStates.Idle] = new Idle();
        states[(int)EnemyStates.RunBuilding] = new RunBuilding();
        states[(int)EnemyStates.RunPlayer] = new RunPlayer();
        states[(int)EnemyStates.Damaged] = new Damaged();
        states[(int)EnemyStates.Attack] = new Attack();
        states[(int)EnemyStates.Die] = new Die();
        states[(int)EnemyStates.SearchTarget] = new SearchTarget();

        // StateMachine 인스턴트 생성 및 Setup
        stateMachine = new BattleStateMachine<Enemy>();
        stateMachine.Setup(this, states[(int)EnemyStates.RunBuilding]);
        stateMachine.SetGlobalState(states[(int)EnemyStates.SearchTarget]);
    }

    private void Update()
    {
        Updated();
    }

    // Update에서 매 프레임 마다 호출
    public override void Updated()
    {
        if (curAttackCoolTime < attackCoolTime)
            curAttackCoolTime += Time.deltaTime;

        stateMachine.Excute();
    }

    // State 변경
    public void ChangeState(EnemyStates newState)
    {
        CurState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }

    // EntityMessage를 통해 외부 클래스에서 데이터 수신
    public override void OnMessage(EntityMessage message)
    {
        stateMachine.OnMessage(message);
    }

    // 공격 받았을 때 호출되는 함수
    public override void OnDamaged(int damage, int senderID)
    {
        EntityMessage message = EntityMessanger.Instance.CreateMessage(damage.ToString(),MessageType.Damaged, ID, senderID);
        ChangeState(EnemyStates.Damaged);
        OnMessage(message);
    }
}
