/*
작성자: 최재호(cjh0798@gmail.com)
기능: FSM의 State
 */

public abstract class State<T>
{
    // 시작할 때 1회 호출
    public abstract void Enter(T entity);

    // Update에서 매 프레임 호출
    public abstract void Excute();

    // 종료할 때 1회 호출
    public abstract void Exit();

    // 외부 클래스에서 데이터 수신 시 1회 호출
    public abstract void OnMessage(EntityMessage entityMessage);
}
