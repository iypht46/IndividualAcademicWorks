using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float m_Movespeed = 5f;
    [SerializeField] float m_RotateSpeed = 50f;
    [SerializeField] float m_MaxSpeed = 25f;

    Rigidbody m_rigidbody;

    Vector2 m_MoveDir = Vector2.zero;
    float m_RotateDir = 0;

    public Vector2 MoveDir { get { return m_MoveDir; } }
    public float RotateDir { get { return m_RotateDir; } }

    bool isRotating = false;

    bool Attaching = false;
    public bool isAttaching { get { return Attaching; } }

    Player player;


    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Attaching)
        {
            BroadcastMessage("Attach");
        }

        if (!isRotating && !Attaching && !GameController.Instance.isBreak)
        {
            BroadcastMessage("Shoot", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void FixedUpdate()
    {
        Movement();

        Rotate();
    }

    void Movement()
    {
        //m_rigidbody.velocity = new Vector3(m_MoveDir.x * m_Movespeed, 0.0f, m_MoveDir.y * m_Movespeed);

        float veloX = m_rigidbody.velocity.x;
        float veloZ = m_rigidbody.velocity.z;

        m_rigidbody.AddForce(new Vector3(m_MoveDir.x * m_Movespeed * 10f, 0.0f, m_MoveDir.y * m_Movespeed * 10f));

        m_rigidbody.velocity = Vector3.ClampMagnitude(m_rigidbody.velocity, m_MaxSpeed);

    }

    void Rotate()
    {
        transform.Rotate(new Vector3(0, m_RotateSpeed * m_RotateDir * Time.deltaTime, 0));

        isRotating = false;

        if (m_RotateDir != 0)
        {
            isRotating = true;
        }
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();

        m_MoveDir = value;
    }

    public void RotateInput(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();

        m_RotateDir = value;
    }

    public void DetachInput(InputAction.CallbackContext context)
    {
        BroadcastMessage("Detach");
    }

    public void AttachInput(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();

        if (value > 0.75f)
        {
            Attaching = true;
        }
        else
        {
            Attaching = false;
        }
    }

}
