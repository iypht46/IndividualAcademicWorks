using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawn : MonoBehaviour
{

    [SerializeField] Gun CurrGun = null;
    [SerializeField] bool EmptyGun;
    [SerializeField] float SpawnDelay;

    ObjectPool GunPool;

    float T;

    // Start is called before the first frame update
    void Start()
    {
        T = 0;
        GunPool = GameObject.Find("GunPool").GetComponent<ObjectPool>();
        
        if(CurrGun == null)
        {
            SpawnGun();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (EmptyGun)
        {
            CurrGun.bullet_amount = 0;
        }
        else
        {
            CurrGun.Reload();
        }

        if (CurrGun.Owner != null || !CurrGun.gameObject.activeInHierarchy)
        {
            T += Time.deltaTime;

            if (T >= SpawnDelay)
            {
                T = 0;
                SpawnGun();
            }
        }
    }

    void SpawnGun()
    {
        CurrGun = GunPool.GetObj().GetComponent<Gun>();
        CurrGun.gameObject.transform.position = transform.position;
        CurrGun.gameObject.SetActive(true);

        if (EmptyGun)
        {
            CurrGun.bullet_amount = 0;
        }
        else
        {
            CurrGun.Reload();
        }
    }
}
