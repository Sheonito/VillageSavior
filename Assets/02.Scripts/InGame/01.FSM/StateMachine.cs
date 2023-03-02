/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: FSM�� State�� ����
 */

public class StateMachine<T>
{
    protected T ownerEntity;
    protected State<T> curState;
    protected State<T> preState;
    protected State<T> globalState; // �ٸ� State�� ������ �۵��ϴ� State

    public virtual void Setup(T entity, State<T> state)
    {
        ownerEntity = entity;
        curState = state;
        preState = null;
        globalState = null;
        curState.Enter(entity);
    }

    // ����Ƽ Update���� �� ������ ȣ��
    public virtual void Excute()
    {
        if (globalState != null)
        {
            globalState.Excute();
        }

        if (curState != null)
        {
            curState.Excute();
        }
    }

    public virtual void ChangeState(State<T> newState)
    {
        if (curState != null)
        {
            preState = curState;
            curState.Exit();
        }

        curState = newState;
        curState.Enter(ownerEntity);
    }

    public virtual void SetGlobalState(State<T> newState)
    {
        globalState = newState;
        globalState.Enter(ownerEntity);
    }

    public virtual void BackToPreState()
    {
        ChangeState(preState);
    }

    // EntityMessage�� ���� �ܺ� Ŭ�������� ������ ����
    public virtual void OnMessage(EntityMessage message)
    {

        if (globalState != null)
        {
            globalState.OnMessage(message);
        }
        if (curState != null)
        {
            curState.OnMessage(message);
        }
        else
        {
            return;
        }
    }
}
