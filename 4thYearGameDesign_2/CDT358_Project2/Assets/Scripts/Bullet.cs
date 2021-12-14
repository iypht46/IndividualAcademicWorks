using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float m_Damage = 1f;
    [SerializeField] float m_Pushforce = 5000f;

    [SerializeField] Material m_PlayerColor;
    [SerializeField] Material m_EnemyColor;

    [SerializeField] Gradient m_PlayerGradiant;
    [SerializeField] Gradient m_EnemyGradiant;

    [SerializeField] Renderer m_Renderer;
    [SerializeField] GameObject m_DisplayObject;

    [SerializeField] TrailRenderer trailRenderer;

    [HideInInspector] public int TargetLayer = 0;

    [HideInInspector] public float m_Speed = 0;

    protected Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetProperties()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Bullet_Player"))
        {
            m_Renderer.material = m_PlayerColor;

            trailRenderer.colorGradient = m_PlayerGradiant;

            TargetLayer = LayerMask.NameToLayer("Bullet_Enemy");
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Bullet_Enemy"))
        {
            m_Renderer.material = m_EnemyColor;

            trailRenderer.colorGradient = m_EnemyGradiant;

            TargetLayer = LayerMask.NameToLayer("Bullet_Player");
        }
    }

    private void Push(Collider collider)
    {
        Rigidbody rigidbody = collider.gameObject.GetComponent<Rigidbody>();

        if (rigidbody != null)
        {
            //Vector3 pushDir = collider.gameObject.transform.position - transform.position;

            rigidbody.AddForce(transform.forward * m_Pushforce, ForceMode.Impulse);
        }
    }

    public void SetDisplayObjectSize(Vector3 size)
    {
        m_DisplayObject.transform.localScale = size;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.layer == LayerMask.NameToLayer("Border"))
        {
        }*/

        if (other.gameObject.TryGetComponent(out hpSystem hp))
        {
            hp.ModifyHPValue(-m_Damage);
        }

        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.SetActive(false);

        if (other.gameObject.TryGetComponent(out Player player))
            return;

        if (other.gameObject.TryGetComponent(out Enemy enemy))
            return;

        Push(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out hpSystem hp))
        {
            hp.hitDir = transform.forward;
            hp.ModifyHPValue(-m_Damage);
        }

        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

}
