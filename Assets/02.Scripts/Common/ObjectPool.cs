/*
�ۼ���: ����ȣ(cjh0798@gmail.com)
���: ������Ʈ Ǯ��
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
        [Header("Tag�� Pool���� Get�ϱ� ���� Key��")]
        public ObjectPoolTag tag;
        public GameObject prefab;
        public Transform objRoot;
        public int size;
    }
    #endregion

    [Header("������ ������Ʈ")]
    public List<PoolData> objectList;
    public Dictionary<ObjectPoolTag, Queue<GameObject>> Pool { get; private set; }

    private void Awake()
    {
        SetUp();
    }

    // ���� �� ������Ʈ ����
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

    // ������Ʈ ��������
    public GameObject GetObject(ObjectPoolTag poolTag)
    {
        if (Pool.ContainsKey(poolTag) == false)
        {
            Log.PrintLogLowLevel($"{poolTag} Tag�� ���� ������Ʈ�� Pool�� �����ϴ�.");
            return null;
        }

        else if (Pool[poolTag].Count == 0)
        {
            Debug.Log("Pool�� ������Ʈ�� �����մϴ�.");
            return null;
        }

        GameObject obj = Pool[poolTag].Dequeue();
        return obj;
    }


    // ������ ������Ʈ �ٽ� Pool�� �ֱ�
    public void BackToPool(ObjectPoolTag poolTag, GameObject obj)
    {
        if (Application.isPlaying == false)
            return;

        if (Pool.ContainsKey(poolTag) == false)
        {
            Log.PrintLogLowLevel($"{poolTag} Tag�� ���� ������Ʈ�� Pool�� �����ϴ�.");
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
