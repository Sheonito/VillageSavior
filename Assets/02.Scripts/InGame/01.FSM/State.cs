/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: FSM�� State
 */

public abstract class State<T>
{
    // ������ �� 1ȸ ȣ��
    public abstract void Enter(T entity);

    // Update���� �� ������ ȣ��
    public abstract void Excute();

    // ������ �� 1ȸ ȣ��
    public abstract void Exit();

    // �ܺ� Ŭ�������� ������ ���� �� 1ȸ ȣ��
    public abstract void OnMessage(EntityMessage entityMessage);
}
