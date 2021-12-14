using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] float m_BoundX;
    [SerializeField] float m_BoundZ;
    [SerializeField] float m_OffsetY;

    [SerializeField] ObjectPool m_pool;

    [SerializeField] float m_StartAmount = 20;

    void Start()
    {
        for(int i = 0; i < m_StartAmount; i++)
        {
            Vector3 randPos = new Vector3(Random.Range(-m_BoundX, m_BoundX), m_OffsetY, Random.Range(-m_BoundZ, m_BoundZ));
            SpawnAt(randPos, new Quaternion(Quaternion.identity.x, Random.rotation.y, Quaternion.identity.z, Quaternion.identity.w));
        }
    }

    void Update()
    {
        
    }

    void SpawnAt(Vector3 pos, Quaternion quaternion)
    {
        GameObject obj = m_pool.GetObj();

        if (obj != null)
        {
            obj.transform.position = pos;
            obj.transform.rotation = quaternion;
            obj.SetActive(true);

            if (obj.TryGetComponent(out Module module))
            {
                module.isDepleted = false;
            }
        }
    }
}
