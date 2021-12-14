using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hpSystem : MonoBehaviour
{
    [SerializeField] float m_MaxHp = 1f;
    [SerializeField] public bool invulnerable = false;
    [SerializeField] float m_DeadDelay = 0.2f;

    [SerializeField] ConnectionPointGroup connectionPointGroup;

    [SerializeField] Image hurtImage;

    [SerializeField] bool m_DestroyOnDetach = false;

    [SerializeField] Renderer[] renderers = null;
    [SerializeField] Material normal;
    [SerializeField] Material oneHit;

    ObjectPool m_ExplosionPool;

    float m_CurrHp;

    public float HP { get { return m_CurrHp; } }

    public Vector3 hitDir;
    public bool isDead = false;

    private void Start()
    {
        ResetHP();

        m_ExplosionPool = GameObject.Find("ExplosionPool").GetComponent<ObjectPool>();
    }

    private void Update()
    {
        if (GameController.Instance.isEnd)
        {
            if (hurtImage != null)
            {
                hurtImage.gameObject.SetActive(false);
            }
        }


        foreach (Renderer renderer in renderers)
        {
            if (m_CurrHp == 1)
            {
                renderer.material = oneHit;
            }
            else
            {
                renderer.material = normal;
            }
        }
    }

    public void ResetHP()
    {
        m_CurrHp = m_MaxHp;
        isDead = false;
    }

    public void Respawn()
    {
        ResetHP();
        gameObject.SetActive(true);
    }

    public void ModifyHPValue(float value)
    {
        if (!invulnerable)
            m_CurrHp += value;

        if (value < 0)
            TakeDamage();

        m_CurrHp = Mathf.Clamp(m_CurrHp, 0, m_MaxHp);

        if (m_CurrHp <= 0)
        {
            ReachZero();
        }
    }

    void Explode()
    {
        GameObject obj = m_ExplosionPool.GetObj();

        if (obj == null)
            return;

        ParticleSystem explode = obj.GetComponent<ParticleSystem>();

        if (explode != null)
        {
            explode.gameObject.transform.position = this.transform.position;

            explode.gameObject.SetActive(true);
            explode.Play();
        }
    }

    void TakeDamage()
    {
        if (hurtImage != null)
        {
            gameObject.GetComponent<Sound>().PlayRandom();

            hurtImage.gameObject.SetActive(true);
            hurtImage.color = new Color(Color.red.r, Color.red.g, Color.red.b, 60f/255f);
            StartCoroutine(FlashRed(0.2f));
        }
    }

    IEnumerator FlashRed(float sec)
    {
        yield return new WaitForSeconds(sec);
        hurtImage.gameObject.SetActive(false);
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(m_DeadDelay);
        gameObject.SetActive(false);

        Explode();

    }

    void ReachZero()
    {
        isDead = true;

        if (TryGetComponent(out Player player))
        {
            player.DetachAll();

            StartCoroutine(Dead());
            return;
        }

        if (TryGetComponent(out Enemy enemy))
        {
            enemy.DetachAll();

            Sound sound = GetComponent<Sound>();

            if (sound != null)
            {
                sound.PlayRandom();
            }

            StartCoroutine(Dead());
            return;
        }

        if (connectionPointGroup != null)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Body_Player"))
            {
                connectionPointGroup.DetachAll();
                BroadcastMessage("Detach");

                gameObject.transform.parent = null;
                StartCoroutine(Dead());
            }
            else
            {
                if (connectionPointGroup.HasModule() && !m_DestroyOnDetach)
                {
                    connectionPointGroup.DetachAll();
                    invulnerable = true;
                }
                else
                {
                    gameObject.transform.parent = null;
                    BroadcastMessage("Detach");

                    if (m_DestroyOnDetach)
                    {
                        StartCoroutine(Dead());
                    }
                }
            }
        }

    }

}
