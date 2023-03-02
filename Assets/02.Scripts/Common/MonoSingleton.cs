/*
작성자: 최재호(cjh0798@gmail.com)
기능: MonoBehaviour 전용 싱글톤
 */
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance 
    {
        get
        {
            if (instance == null)
            {
                T[] objs = FindObjectsOfType<T>();

                // 씬에 여러 개의 T 클래스가 있을 때
                if (objs.Length > 1)
                {
                    for (int i = 1; i < objs.Length; i++)
                    {
                        Destroy(objs[i].gameObject);
                    }
                    instance = objs[0];
                }
                // 씬에 T 클래스가 하나일 때
                else if (objs.Length == 1)
                {
                    instance = objs[0];
                }
                // 씬에 T 클래스가 없을 때
                else
                {
                    T obj = new GameObject().AddComponent<T>();
                    obj.name = obj.GetType().ToString();
                    instance = obj;
                }
                return instance;
            }
            else
            {
                return instance;
            }
        }
    }
}
