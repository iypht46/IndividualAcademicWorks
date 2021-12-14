using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootModule : MonoBehaviour
{
    public enum BulletType { Player = 0, Enemy }

    [SerializeField] protected Transform[] m_ShootPoint = null;
    [SerializeField] LineRenderer[] lineRenderers;

    [SerializeField] protected float m_FireRate = 0.5f;
    [SerializeField] protected float m_EnemyFireRate = 1f;

    [SerializeField] protected float m_BulletSpeed = 20f;
    [SerializeField] protected float m_EnemyBulletSpeed = 20f;

    [SerializeField] protected float m_LineLength = 5.0f;

    [SerializeField] protected float m_BulletSizePlayer = 1.0f;
    [SerializeField] protected float m_BulletSizeEnemy = 1.0f;

    [SerializeField] protected int[] m_LayerNum;

    [SerializeField] ConnectionPointGroup connectionPointGroup;
    [SerializeField] AttachedPointGroup attachedPointGroup;

    [SerializeField] Gradient Gradient_Player;
    [SerializeField] Gradient Gradient_Enemy;


    [SerializeField] string m_PoolName = "BulletPool";
    protected BulletType bulletType;

    protected ObjectPool BulletPool;

    protected float fireRate;
    protected float bulletSpeed;
    protected float bulletSize;

    protected bool m_Obstruct = false;

    private void Awake()
    {
        BulletPool = GameObject.Find(m_PoolName).GetComponent<ObjectPool>();

        fireRate = m_FireRate;
    }

    private void Update()
    {
        UpdateBulletType();
        
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 5.0f, LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer))))
        {
            SetAllLine(false);
            m_Obstruct = true;
        }
        else
        {
            if (gameObject.layer == LayerMask.NameToLayer("Body_Player"))
            {
                SetLaserSight(Gradient_Player);
            }
            else if (gameObject.layer == LayerMask.NameToLayer("Body_Enemy"))
            {
                SetLaserSight(Gradient_Enemy);
            }
            else
            {
                SetAllLine(false);
            }

            m_Obstruct = false;
        }
    }

    public void SetLaserSight(Gradient gradient)
    {
        foreach (LineRenderer line in lineRenderers)
        {

        }

        for(int i = 0; i < m_ShootPoint.Length; i++)
        {
            lineRenderers[i].gameObject.SetActive(true);

            lineRenderers[i].positionCount = 2;

            lineRenderers[i].SetPosition(0, m_ShootPoint[i].position);

            lineRenderers[i].SetPosition(1, m_ShootPoint[i].position + (m_ShootPoint[i].forward * m_LineLength));

            lineRenderers[i].colorGradient = gradient;
        }
    }

    void UpdateBulletType()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Body_Player"))
        {
            bulletType = BulletType.Player;

            fireRate = m_FireRate;

            bulletSpeed = m_BulletSpeed;

            bulletSize = m_BulletSizePlayer;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Body_Enemy"))
        {
            bulletType = BulletType.Enemy;

            fireRate = m_EnemyFireRate;

            bulletSpeed = m_EnemyBulletSpeed;

            bulletSize = m_BulletSizeEnemy;
        }
    }

    void SetAllLine(bool active)
    {
        foreach (LineRenderer line in lineRenderers)
        {
            line.gameObject.SetActive(active);
        }
    }
}
