using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Queue<GameObject> pooledObjs;
    public GameObject ObjToPool;
    public int amount;
    
    void Awake()
    {
        pooledObjs = new Queue<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amount; i++)
        {
            tmp = Instantiate(ObjToPool);
            tmp.SetActive(false);
            pooledObjs.Enqueue(tmp);
        }
    }

    public GameObject GetObj()
    {
        int qSize = pooledObjs.Count;

        for(int i = 0; i < qSize; i++)
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
}
