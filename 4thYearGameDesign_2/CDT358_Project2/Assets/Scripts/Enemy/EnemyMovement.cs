using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float m_MoveSpeed = 20f;
    [SerializeField] float m_StopDist = 5.0f;
    [SerializeField] float m_DetectRange = 5.0f;

    [SerializeField] float m_StrafeTime = 1.0f;
    float strafeT = 0f;

    GameObject m_Player;
    Rigidbody m_Rigidbody;

    float m_smoothRot = 0.2f;
    float m_turnSmoothVelocity;

    [SerializeField] float m_DistanceToPlayer;
    [SerializeField] bool m_canMove = true;
    [SerializeField] bool m_rotateFollowPlayer = true;

    [SerializeField] bool m_rotateSelf = false;
    [SerializeField] float m_rotateSelfSpeed = 1.0f;

    public bool m_CanShoot = false;

    public bool m_CanMove = false;

    bool isStrafing = false;

    Vector3 PlayerDir;

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Rigidbody = GetComponent<Rigidbody>();

        strafeT = m_StrafeTime;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDir = (m_Player.transform.position - this.transform.position).normalized;


        if (m_Player.activeInHierarchy && m_Player != null)
        {
            m_CanShoot = true;

            if (!GetComponent<hpSystem>().isDead && m_CanMove)
            {
                if (m_rotateFollowPlayer)
                {
                    Vector3 Direction = (m_Player.transform.position - transform.position).normalized;

                    float targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;

                    float bodyAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_turnSmoothVelocity, m_smoothRot);
                    transform.eulerAngles = new Vector3(0f, bodyAngle, 0f);
                }
            }

            if (m_rotateSelf)
            {
                float targetAngle = transform.eulerAngles.y + m_rotateSelfSpeed;
                float bodyAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_turnSmoothVelocity, m_smoothRot);
                transform.eulerAngles = new Vector3(0f, bodyAngle, 0f);
            }

            m_DistanceToPlayer = Vector3.Distance(m_Player.transform.position, this.transform.position);

            if (m_DistanceToPlayer > m_DetectRange)
            {
                m_CanShoot = false;
            }

            if (m_CanMove && m_canMove)
            {
                Move();
            }
        }
    }

    private void Move()
    {
        if (m_DistanceToPlayer > m_StopDist && !isStrafing)
        {
            strafeT = m_StrafeTime;
            m_Rigidbody.velocity = PlayerDir * m_MoveSpeed;

        }
        else
        {
            if (!isStrafing)
            {
                isStrafing = true;
                m_Rigidbody.velocity = transform.right * m_MoveSpeed * (Random.Range(0, 2) * 2 - 1);
            }

            strafeT += Time.deltaTime;

            if (strafeT > m_StrafeTime)
            {
                isStrafing = false;
                strafeT = 0f;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, m_DetectRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_StopDist);
    }
}
