using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] Transform m_AttackPoint;

    [SerializeField] float m_AttackRange = 0.5f;

    [SerializeField] float m_AttackDmg = 50.0f;

    [SerializeField] float m_MaxVP = 10.0f;

    [SerializeField] float m_CurrVP = 0.0f;

    [SerializeField] float m_BlockVP = 2.0f;

    [SerializeField] float m_ClashVP = 1.0f;

    [SerializeField] float m_VulnarableDMGmultiply = 1.5f;

    [SerializeField] float vpDecrease = 0.25f;

    [SerializeField] ParticleSystem particle;

    [SerializeField] ParticleSystem particle_1;

    [SerializeField] Sound Clash;

    [SerializeField] Sound Block;

    [SerializeField] Sound Attack;

    [SerializeField] Sound Slash;

    public bool isBlock = false;

    public bool isAttacking = false;

    public bool isClashing = false;

    public bool m_AcceptingInput = false;

    public bool isVulnarable = false;

    GameObject blockTarget = null;

    LayerMask m_TargetLayer;

    private void OnEnable()
    {
        ResetVP();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetVP();

        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            m_TargetLayer = LayerMask.GetMask("Player");
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            m_TargetLayer = LayerMask.GetMask("Enemy");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isVulnarable)
        {
            if (m_CurrVP >= m_MaxVP)
            {
                isVulnarable = true;
            }

            m_CurrVP -= Time.deltaTime * vpDecrease;

            if (m_CurrVP < 0)
            {
                m_CurrVP = 0;
            }
        }
        else
        {
            m_CurrVP -= Time.deltaTime;

            if (m_CurrVP < 0)
            {
                m_CurrVP = 0;
                isVulnarable = false;
            }
        }


        Collider[] enemiesColliders = Physics.OverlapSphere(m_AttackPoint.position, m_AttackRange, m_TargetLayer);

        if (isAttacking && !isClashing)
        {
            foreach (Collider col in enemiesColliders)
            {
                if (col.gameObject.TryGetComponent(out CombatSystem combat))
                {
                    if (combat.isBlock && !combat.isVulnarable)
                    {
                        gameObject.GetComponent<Animator>().SetTrigger("Stuck");
                        blockTarget = col.gameObject;

                        Debug.Log("Block !");

                        isAttacking = false;
                        continue;
                    }

                    if (combat.isAttacking && !combat.isVulnarable)
                    {
                        combat.gameObject.GetComponent<Animator>().SetTrigger("Stuck");

                        Debug.Log("Clash !");

                        //combat.isClashing = true;
                        //isAttacking = false;
                        //combat.isAttacking = false;
                        isClashing = true;

                        continue;
                    }
                }
            }
        }
    }

    public void StartAttacking()
    {
        blockTarget = null;
        isAttacking = true;
        isClashing = false;

        Debug.Log("StartAttacking from " + gameObject.name);

        Attack.PlayRandom();
    }
    public void StopAttacking()
    {
        isAttacking = false;

        Debug.Log("StopAttacking from " + gameObject.name);
    }

    public void StopClashing()
    {
        isClashing = false;

        Debug.Log("StopClashing from " + gameObject.name);
    }

    public void Clashing()
    {
        if (isClashing && !isVulnarable)
        {
            m_CurrVP += m_ClashVP;

            Clash.PlayRandom();

            Debug.Log("Clashing from " + gameObject.name);

            if (particle != null)
            {
                particle.Emit(5);
            }
        }

        if (blockTarget != null)
        {
            CombatSystem cs = blockTarget.GetComponent<CombatSystem>();

            cs.AddVP(cs.m_BlockVP);

            blockTarget = null;

            Block.PlayRandom();

            if (particle != null)
            {
                particle_1.Emit(5);
            }

            Debug.Log("Blocking from " + gameObject.name);
        }

        isAttacking = false;
        isClashing = false;
    }


    public void StartAcceptInput()
    {
        m_AcceptingInput = true;
    }

    public void StopAcceptInput()
    {
        m_AcceptingInput = false;
    }

    public void ShakeCam()
    {
        CinemachineShake.instance.ShakeCamera(10, 0.3f);
    }

    public void DealDamage()
    {
        if (!isAttacking)
        {
            return;
        }

        Collider[] enemiesColliders = Physics.OverlapSphere(m_AttackPoint.position, m_AttackRange, m_TargetLayer);

        foreach (Collider col in enemiesColliders)
        {
            if (col.gameObject.TryGetComponent(out CombatSystem combat))
            {
                if (combat.isVulnarable)
                {
                    if (col.gameObject.TryGetComponent(out hpSystem hp))
                    {
                        hp.TakeDamage(m_AttackDmg * m_VulnarableDMGmultiply);
                        Slash.PlayRandom();

                        continue;
                    }
                }

                if (!isClashing)
                {
                    if (col.gameObject.TryGetComponent(out hpSystem hp))
                    {
                        hp.TakeDamage(m_AttackDmg);
                        Slash.PlayRandom();
                    }
                }
            }

        }

        isAttacking = false;
    }


    private void OnDrawGizmosSelected()
    {
        if (m_AttackPoint == null)
            return;

        Gizmos.DrawWireSphere(m_AttackPoint.position, m_AttackRange);
    }

    public void ResetVP()
    {
        m_CurrVP = 0;
    }


    public float GetVP() { return m_CurrVP; }

    public float GetVPRatio() { return m_CurrVP / m_MaxVP; }

    public void AddVP(float value) { m_CurrVP += value; }
}
