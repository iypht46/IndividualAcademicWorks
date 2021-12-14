using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float Stun_time = 1;

    [SerializeField] float Bullet_Speed = 10f;
    [SerializeField] float Bullet_Rate = 1f;
    [SerializeField] float Bullet_init = 0f;

    [SerializeField] float Bullet_Reload_Time = 1f;

    [SerializeField] LayerMask mask;

    [SerializeField] AudioClip[] GunSFX;

    public bool isStun;
    public AudioSource audioSource;
    public GameObject Gun_curr;

    Vector2 gunDir;

    Mouse mouse;
    GameObject Player;

    ObjectPool bulletPool;

    Color StartColor;

    float stunT;
    float bulletT;
    float reloadT;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {

        bulletPool = GameObject.Find("BulletPoolEnemy").GetComponent<ObjectPool>();
        mouse = GameObject.Find("Mouse").GetComponent<Mouse>();
        Player = GameObject.Find("Player");

        StartColor = GetComponent<SpriteRenderer>().color;

        bulletT = Random.Range(Bullet_init - 2.0f, Bullet_init);

        reloadT = 0;
    }

    private void OnEnable()
    {
        reloadT = 0;

        isStun = false;
        bulletT = Random.Range(Bullet_init - 2.0f, Bullet_init);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStun)
        {
            GetComponent<SpriteRenderer>().color = StartColor;
            GunMovement();

            if (bulletT < Bullet_Rate)
            {
                bulletT += Time.deltaTime;
            }
            else
            {
                if (Gun_curr != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(Gun_curr.transform.position, gunDir, Mathf.Infinity, mask);

                    //Debug.Log(hit.collider.gameObject.layer);

                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            bulletT = 0;
                            Shoot();
                        }
                    }
                }
            }

            if (Gun_curr != null)
            {
                if (Gun_curr.GetComponent<Gun>().bullet_amount == 0)
                {
                    reloadT += Time.deltaTime;

                    if (reloadT >= Bullet_Reload_Time)
                    {
                        reloadT = 0;
                        Gun_curr.GetComponent<Gun>().Reload();
                    }
                }
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;

            if (stunT < Stun_time)
            {
                stunT += Time.deltaTime;
            }
            else
            {
                stunT = 0;
                isStun = false;
            }
        }
    }

    void GunMovement()
    {
        gunDir = new Vector2(Player.transform.position.x - transform.position.x, Player.transform.position.y - transform.position.y).normalized;

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

                GameObject bullet = bulletPool.GetObj();

                if (bullet != null)
                {
                    bullet.SetActive(true);

                    bullet.transform.position = gameObject.transform.position + new Vector3(gunDir.x, gunDir.y, 0.0f);
                    bullet.GetComponent<Rigidbody2D>().velocity = gunDir * Bullet_Speed;

                    audioSource.clip = GunSFX[UnityEngine.Random.Range(0, GunSFX.Length)];
                    audioSource.Play();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Gun_curr.GetComponent<Gun>().Owner = null;
            Gun_curr = null;
            gameObject.SetActive(false);

            GameObject.Find("GameController").GetComponent<GameController>().totalEnemy += 1;
        }
    }
}
