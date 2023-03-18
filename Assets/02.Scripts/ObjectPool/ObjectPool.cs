/*
작성자: 최재호(cjh0798@gmail.com)
기능: 오브젝트 풀링
 */
using System.Collections.Generic;
using UnityEngine;

public enum ObjectPoolTag
{
    warrior,
    tinyDragon,
    warriorEffect_01
}

public class ObjectPool : MonoSingleton<ObjectPool>
{
    #region PoolObject Class
    [System.Serializable]
    public class PoolData
    {
        [Header("Tag는 Pool에서 Get하기 위한 Key값")]
        public ObjectPoolTag tag;
        public GameObject prefab;
        public Transform objRoot;
        public int size;
    }
    #endregion

    [Header("생성할 오브젝트")]
    public List<PoolData> objectList;
    public Dictionary<ObjectPoolTag, Queue<GameObject>> Pool { get; private set; }

    private void Awake()
    {
        SetUp();
    }

    // 시작 시 오브젝트 생성
    public void SetUp()
    {
        Pool = new Dictionary<ObjectPoolTag, Queue<GameObject>>();

        for (int i = 0; i < objectList.Count; i++)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int j = 0; j < objectList[i].size; j++)
            {
                GameObject obj = Instantiate(objectList[i].prefab, objectList[i].objRoot);
                obj.AddComponent<ObjectPoolObj>();
                ObjectPoolObj poolObj = obj.GetComponent<ObjectPoolObj>();
                poolObj.Setup(objectList[i].tag, () => BackToPool(poolObj.PoolTag, obj));
                queue.Enqueue(obj);
            }
            Pool.Add(objectList[i].tag, queue);
        }
    }

    // 오브젝트 가져오기
    public GameObject GetObject(ObjectPoolTag poolTag)
    {
        if (Pool.ContainsKey(poolTag) == false)
        {
            Log.PrintLogLowLevel($"{poolTag} Tag를 가진 오브젝트가 Pool에 없습니다.");
            return null;
        }

        else if (Pool[poolTag].Count == 0)
        {
            Debug.Log("Pool에 오브젝트가 부족합니다.");
            return null;
        }

        GameObject obj = Pool[poolTag].Dequeue();
        return obj;
    }


    // 가져온 오브젝트 다시 Pool에 넣기
    public void BackToPool(ObjectPoolTag poolTag, GameObject obj)
    {
        if (Application.isPlaying == false)
            return;

        if (Pool.ContainsKey(poolTag) == false)
        {
            Log.PrintLogLowLevel($"{poolTag} Tag를 가진 오브젝트가 Pool에 없습니다.");
            return;
        }

        else if (obj.activeSelf)
        {
            obj.SetActive(false);
            return;
        }

        Pool[poolTag].Enqueue(obj);
        obj.transform.SetParent(null);
    }
}
