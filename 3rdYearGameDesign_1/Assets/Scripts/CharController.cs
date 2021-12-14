using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float move_speed = 200;

    [SerializeField] private float dash_dist = 200;
    [SerializeField] private float dash_speed = 200;
    [SerializeField] private float dash_delayTime = 0.5f;
    [SerializeField] private float dash_stundashMouseDist = 0.5f;

    [SerializeField] private float bullet_speed = 1000;

    [SerializeField] private int bullet_max = 6;

    [Header("HP")]
    [SerializeField] public float hp_max = 200f;
    [SerializeField] private float hp_damage = 10f;

    [Header("UI Stuff")]
    [SerializeField] Text Txt_CurrAmmo;
    [SerializeField] Slider ui_slider;

    [HideInInspector] public float hp_curr;
    [HideInInspector] public GameObject Gun_curr;

    [Header("Player SFX")]
    [SerializeField] AudioClip[] GunSFX;
    [SerializeField] AudioClip[] TakeDownSFX;
    [SerializeField] AudioClip[] DashSFX;
    [SerializeField] AudioClip[] HitSFX;
    [SerializeField] AudioClip[] ThrowSFX;

    [HideInInspector] public AudioSource audioSource;


    int bullet_curr;

    Rigidbody2D rb;

    Vector3 endPosition;
    Vector3 dashDir;
    Vector2 moveDir;
    Vector2 gunDir;

    Color StartColor;

    ObjectPool BulletPool;
    Mouse mouse;

    Color Txt_OGcolor;

    public Enemy[] enemies;
    GameObject TargetEnem;
    CameraController cam;
    
    public bool isDash;
    bool isSTDash;
    float in_H, in_V;
    float dashT = 0;

    public bool isGodMode = false;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>();

        rb = GetComponent<Rigidbody2D>();
        isDash = false;

        StartColor = GetComponent<SpriteRenderer>().color;

        BulletPool = GameObject.Find("BulletPool").GetComponent<ObjectPool>();
        bullet_curr = bullet_max;
        mouse = GameObject.Find("Mouse").GetComponent<Mouse>();

        enemies = FindObjectsOfType(typeof(Enemy)) as Enemy[];

        hp_curr = hp_max;

        Txt_OGcolor = Txt_CurrAmmo.color;
    }

    void Update()
    {
        ManageInput();

        GunMovement();
    }

    void FixedUpdate()
    {
        UpdateUI();

        Movement();
    }

    void ManageInput()
    {
        moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            moveDir.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDir.x = 1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            moveDir.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDir.y = -1;
        }

        if (!GameObject.Find("GameController").GetComponent<GameController>().isPause)
        {
            if (!isDash)
            {
                if (dashT <= 0)
                {
                    GetComponent<SpriteRenderer>().color = StartColor;

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        dashDir = moveDir;
                        endPosition = transform.position + (dashDir.normalized * dash_dist);

                        isDash = true;

                        audioSource.clip = DashSFX[UnityEngine.Random.Range(0, DashSFX.Length)];
                        audioSource.Play();

                        StunDash();
                    }
                }
                else
                {
                    dashT -= Time.deltaTime;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StunDash();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    Shoot();
                }

                if (Input.GetMouseButtonDown(1) && Gun_curr != null)
                {
                    ThrowGun();
                }
            }
        }

       
    }

    void Movement()
    {
        if (!isDash)
        {
            rb.velocity = new Vector2(moveDir.x, moveDir.y) * move_speed;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = StartColor * Color.gray;

            if (Vector3.Distance(transform.position, endPosition) >= 1.0f)
            {
                if (isSTDash)
                {
                    rb.velocity = new Vector2(dashDir.x, dashDir.y) * dash_speed * 2.0f;
                }
                else
                {
                    rb.velocity = new Vector2(dashDir.x, dashDir.y) * dash_speed;
                }
            }
            else
            {
                dashT = dash_delayTime;

                if (isSTDash)
                {
                    GameObject.Find("GameController").GetComponent<GameController>().totalEnemy += 1;

                    dashT = 0;

                    if (TargetEnem.GetComponent<Enemy>().Gun_curr != null)
                    {
                        if(Gun_curr != null)
                        {
                            Gun_curr.gameObject.GetComponent<Gun>().SetOwner(null);
                        }

                        Gun_curr = TargetEnem.GetComponent<Enemy>().Gun_curr;
                        Gun_curr.gameObject.GetComponent<Gun>().SetOwner(gameObject);
                        TargetEnem.GetComponent<Enemy>().Gun_curr = null;

                        TargetEnem.SetActive(false);

                        audioSource.clip = GunSFX[UnityEngine.Random.Range(0, GunSFX.Length)];
                        audioSource.Play();
                    }
                }

                isDash = false;
                isSTDash = false;
            }
        }
    }

    void GunMovement()
    {
        gunDir = new Vector2(mouse.transform.position.x - transform.position.x, mouse.transform.position.y - transform.position.y).normalized;

        if (Gun_curr != null)
        {
            Gun_curr.transform.position = gameObject.transform.position + (new Vector3(gunDir.x, gunDir.y, 0.0f) * 0.5f);

            float Angle = (Mathf.Atan2(gunDir.y, gunDir.x) * Mathf.Rad2Deg);
            Gun_curr.transform.eulerAngles = new Vector3(0.0f, 0.0f, Angle + 180);

            if (Angle > -90 && Angle < 90)
            {
                Gun_curr.transform.localScale = new Vector3(Gun_curr.transform.localScale.x, Mathf.Abs(Gun_curr.transform.localScale.y) * -1, Gun_curr.transform.localScale.z);
            }
            else
            {
                Gun_curr.transform.localScale = new Vector3(Gun_curr.transform.localScale.x, Mathf.Abs(Gun_curr.transform.localScale.y), Gun_curr.transform.localScale.z);
            }
        }
    }

    void Shoot()
    {
        if (Gun_curr != null)
        {
            Gun g_curr = Gun_curr.GetComponent<Gun>();

            if (g_curr.bullet_amount != 0)
            {
                g_curr.bullet_amount -= 1;

                GameObject bullet = BulletPool.GetObj();

                if (bullet != null)
                {
                    audioSource.clip = TakeDownSFX[UnityEngine.Random.Range(0, TakeDownSFX.Length)];
                    audioSource.Play();

                    bullet.SetActive(true);

                    bullet.transform.position = gameObject.transform.position + new Vector3(gunDir.x, gunDir.y, 0.0f);
                    bullet.GetComponent<Rigidbody2D>().velocity = gunDir * bullet_speed;
                }
            }
        }
    }

    void ThrowGun()
    {
        GameObject enem = null;
        float minDist = 999f;

        if (Gun_curr != null)
        {
            foreach (Enemy e in enemies)
            {
                if (!e.gameObject.activeInHierarchy)
                {
                    continue;
                }

                float Dist = Vector3.Distance(mouse.transform.position, e.transform.position);

                if ((Dist < minDist) && (Dist <= dash_stundashMouseDist))
                {
                    minDist = Dist;
                    enem = e.gameObject;
                }
            }

            Gun GunCom = this.Gun_curr.GetComponent<Gun>();

            GunCom.isThrown = true;
            GunCom.Owner = null;
            GunCom.targetEnem = null;
            GunCom.Dir = gunDir;

            if (enem != null)
            {
                GunCom.targetEnem = enem;
            }

            audioSource.clip = ThrowSFX[UnityEngine.Random.Range(0, ThrowSFX.Length)];
            audioSource.Play();

            Gun_curr = null;
        }
    }

    void StunDash()
    {
        foreach (Enemy e in enemies)
        {
            if (!e.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (!e.isStun)
            {
                continue;
            }

            if (Vector3.Distance(mouse.transform.position, e.transform.position) <= dash_stundashMouseDist)
            {
                isDash = true;
                isSTDash = true;
                endPosition = e.gameObject.transform.position;
                TargetEnem = e.gameObject;
                dashDir = (e.gameObject.transform.position - transform.position).normalized;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isDash = false;
            isSTDash = false;
            dashT = dash_delayTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Gun") && Gun_curr == null)
        {
            if (col.TryGetComponent(out Gun g))
            {
                if (g.Owner == null && !g.isThrown)
                {
                    Gun_curr = g.gameObject;
                    col.gameObject.GetComponent<Gun>().SetOwner(gameObject);
                }
            }
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("BulletEnem"))
        {
            if (!isDash)
            {
                if (!isGodMode)
                {
                    hp_curr -= hp_damage;

                    cam.screenShake(0.2f);

                    audioSource.clip = HitSFX[UnityEngine.Random.Range(0, HitSFX.Length)];
                    audioSource.Play();
                }

                col.gameObject.SetActive(false);
            }

            if (hp_curr <= 0)
            {
                UpdateUI();
                GameObject.Find("GameController").GetComponent<GameController>().ActiveEnding(true);
                GameObject.Find("GameController").GetComponent<GameController>().ActiveIngame(false);
                GameObject.Find("GameController").GetComponent<GameController>().UpdateEnding();
                gameObject.SetActive(false);

                Cursor.visible = true;
            }
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Goal"))
        {
            isDash = false;
            GameObject.Find("GameController").GetComponent<GameController>().NewStage();
        }
    }

    void UpdateUI()
    {
        if (Gun_curr != null)
        {
            Txt_CurrAmmo.text = Gun_curr.GetComponent<Gun>().bullet_amount.ToString() + "/" + Gun_curr.GetComponent<Gun>().bullet_max_amount.ToString();

            if (Gun_curr.GetComponent<Gun>().bullet_amount == 0)
            {
                Txt_CurrAmmo.color = Color.red;
            }
            else
            {
                Txt_CurrAmmo.color = Txt_OGcolor;
            }
        }
        else
        {
            Txt_CurrAmmo.text = "0";
        }

        ui_slider.value = hp_curr / hp_max;
    }

    public void Respawn()
    {
        /*Gun_curr = GameObject.Find("GunPool").GetComponent<ObjectPool>().GetObj();

        Gun_curr.GetComponent<Gun>().SetOwner(gameObject);
        Gun_curr.SetActive(true);*/

        hp_curr = hp_max;
    }
}
