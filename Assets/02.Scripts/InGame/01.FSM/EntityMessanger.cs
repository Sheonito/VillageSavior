/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: EntityMessage ����,����
 */

using Cysharp.Threading.Tasks;
using UnityEngine;

public class EntityMessanger : Singleton<EntityMessanger>
{
    // �޼��� ����
    public async UniTaskVoid SendMessage(float delayTime, EntityMessage message)
    {
        Entity receiver = EntityDatabase.Instance.GetEntity(message.receiver);

        if (receiver == null)
        {
            Debug.Log("�ش� ID�� ���� Receiver�� DataBase�� �������� �ʽ��ϴ�.");
            return;
        }

        if (delayTime <= 0)
        {
            receiver.OnMessage(message);
        }
        else
        {
            await UniTask.Delay((int)delayTime * 1000);
            message.sendTime = Time.time + delayTime;
            receiver.OnMessage(message);
        }
    }

    // �޼��� ����
    public EntityMessage CreateMessage(string message, MessageType messageType, int receiverID, int senderID)
    {
        EntityMessage entityMessage = new EntityMessage()
        {
            receiver = receiverID,
            sender = senderID,
            sendTime = Time.time,
            message = message,
            type = messageType,
        };

        return entityMessage;
    }
}
