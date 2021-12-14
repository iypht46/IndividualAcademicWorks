using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSMovement : MonoBehaviour
{
    [HideInInspector] public Vector2 m_input_MoveDir;
    [HideInInspector] public Vector2 m_input_LookDir;

    [HideInInspector] public Vector3 m_nextPos;
    [HideInInspector] public Quaternion m_nextRot;

    public float m_rotPower = 3f;
    public float m_rotLerp = 0.5f;

    public float m_lockSpeed = 5f;
    public bool m_lock = false;

    public float m_smoothVerocity = 0.2f; 
    Vector3 m_targetVel;
    Vector3 m_refVel = Vector3.zero;

    public float m_smoothRot = 0.2f;
    float m_turnSmoothVelocity;

    public float m_moveSpeed = 1.0f;
    public GameObject m_followTransform;

    [HideInInspector] public Rigidbody m_rb;

    [HideInInspector] public Vector3 m_FinalMoveDir;

    [SerializeField] Transform m_Body;

    [SerializeField] public GameObject m_LookAtObj;

    [SerializeField] GameController gameController;

    Animator m_animator;

    public bool m_isAttacking;
    public bool m_isBlocking;

    public void OnMove(InputAction.CallbackContext context)
    {
        m_input_MoveDir = context.ReadValue<Vector2>();
    }

    public void Test()
    {
        Debug.Log("Test");
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();

        m_input_LookDir = new Vector2(value.x, value.y * -1);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.isEnd)
        {
            if (m_LookAtObj == null)
            {
                CameraMovement();
            }
            else
            {
                CameraLock();
            }

            if (m_isAttacking || m_isBlocking)
            {
                m_targetVel = Vector3.zero;
            }
            else
            {
                Movement();
            }

            m_rb.velocity = Vector3.SmoothDamp(m_rb.velocity, m_targetVel, ref m_refVel, m_smoothVerocity);
        }
        else
        {
            if (!GetComponent<hpSystem>().isDead)
            {
                m_animator.Play("Idle");
            }
        }
    }

    private void CameraMovement()
    {
        m_followTransform.transform.rotation *= Quaternion.AngleAxis(m_input_LookDir.x * m_rotPower, Vector3.up);
        m_followTransform.transform.rotation *= Quaternion.AngleAxis(m_input_LookDir.y * m_rotPower, Vector3.right);

        Vector3 angles = m_followTransform.transform.localEulerAngles;
        angles.z = 0;

        float angle = m_followTransform.transform.localEulerAngles.x;

        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        m_followTransform.transform.localEulerAngles = angles;
        m_nextRot = Quaternion.Lerp(m_followTransform.transform.rotation, m_nextRot, Time.deltaTime * m_rotLerp);
    }

    public void CameraLock()
    {
        //m_followTransform.transform.LookAt(m_LookAtObj.transform);
        if (!m_lock)
        {
            Quaternion targetRotation = Quaternion.LookRotation(m_LookAtObj.transform.position - (m_followTransform.transform.position + new Vector3(0f, 1f, 0f)));
            m_followTransform.transform.rotation = Quaternion.Lerp(m_followTransform.transform.rotation, targetRotation, m_lockSpeed * Time.deltaTime);

            if (m_followTransform.transform.rotation == targetRotation)
            {
                m_lock = true;
            }
        }
        else
        {
            m_followTransform.transform.LookAt(m_LookAtObj.transform.position - new Vector3(0f, 1f, 0f));

            transform.rotation = Quaternion.Euler(0, m_followTransform.transform.rotation.eulerAngles.y, 0);
            m_followTransform.transform.localEulerAngles = new Vector3(m_followTransform.transform.localEulerAngles.x, 0, 0);
        }
    }

    private void Movement()
    {
        if (m_input_MoveDir.x == 0 && m_input_MoveDir.y == 0)
        {
            m_nextPos = transform.position;

            //m_rb.velocity = Vector3.zero;

            m_targetVel = Vector3.zero;

            m_animator.SetBool("isRunningF", false);
            m_animator.SetBool("isRunningB", false);
            m_animator.SetBool("isRunningL", false);
            m_animator.SetBool("isRunningR", false);

            //return;
        }
        else
        {
            float moveSpeed = m_moveSpeed;
            Vector3 moveDir = (transform.forward * m_input_MoveDir.y * moveSpeed) + (transform.right * m_input_MoveDir.x * moveSpeed);

            m_FinalMoveDir = moveDir;

            m_nextPos = transform.position + moveDir;

            m_targetVel = moveDir;

            transform.rotation = Quaternion.Euler(0, m_followTransform.transform.rotation.eulerAngles.y, 0);

            m_followTransform.transform.localEulerAngles = new Vector3(m_followTransform.transform.localEulerAngles.x, 0, 0);

            m_animator.SetBool("isRunningB", m_input_MoveDir.y < 0);
            m_animator.SetBool("isRunningF", m_input_MoveDir.y > 0);

            m_animator.SetBool("isRunningL", m_input_MoveDir.x < 0);
            m_animator.SetBool("isRunningR", m_input_MoveDir.x > 0);

            float targetAngle = 0;

            if (m_input_MoveDir.y > 0)
            {
                targetAngle = Mathf.Atan2(m_input_MoveDir.y, m_input_MoveDir.x) * Mathf.Rad2Deg;

                if (m_input_MoveDir.x == 0)
                {
                    //m_Body.transform.localEulerAngles = new Vector3(0, Mathf.Atan2(m_input_MoveDir.y, m_input_MoveDir.x) * Mathf.Rad2Deg - 90, 0);
                    targetAngle -= 90;
                }
                else if (m_input_MoveDir.x < 0)
                {
                    //m_Body.transform.localEulerAngles = new Vector3(0, Mathf.Atan2(m_input_MoveDir.y, m_input_MoveDir.x) * Mathf.Rad2Deg - 180, 0);
                    targetAngle -= 180;
                }
            }

            float bodyAngle = Mathf.SmoothDampAngle(m_Body.localEulerAngles.y, targetAngle, ref m_turnSmoothVelocity, m_smoothRot);
            m_Body.localEulerAngles = new Vector3(0f, bodyAngle, 0f);

        }
    }

    public void LookAtCameraAngle()
    {
        transform.rotation = Quaternion.Euler(0, m_followTransform.transform.rotation.eulerAngles.y, 0);
        m_followTransform.transform.localEulerAngles = new Vector3(m_followTransform.transform.localEulerAngles.x, 0, 0);
    }
}
