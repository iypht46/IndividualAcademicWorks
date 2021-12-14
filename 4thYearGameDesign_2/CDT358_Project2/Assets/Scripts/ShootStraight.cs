using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootStraight : ShootModule
{
    [Header("BulletPattern")]
    [SerializeField] int m_BulletPerInterval = 5;
    [SerializeField] float m_BulletInterval = 0.5f;

    int m_BulletFired = 0;
    float T_Interval, T_FireRate;

    public void Shoot()
    {
        T_Interval += Time.deltaTime;

        if (T_Interval >= m_BulletInterval && (!m_Obstruct))
        {
            T_FireRate += Time.deltaTime;

            if (T_FireRate >= fireRate)
            {
                ShootBullet();

                m_BulletFired += 1;
                T_FireRate = 0;
            }

            if (m_BulletFired == m_BulletPerInterval)
            {
                m_BulletFired = 0;
                T_Interval = 0;
            }
        }
    }

    void ShootBullet()
    {
        foreach(Transform shootPoint in m_ShootPoint)
        {
            GameObject bullet = BulletPool.GetObj();

            bullet.layer = m_LayerNum[(int)bulletType];

            if (bullet.TryGetComponent(out Bullet b))
            {
                b.SetProperties();
                b.SetDisplayObjectSize(Vector3.one * bulletSize);

                b.m_Speed = bulletSpeed;
            }

            Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
            rigidbody.velocity = shootPoint.forward * bulletSpeed;

            bullet.transform.position = shootPoint.position;
            bullet.transform.rotation = shootPoint.rotation;

            bullet.SetActive(true);
        }

        gameObject.GetComponent<Sound>().PlayRandom();
    }
}
