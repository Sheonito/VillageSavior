/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: FSM�� ���Ǵ� �θ� ��ü
 */
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    // Entity�� ���� ID
    private static int nextID;
    private int id;
    public int ID
    {
        set
        {
            id = value;
            nextID++;
        }
        get => id;
    }

    // �ʱ�ȭ
    public virtual void Setup()
    {
        ID = nextID;
        EntityDatabase.Instance.AddEntity(this);
    }

    // ����Ƽ Update���� �� ������ ���� ȣ��
    public abstract void Updated();

    // EntityMessage�� ���� �ܺ� Ŭ�������� ������ ����
    public abstract void OnMessage(EntityMessage entityMessage);
}
