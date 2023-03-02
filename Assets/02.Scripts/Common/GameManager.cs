/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: Log ���� ���� ��ü�� ������ �ִ� ��� ����
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public LogLevel logLevel;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        EntityDatabase.Instance.Setup();
    }

}
