/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: �ܺ� Ŭ������ ������ Entity���� �����ϴµ� ���Ǵ� Model
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