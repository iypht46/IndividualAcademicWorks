using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private static PlayerController m_instance;
    public static PlayerController Instance { get { return m_instance; } }

    [SerializeField] public float m_base_move_speed = 5.0f;
    [SerializeField] public float m_godmode_up_down_speed = 2.0f;

    [SerializeField] float m_jump_force = 10.0f;
    [SerializeField] int m_jump_times = 2;

    [SerializeField] float m_speed_up = 0.25f;
    [SerializeField] float m_speed_down = 0.25f;
    [SerializeField] float m_speed_max = 12f;

    [SerializeField] ObjectPool m_spear_pool;
    [SerializeField] GameObject m_spear_pos;
    [SerializeField] float m_spear_speed = 5.0f;

    [SerializeField] float m_hit_slow_time = 1f;

    [SerializeField] GameObject[] m_Body;
    [SerializeField] Color m_hitColor;

    
    Rigidbody2D m_rb;

    float m_move_dir = 0;
    int m_move_dir_y = 0;

    int m_jump_count = 0;

    bool m_Jump = false;
    bool m_OnGround = false;
    bool m_EnemyHit = false;
    bool m_isGodMode = false;
    public bool m_OutOfBound = false;

    float m_slowT;

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_instance = this;
        }
    }

    void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody2D>();

        m_OutOfBound = false;

        m_slowT = 0;
    }

    void Update()
    {
        if (!GameController.Instance.m_isEnd && !GameController.Instance.m_isPause)
        {
            ManageInput();

            CheckOutOfBound();

            if (m_EnemyHit)
            {
                m_slowT += Time.deltaTime;

                if (m_slowT > m_hit_slow_time)
                {
                    m_EnemyHit = false;
                    m_slowT = 0;

                    foreach (GameObject obj in m_Body)
                    {
                        obj.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }

            Movement();
        }
    }

    void ManageInput()
    {
        m_move_dir = 0;
        m_move_dir_y = 0;


        if (Input.GetKey(KeyCode.A))
        {
            m_move_dir = -1.25f;
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x) * -1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            m_move_dir = 0.75f;
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }

        if (!m_isGodMode)
        {
            if (Input.GetKeyDown(KeyCode.Space) && (m_jump_count != 0))
            {
                Jump();
            }

            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.S)) && (!m_OnGround))
            {
                Stomp();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                m_move_dir_y = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                m_move_dir_y = -1;
            }
        }


        if (Input.GetKeyDown(KeyCode.G))
        {
            m_isGodMode = !m_isGodMode;

            if (m_isGodMode)
            {
                m_rb.gravityScale = 0;
            }
            else
            {
                m_rb.gravityScale = 1.75f;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Movement()
    {
        if (m_isGodMode)
        {
            m_rb.velocity = new Vector2(m_rb.velocity.x, m_godmode_up_down_speed * m_move_dir_y);
        }

        if (m_EnemyHit)
        {
            m_rb.velocity = new Vector2((m_base_move_speed * 0.25f) * m_move_dir, m_rb.velocity.y);
        }
        else
        {
            m_rb.velocity = new Vector2(m_base_move_speed * m_move_dir, m_rb.velocity.y);
        }
    }

    void Jump()
    {
        m_rb.velocity = new Vector2(m_rb.velocity.x, m_jump_force);

        m_Jump = true;
        m_jump_count--;
    }

    void Stomp()
    {
        m_rb.velocity = new Vector2(m_rb.velocity.x, -m_jump_force * 1.75f);
    }

    void Shoot()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 spearDir = new Vector2(mousePos.x - m_spear_pos.transform.position.x, mousePos.y - m_spear_pos.transform.position.y).normalized;
        float Angle = (Mathf.Atan2(spearDir.y, spearDir.x) * Mathf.Rad2Deg);

        GameObject spear = m_spear_pool.GetObj();

        if (spear != null)
        {
            spear.transform.position = m_spear_pos.transform.position;
            spear.transform.eulerAngles = new Vector3(0, 0, Angle - 90);

            spear.SetActive(true);
            spear.GetComponent<Rigidbody2D>().velocity = spearDir * (m_spear_speed + m_base_move_speed);

            SoundPlayer.Instance.PlaySFX(Random.Range(1, 3));
        }
    }

    void CheckOutOfBound()
    {
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        if ((transform.position.x + (Mathf.Abs(transform.localScale.x) / 2)) < (-width / 2))
        {
            m_OutOfBound = true;
        }
        else if ((transform.position.y + (Mathf.Abs(transform.localScale.y))) < (-height / 2))
        {
            m_OutOfBound = true;
        }
        else
        {
            m_OutOfBound = false;
        }
    }

    public void AddSpeed()
    {
        m_base_move_speed += m_speed_up;

        if (m_base_move_speed > m_speed_max)
        {
            m_base_move_speed = m_speed_max;
        }
    }

    public void DecSpeed()
    {
        m_base_move_speed -= m_speed_down;
    }



    public void AddSpeed(float spd)
    {
        m_base_move_speed += spd;

        if (m_base_move_speed > m_speed_max)
        {
            m_base_move_speed = m_speed_max;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (m_slowT == 0)
            {
                m_EnemyHit = true;

                foreach (GameObject obj in m_Body)
                {
                    obj.GetComponent<SpriteRenderer>().color = m_hitColor;
                    SoundPlayer.Instance.PlaySFX(7);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            m_jump_count = m_jump_times;
            m_OnGround = true;
            m_Jump = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            m_OnGround = false;
        }
    }

}
