/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: ������Ʈ Ǯ���� ������ ������Ʈ, OnDisable �� ObejctPool�� ���ư�
 */
using System;
using UnityEngine;

public class ObjectPoolObj : MonoBehaviour
{
    public ObjectPoolTag PoolTag { get; private set; }
    public Action backToPool;

    public void Setup(ObjectPoolTag poolTag,Action callback)
    {
        PoolTag = poolTag;
        backToPool = callback;
    }
    
    // ObjectPool�� ���ư�
    private void OnDisable()
    {
        backToPool.Invoke();
    }
}
