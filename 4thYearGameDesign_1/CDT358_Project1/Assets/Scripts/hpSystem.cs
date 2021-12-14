using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpSystem : MonoBehaviour
{
    [SerializeField] float m_MaxHP = 100.0f;
    float m_HP = 100.0f;

    public float CurrHP { get { return m_HP; } }

    public bool isDead = false;

    private void OnEnable()
    {
        ResetHP();

        if (TryGetComponent(out Collider col))
        {
            col.enabled = true;
        }

        if (TryGetComponent(out Rigidbody rigid))
        {
            rigid.isKinematic = false;
        }
    }

    void Start()
    {
        ResetHP();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetHP()
    {
        m_HP = m_MaxHP;
    }

    public void TakeDamage(float damage)
    {
        m_HP -= damage;

        if (m_HP <= 0)
        {
            isDead = true;
            Dead();
        }
        else if(!isDead)
        {
            if (TryGetComponent(out Animator animator))
            {
                animator.SetTrigger("Impact");
            }
        }
    }

    void Dead()
    {
        if (TryGetComponent(out Collider col))
        {
            col.enabled = false;
        }

        if (TryGetComponent(out Rigidbody rigid))
        {
            rigid.isKinematic = true;
        }

        if (TryGetComponent(out Animator animator))
        {
            animator.SetTrigger("Dead");

            if (isActiveAndEnabled)
            {
                StartCoroutine(DieIn(5f));
            }
        }
    }

    public IEnumerator DieIn(float sec)
    {
        yield return new WaitForSeconds(sec);
        gameObject.SetActive(false);
    }

    public float GetHPRatio()
    {
        return m_HP / m_MaxHP;
    }
}
