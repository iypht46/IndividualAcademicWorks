using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoming : Bullet
{
    Transform Target = null;

    [SerializeField] float m_RotSpeed = 1.0f;
    [SerializeField] float m_LifeTime = 5.0f;

    float T_LifeTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        T_LifeTime = 0f;
        Target = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
            Target = FindTarget();

        if (Target != null)
        {
            if (Target.gameObject.activeInHierarchy)
                UpdateRotation();

            if (rb != null)
                rb.velocity = transform.forward * m_Speed;

            T_LifeTime += Time.deltaTime;

            if (T_LifeTime >= m_LifeTime)
                gameObject.SetActive(false);
        }
    }

    void UpdateRotation()
    {
        Vector3 targetDirection = Target.position - transform.position;

        float singleStep = m_RotSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    Transform FindTarget()
    {
        if (TargetLayer == LayerMask.NameToLayer("Bullet_Player"))
        {
            Transform trnf = GameObject.FindGameObjectWithTag("Player").transform;

            return trnf;
        }
        
        if (TargetLayer == LayerMask.NameToLayer("Bullet_Enemy"))
        {
            Enemy[] enem = (Enemy[]) GameObject.FindObjectsOfType(typeof(Enemy));

            return GetClosestEnemy(enem);
        }

        return null;
    }

    Transform GetClosestEnemy(Enemy[] enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Enemy enem in enemies)
        {
            float dist = Vector3.Distance(enem.gameObject.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = enem.gameObject.transform;
                minDist = dist;
            }
        }

        return tMin;
    }

}
