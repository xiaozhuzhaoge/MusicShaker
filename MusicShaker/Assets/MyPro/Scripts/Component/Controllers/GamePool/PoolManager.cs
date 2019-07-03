using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    private static PoolManager instance;
    public static PoolManager Instance
    {
        get {

            if (instance == null)
            {
                instance = new GameObject().AddComponent<PoolManager>();
            }

            return instance;
        }
    }

    private Dictionary<string, PoolQueue> cachePool;
    private Dictionary<string, PoolQueue> dePool;


    public List<string> prefabNameList;
    public List<int> limits;

    void Awake()
    {
        instance = this;
        cachePool = new Dictionary<string, PoolQueue>();
        dePool = new Dictionary<string, PoolQueue>();
    }

    void InitPoolManager()
    {
        for(int i = 0; i < prefabNameList.Count; i++)
        {
            cachePool.Add(prefabNameList[i], new PoolQueue(limits[prefabNameList.IndexOf(prefabNameList[i])]));
            dePool.Add(prefabNameList[i], new PoolQueue(limits[prefabNameList.IndexOf(prefabNameList[i])]));
        }
    }

    public void AddToCahcePool(string type,string prefabName)
    {
        if(prefabNameList == null || limits == null)
        {
            Debug.LogError("请确保缓存注册有数据！");
            return;
        }

        if(prefabNameList.Count != limits.Count)
        {
            Debug.LogError("请确保缓存对象与缓存池大小相等");
            return;
        }

        if(!cachePool.ContainsKey(prefabName))
        {
            cachePool.Add(prefabName, new PoolQueue(limits[prefabNameList.IndexOf(prefabName)]));
            dePool.Add(prefabName, new PoolQueue(limits[prefabNameList.IndexOf(prefabName)]));
        }
   

        cachePool[prefabName].EnQueue(ResourceMgr.LoadResource(type, prefabName));
    }

    public GameObject GetFromCache(string type,string prefabName)
    {
        if (!cachePool.ContainsKey(prefabName))
        {
            AddToCahcePool(type, prefabName);
            Debug.Log("初始化数据");
        }
        else if(cachePool[prefabName] != null)
        {
            if (!cachePool[prefabName].IsExLimit())
            {
                AddToCahcePool(type, prefabName);
            }
        }

        GameObject dego = cachePool[prefabName].DeQueue();
        dePool[prefabName].EnQueue(dego);

        if (dePool[prefabName].IsExLimit())
        {
            dego = dePool[prefabName].DeQueue();
            cachePool[prefabName].EnQueue(dego);
        }
        
        return dego;
    }
}

public class PoolQueue 
{
    public int LimitCount;
    public Queue<GameObject> caches;
    public int Count
    {
        get { return caches.Count; }
    }

    public PoolQueue(int limit)
    {
        LimitCount = limit;
        caches = new Queue<GameObject>();
    }

    public void EnQueue(GameObject gameobj)
    {
        if(!IsExLimit())
            caches.Enqueue(gameobj);
    }

    public GameObject DeQueue()
    {
        return caches.Dequeue();
    }

    public bool IsExLimit()
    {
        return caches.Count > LimitCount;
    }
}
