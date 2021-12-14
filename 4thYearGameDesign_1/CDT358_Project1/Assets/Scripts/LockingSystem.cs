using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockingSystem : MonoBehaviour
{
    [SerializeField] TPSMovement m_movement;
    [SerializeField] GameObject m_TargetLockIcon;

    Queue<GameObject> m_VisibleEnemies = new Queue<GameObject>();

    public GameObject m_LockingObj = null;

    public void OnLock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (m_LockingObj == null)
            {
                CycleTarget();
            }
            else
            {
                UnlockTarget();
            }
        }
    }

    public void OnCycleLock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CycleTarget();
        }

    }

    public void CycleTarget()
    {
        if(m_VisibleEnemies.Count > 0)
        {
            GameObject tmp = m_VisibleEnemies.Dequeue();

            m_LockingObj = tmp;

            if (tmp.activeInHierarchy)
            {
                m_VisibleEnemies.Enqueue(tmp);
            }

            m_TargetLockIcon.transform.position = m_LockingObj.GetComponent<Enemy>().m_hip.position;
            m_TargetLockIcon.SetActive(true);

            m_movement.m_LookAtObj = m_LockingObj;

            m_movement.m_lock = false;
        }
    }

    public void UnlockTarget()
    {
        m_TargetLockIcon.SetActive(false);
        m_movement.m_LookAtObj = null;
        m_LockingObj = null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisibleEnemyList();

        if (m_LockingObj != null)
        {
            m_TargetLockIcon.transform.position = m_LockingObj.transform.position;

            if (m_LockingObj.TryGetComponent(out hpSystem HP))
            {
                if (HP.isDead)
                {
                    if (m_VisibleEnemies.Count > 0)
                    {
                        CycleTarget();
                    }
                    else
                    {
                        UnlockTarget();
                    }
                }
            }
        }
    }

    void UpdateVisibleEnemyList()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enem in enemies)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(enem.transform.position);

            bool behind = Vector3.Dot((enem.transform.position - Camera.main.transform.position), Camera.main.transform.forward) < 0;
            bool onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;

            if (enem.GetComponentInChildren<Renderer>().isVisible && onScreen && !behind && enem.activeInHierarchy && !enem.GetComponentInChildren<hpSystem>().isDead)
            {
                if (!m_VisibleEnemies.Contains(enem))
                {
                    m_VisibleEnemies.Enqueue(enem);
                }
            }
            else
            {
                if (m_VisibleEnemies.Contains(enem))
                {
                    RemoveFromQueue(enem);
                }
            }
        }

    }

    void RemoveFromQueue(GameObject removeObj)
    {
        Queue<GameObject> tmpQ = new Queue<GameObject>();

        foreach (GameObject obj in m_VisibleEnemies)
        {
            if (obj != removeObj)
            {
                tmpQ.Enqueue(obj);
            }
        }

        m_VisibleEnemies = tmpQ;
    }
}
