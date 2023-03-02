/*
작성자: 최재호(cjh0798@gmail.com)
기능: 외부 클래스의 정보를 Entity에게 전달하는데 사용되는 Model
 */

public struct EntityMessage
{
    public float sendTime;
    public int sender;
    public int receiver;
    public string message;
    public MessageType type;
}

public enum MessageType
{ 
    Damage,
    Damaged,
    DestroyedStructure,
    StageLevelChange
}