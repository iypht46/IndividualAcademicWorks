using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_move_speed = 5.0f;
    [SerializeField] float m_jump_force = 10.0f;
    [SerializeField] Fader m_Fader;

    Rigidbody2D m_rb;

    int m_move_dir = 0;
    bool m_Jump = false;
    bool m_OnGround = false;
    bool m_CollideWall = false;

    bool m_SwitchTrigger = false;

    GameObject m_Switch;

    void Start()
    {
        if (m_Fader != null)
        {
            m_Fader.Fade(Fader.FadeType.FadeIn);
        }
        m_rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ManageInput();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void ManageInput()
    {
        m_move_dir = 0;

        /*if (!(m_Jump && m_CollideWall) || (m_rb.gravityScale == 0))
        {*/
        //}

        if (Input.GetKey(KeyCode.A))
        {
            m_move_dir = -1;
            gameObject.transform.localScale = new Vector3(-1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            m_move_dir = 1;
            gameObject.transform.localScale = new Vector3(1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            m_rb.gravityScale *= -1;

            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y * -1, gameObject.transform.localScale.z);
        }

        if (m_SwitchTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (m_Switch.TryGetComponent(out SwitchObj sw))
                {
                    sw.CallOnPress();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !m_Jump && m_OnGround)
        {
            SoundPlayer.Instance.PlaySFX(0);
            Jump();
        }
    }

    void Movement()
    {
        m_rb.velocity = new Vector2(m_move_speed * m_move_dir, m_rb.velocity.y);

    }

    void Jump()
    {
        m_rb.AddForce(new Vector2(0, m_jump_force * m_rb.gravityScale), ForceMode2D.Impulse);
        m_Jump = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform") || (collision.gameObject.layer == LayerMask.NameToLayer("Object")))
        {
            m_CollideWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform") || (collision.gameObject.layer == LayerMask.NameToLayer("Object")))
        {
            m_CollideWall = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Platform")) || (collision.gameObject.layer == LayerMask.NameToLayer("Object")))
        {
            m_OnGround = true;
            m_Jump = false;
        }

        if (collision.gameObject.tag == "Switch")
        {
            m_SwitchTrigger = true;
            m_Switch = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Platform")) || (collision.gameObject.layer == LayerMask.NameToLayer("Object")))
        {
            m_OnGround = false;
        }

        if (collision.gameObject.tag == "Switch")
        {
            m_Switch = null;
            m_SwitchTrigger = false;
        }
    }

}
