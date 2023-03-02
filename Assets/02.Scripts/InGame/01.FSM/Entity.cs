/*
작성자: 최재호(cjh0798@gmail.com)
기능: FSM에 사용되는 부모 객체
 */
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    // Entity의 고유 ID
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

    // 초기화
    public virtual void Setup()
    {
        ID = nextID;
        EntityDatabase.Instance.AddEntity(this);
    }

    // 유니티 Update에서 매 프레임 마다 호출
    public abstract void Updated();

    // EntityMessage를 통해 외부 클래스에서 데이터 수신
    public abstract void OnMessage(EntityMessage entityMessage);
}
