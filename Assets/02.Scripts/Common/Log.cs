/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: ������ ���� ������ �α׸� ���
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
