using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyMovement m_movement;

    [HideInInspector] public Vector3 m_AssignedPos;

    [SerializeField] bool m_firstAttach = false;

    GameObject m_Player;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Waypoint>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_firstAttach)
        {
            //m_firstAttach = true;
            BroadcastMessage("Attach");
        }

        if (m_movement.m_CanShoot)
        {
            BroadcastMessage("Shoot", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void DetachAll()
    {
        BroadcastMessage("Detach");
    }

    public void MoveToAssignPos()
    {
        gameObject.GetComponent<Waypoint>().enabled = true;

        transform.position = EnemySpawner.Instance.GetPos();
        m_movement.m_CanMove = true;
        m_firstAttach = true;
    }
}
