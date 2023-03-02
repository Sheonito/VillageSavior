/*
작성자: 최재호(cjh0798@gmail.com)
기능: Log 등의 게임 전체에 영향을 주는 기능 관리
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
