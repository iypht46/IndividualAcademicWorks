using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator m_animator;

    public Transform m_hip; 

    bool isAttacking = false;
    bool isBlocking = false;

    bool isBack = false;

    [SerializeField] bool isMove = true;

    [SerializeField] float m_actionDelay = 2.0f;
    float m_actionT = 0f;

    [SerializeField] float m_BlockDuration = 1.0f;
    float m_BlockT = 0f;

    [SerializeField] float m_BackDistance = 10.0f;

    float m_smoothRot = 0.2f;
    float m_turnSmoothVelocity;

    [SerializeField] Vector2Int ActionRange = new Vector2Int(0, 1);
    [SerializeField] float MoveSpeed = 12f;
    [SerializeField] float DistanceBetweenPlayer = 5f;

    GameObject Player;

    [SerializeField] Collider m_shieldCollider;

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<hpSystem>().isDead)
        {
            Vector3 Direction = (Player.transform.position - transform.position).normalized;

            float targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg - 180;

            float bodyAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_turnSmoothVelocity, m_smoothRot);
            transform.eulerAngles = new Vector3(0f, bodyAngle, 0f);
        }

        isAttacking = m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_1") || m_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackStuck");

        if (!isAttacking && !isBlocking && !GetComponent<hpSystem>().isDead && !isBack)
        {
            m_actionT += Time.deltaTime;

            if (m_actionT > m_actionDelay)
            {
                m_actionT = 0f;

                int RandomAction = Random.Range(ActionRange.x, ActionRange.y + 1);

                switch (RandomAction)
                {
                    case 0: Attack(); /*Debug.Log("Attack")*/ ; break;
                    case 1: Block(); /*Debug.Log("block")*/ ; break;
                    case 2: Back(); /*Debug.Log("Back");*/ break;
                }
            }
        }

        if (isBlocking)
        {
            m_BlockT += Time.deltaTime;

            if (m_BlockT > m_BlockDuration)
            {
                m_BlockT = 0;

                isBlocking = false;
            }

            GetComponent<CombatSystem>().isBlock = isBlocking;
            m_shieldCollider.enabled = isBlocking;
            m_animator.SetBool("isBlocking", isBlocking);
        }

        if (isMove)
        {
            Move();
        }

    }


    void Attack()
    {
        m_animator.SetTrigger("Attack_1");
    }

    void Block()
    {
        m_animator.SetTrigger("Block");
        isBlocking = true;
    }

    void Back()
    {
        isBack = true;
    }

    private void Move()
    {
        if (isAttacking || isBlocking)
        {
            m_animator.SetBool("isRunningF", false);
            m_animator.SetBool("isRunningB", false);
            rigidbody.velocity = Vector3.zero;
        }
        else
        {
            float dist = Vector3.Distance(transform.position, Player.transform.position);

            //Debug.Log(dist);

            if (isBack)
            {
                if (dist < m_BackDistance)
                {
                    rigidbody.velocity = transform.forward * MoveSpeed;
                    m_animator.SetBool("isRunningF", false);
                    m_animator.SetBool("isRunningB", true);
                }
                else
                {
                    isBack = false;
                }
            }
            else
            {
                if (dist > DistanceBetweenPlayer)
                {
                    rigidbody.velocity = transform.forward * -MoveSpeed;
                    m_animator.SetBool("isRunningF", true);
                    m_animator.SetBool("isRunningB", false);
                }
                else if (dist < DistanceBetweenPlayer - 0.5f)
                {
                    rigidbody.velocity = transform.forward * MoveSpeed;
                    m_animator.SetBool("isRunningF", false);
                    m_animator.SetBool("isRunningB", true);
                }
                else
                {
                    m_animator.SetBool("isRunningF", false);
                    m_animator.SetBool("isRunningB", false);
                    rigidbody.velocity = Vector3.zero;
                }
            }
        }
    }
}
