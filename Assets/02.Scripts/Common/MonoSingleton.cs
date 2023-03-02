/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: MonoBehaviour ���� �̱���
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

                // ���� ���� ���� T Ŭ������ ���� ��
                if (objs.Length > 1)
                {
                    for (int i = 1; i < objs.Length; i++)
                    {
                        Destroy(objs[i].gameObject);
                    }
                    instance = objs[0];
                }
                // ���� T Ŭ������ �ϳ��� ��
                else if (objs.Length == 1)
                {
                    instance = objs[0];
                }
                // ���� T Ŭ������ ���� ��
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
