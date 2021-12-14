using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] public int bullet_max_amount = 10;
    /*[HideInInspector]*/ public int bullet_amount = 10;
    [SerializeField] float rot_speed = 2;

    [SerializeField] private float Gun_rot_speed = 200.0f;
    [SerializeField] private float Gun_Throw_speed = 15.0f;


    [HideInInspector] public GameObject targetEnem;
    [HideInInspector] public Vector3 Dir;
    public GameObject Owner;

    public bool isThrown;

    Rigidbody2D GunRb;

    Color StartColor;
    [SerializeField] Color EmptyColor;
    
    void Start()
    {
        bullet_amount = bullet_max_amount;
        isThrown = false;
        GunRb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        StartColor = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
    }

    private void OnEnable()
    {
        isThrown = false;
        targetEnem = null;
        Owner = null;

        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.GetChild(0).rotation = Quaternion.identity;
    }
    
    void Update()
    {
        if (targetEnem != null)
        {
            if (!targetEnem.activeInHierarchy)
            {
                targetEnem = null;
            }
        }

        if (isThrown)
        {
            transform.GetChild(0).Rotate(0.0f, 0.0f, rot_speed * Time.deltaTime);

            if (targetEnem != null)
            {
                Vector3 throwDir = (targetEnem.transform.position - transform.position).normalized;

                float rotateAmount = Vector3.Cross(throwDir, transform.up).z;

                GunRb.angularVelocity = -rotateAmount * Gun_rot_speed;

                GunRb.velocity = transform.up * Gun_Throw_speed;
            }
            else
            {
                GunRb.velocity = Dir * Gun_Throw_speed;
            }
        }

        if (bullet_amount != 0)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = StartColor;
        }
        else
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = EmptyColor;
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (isThrown)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                gameObject.SetActive(false);
            }

            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                col.gameObject.GetComponent<Enemy>().isStun = true;
                isThrown = false;
                targetEnem = null;
                gameObject.SetActive(false);
            }
        }
    }

    public void Reload()
    {
        bullet_amount = bullet_max_amount;
    }

    public void Reload(int bullet)
    {
        bullet_max_amount = bullet;
        bullet_amount = bullet_max_amount;
    }

    public void SetOwner(GameObject o)
    {
        Owner = o;
    }
}
