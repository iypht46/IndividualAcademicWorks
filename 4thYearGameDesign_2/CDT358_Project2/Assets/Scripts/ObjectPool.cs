using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] protected GameObject ObjToPool;
    [Tooltip("If left null, this object becomes parent")]
    [SerializeField] protected GameObject ObjParent;
    protected GameObject objParent
    {
        get
        {
            if (ObjParent == null)
                return this.gameObject;

            return ObjParent;
        }
    }

    [SerializeField] protected int amount;
    public int PoolSize
    {
        get { return amount; }
    }

    public int ActiveObjCount
    {
        get
        {
            int count = 0;
            foreach (GameObject obj in pooledObjs)
            {
                if (obj.activeInHierarchy)
                {
                    ++count;
                }
            }
            return count;
        }
    }

    Queue<GameObject> pooledObjs;

    protected virtual void Awake()
    {
        pooledObjs = new Queue<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amount; i++)
        {
            tmp = Instantiate(ObjToPool);
            tmp.SetActive(false);
            pooledObjs.Enqueue(tmp);

            tmp.transform.parent = objParent.transform;
        }
    }

    public GameObject GetObj()
    {
        int qSize = pooledObjs.Count;

        for (int i = 0; i < qSize; i++)
        {
            GameObject tmp = pooledObjs.Dequeue();
            pooledObjs.Enqueue(tmp);

            if (!tmp.activeInHierarchy)
            {
                return tmp;
            }

        }

        return null;
    }

    public GameObject GetFirstObjInQ()
    {
        GameObject tmp = pooledObjs.Dequeue();
        pooledObjs.Enqueue(tmp);

        return tmp;
    }

    public bool isAllInactive()
    {
        foreach(GameObject obj in pooledObjs)
        {
            if (obj.activeInHierarchy)
            {
                return false;
            }
        }

        return true;
    }

    public void SetActiveAll(bool active)
    {
        foreach (GameObject obj in pooledObjs)
        {
            if (obj.activeInHierarchy)
            {
                obj.SetActive(false);
            }
        }
    }

    public Queue<GameObject> GetPool()
    {
        return pooledObjs;
    }
}
