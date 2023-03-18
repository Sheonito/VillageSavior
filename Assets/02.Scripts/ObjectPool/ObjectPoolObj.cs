/*
작성자: 최재호(cjh0798@gmail.com)
기능: 오브젝트 풀에서 생성된 오브젝트, OnDisable 시 ObejctPool로 돌아감
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
    
    // ObjectPool로 돌아감
    private void OnDisable()
    {
        backToPool.Invoke();
    }
}
