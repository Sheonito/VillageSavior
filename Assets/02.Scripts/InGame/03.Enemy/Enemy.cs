using EnemyOwnedStates;
using System.Collections;
using System.Collections.Generic;
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

    public NavMeshAgent Agent;
    public Animator Animator { get; set; }
    public AITarget CurAITarget { get; set; }
    public Transform CurDestTrans { get; set; }
    public AITargetTag CurAITargetTag { get; set; }

    public float attackCoolTime;
    public float curAttackCoolTime;
    public bool IsAttacking { get; set; }

    public EnemyStates CurState { get; private set; }
    private State<Enemy>[] states;
    private BattleStateMachine<Enemy> stateMachine;

    [HideInInspector]
    public enemyAIManager aiManager;

    public override void Setup()
    {
        base.Setup();

        gameObject.SetActive(true);
        gameObject.name = $"{GetType().Name}_{ID}";

        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();

        curAttackCoolTime = attackCoolTime;

        states = new State<Enemy>[7];
        states[(int)EnemyStates.Idle] = new Idle();
        states[(int)EnemyStates.RunBuilding] = new RunBuilding();
        states[(int)EnemyStates.RunPlayer] = new RunPlayer();
        states[(int)EnemyStates.Damaged] = new Damaged();
        states[(int)EnemyStates.Attack] = new Attack();
        states[(int)EnemyStates.Die] = new Die();
        states[(int)EnemyStates.SearchTarget] = new SearchTarget();

        stateMachine = new BattleStateMachine<Enemy>();
        stateMachine.Setup(this, states[(int)EnemyStates.RunBuilding]);
        stateMachine.SetGlobalState(states[(int)EnemyStates.SearchTarget]);
    }

    public override void Updated()
    {
        if (curAttackCoolTime < attackCoolTime)
            curAttackCoolTime += Time.deltaTime;

        stateMachine.Excute();
    }

    public void ChangeState(EnemyStates newState)
    {
        CurState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }

    public override void OnMessage(EntityMessage message)
    {
        stateMachine.OnMessage(message);
    }

    public override void OnDamaged(int damage, int senderID)
    {
        EntityMessage message = EntityMessanger.Instance.CreateMessage(damage.ToString(),MessageType.Damaged, ID, senderID);
        ChangeState(EnemyStates.Damaged);
        OnMessage(message);
    }
}
