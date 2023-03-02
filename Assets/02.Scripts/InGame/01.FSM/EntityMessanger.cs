/*
작성자: 최재호(cjh0798@gmail.com)
기능: EntityMessage 생성,전송
 */

using Cysharp.Threading.Tasks;
using UnityEngine;

public class EntityMessanger : Singleton<EntityMessanger>
{
    // 메세지 전송
    public async UniTaskVoid SendMessage(float delayTime, EntityMessage message)
    {
        Entity receiver = EntityDatabase.Instance.GetEntity(message.receiver);

        if (receiver == null)
        {
            Debug.Log("해당 ID를 가진 Receiver는 DataBase에 존재하지 않습니다.");
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

    // 메세지 생성
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
