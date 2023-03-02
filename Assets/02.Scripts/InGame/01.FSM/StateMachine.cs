/*
작성자: 최재호(cjh0798@gmail.com)
기능: FSM의 State를 관리
 */

public class StateMachine<T>
{
    protected T ownerEntity;
    protected State<T> curState;
    protected State<T> preState;
    protected State<T> globalState; // 다른 State와 별개로 작동하는 State

    public virtual void Setup(T entity, State<T> state)
    {
        ownerEntity = entity;
        curState = state;
        preState = null;
        globalState = null;
        curState.Enter(entity);
    }

    // 유니티 Update에서 매 프레임 호출
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

    // EntityMessage를 통해 외부 클래스에서 데이터 수신
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
