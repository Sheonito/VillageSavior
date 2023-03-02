/*
작성자: 최재호(cjh0798@gmail.com)
기능: 레벨에 따라 게임의 로그를 출력
 */
using UnityEngine;

public enum LogLevel
{
    Low,
    Middle
}

public static class Log
{
    public static void PrintLogLowLevel(string log)
    {
        Debug.Log(log);
    }

    public static void PrintLogMiddleLevel(string log)
    {
        if (GameManager.Instance.logLevel == LogLevel.Middle)
            Debug.Log(log);
    }
}
