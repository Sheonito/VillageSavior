/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: FSM�� ���Ǵ� Player
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

    // �ʱ�ȭ
    public override void Setup()
    {
        base.Setup();

        gameObject.name = $"{GetType().Name}_{ID}";
        gameObject.SetActive(true);

        // Player State �ν���Ʈ ����
        states = new State<Player>[5];
        states[(int)PlayerStates.Idle] = new Idle();
        states[(int)PlayerStates.Run] = new Run();
        states[(int)PlayerStates.Attack] = new Attack();
        states[(int)PlayerStates.Damaged] = new Damaged();
        states[(int)PlayerStates.Die] = new Die();

        // Player�� �ִ� ������Ʈ ����
        Controller = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();

        // StateMachine �ν���Ʈ ���� �� Setup
        stateMachine = new BattleStateMachine<Player>();
        stateMachine.Setup(this, states[(int)PlayerStates.Idle]);

    }

    private void Update()
    {
        Updated();
    }
    
    // Update���� �� ������ ���� ȣ��
    public override void Updated()
    {
        stateMachine.Excute();
    }

    // State ����
    public void ChangeState(PlayerStates newState)
    {
        curState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }

    // EntityMessage�� ���� �ܺ� Ŭ�������� ������ ����
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
