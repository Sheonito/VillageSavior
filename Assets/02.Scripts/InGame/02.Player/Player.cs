/*
작성자: 최재호(cjh0798@gmail.com)
기능: FSM에 사용되는 Player
 */
using PlayerOwnedStates;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public enum PlayerStates
{
    Idle,
    Run,
    Attack,
    Damaged,
    Die,
    Cast,
    Home,
    Stun,
}

public class Player : BattleEntity
{
    public string NickName { get; set; }

    public Vector3 MoveDir { get; private set; }
    public float moveSpeed;

    public bool IsMine { get; set; }
    public bool IsAttacking { get; set; }


    private PlayerStates curState;
    private State<Player>[] states;
    private BattleStateMachine<Player> stateMachine;

    public CharacterController Controller { get; private set; }
    public Animator Animator { get; private set; }

    // 초기화
    public override void Setup()
    {
        base.Setup();

        gameObject.name = $"{GetType().Name}_{ID}";
        gameObject.SetActive(true);

        // Player State 인스턴트 생성
        states = new State<Player>[5];
        states[(int)PlayerStates.Idle] = new Idle();
        states[(int)PlayerStates.Run] = new Run();
        states[(int)PlayerStates.Attack] = new Attack();
        states[(int)PlayerStates.Damaged] = new Damaged();
        states[(int)PlayerStates.Die] = new Die();

        // Player에 있는 컴포넌트 주입
        Controller = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();

        // StateMachine 인스턴트 생성 및 Setup
        stateMachine = new BattleStateMachine<Player>();
        stateMachine.Setup(this, states[(int)PlayerStates.Idle]);

    }

    private void Update()
    {
        Updated();
    }
    
    // Update에서 매 프레임 마다 호출
    public override void Updated()
    {
        stateMachine.Excute();
    }

    // State 변경
    public void ChangeState(PlayerStates newState)
    {
        curState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }

    // EntityMessage를 통해 외부 클래스에서 데이터 수신
    public override void OnMessage(EntityMessage message)
    {
        stateMachine.OnMessage(message);
    }

    // PlayerInput
    public void OnMovement(CallbackContext value)
    {
        if (IsAttacking)
            return;

        Vector2 inputMovement = value.ReadValue<Vector2>();
        MoveDir = new Vector3(inputMovement.x, 0, inputMovement.y);

        ChangeState(PlayerStates.Run);
    }

    public void OnAttack(CallbackContext value)
    {
        if (!value.canceled || IsAttacking)
            return;


        ChangeState(PlayerStates.Attack);
    }

    public override void OnDamaged(int damage, int senderID)
    {
        EntityMessage message = EntityMessanger.Instance.CreateMessage(damage.ToString(), MessageType.Damaged, ID, senderID);
        ChangeState(PlayerStates.Damaged);
        OnMessage(message);
    }


}
