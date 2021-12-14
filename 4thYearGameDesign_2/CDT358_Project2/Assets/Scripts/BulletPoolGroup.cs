using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolGroup : MonoBehaviour
{
    static BulletPoolGroup _instance;
    public static BulletPoolGroup Instance
    {
        get { return _instance; }
    }

    [SerializeField] ObjectPool[] Pools;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public ObjectPool GetPool(int id)
    {
        ObjectPool tmp = null;

        if (id < Pools.Length)
        {
            tmp = Pools[id];
        }

        return tmp;
    }

}
