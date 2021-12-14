using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    ObjectPool EnemyPool;
    ObjectPool GunPool;
    ObjectPool WallPool;

    [SerializeField] public int maxEnemy = 10;

    [SerializeField] float spawnRangeX1 = -1;
    [SerializeField] float spawnRangeX2 = 1;
    [SerializeField] float spawnRangeY1 = -1;
    [SerializeField] float spawnRangeY2 = 1;

    // Start is called before the first frame update
    void Start()
    {
        EnemyPool = GameObject.Find("EnemyPool").GetComponent<ObjectPool>();
        GunPool = GameObject.Find("GunPool").GetComponent<ObjectPool>();


        ReSpawnEnemy();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    public bool isAllInactive()
    {
        return EnemyPool.isAllInactive();
    }

    public void ReSpawnEnemy()
    {
        for (int i = 0; i < maxEnemy; i++)
        {
            if (EnemyPool.GetObj() != false)
            {
                GameObject Enem = EnemyPool.GetObj();
                GameObject eGun = GunPool.GetObj();

                eGun.SetActive(true);
                Enem.SetActive(true);

                Enem.transform.position = new Vector3(Random.Range(spawnRangeX1, spawnRangeX2), Random.Range(spawnRangeY1, spawnRangeY2), 0.0f);

                eGun.GetComponent<Gun>().SetOwner(Enem);
                eGun.GetComponent<Gun>().Reload();

                Enem.GetComponent<Enemy>().Gun_curr = eGun;
            }
        }
    }
}
