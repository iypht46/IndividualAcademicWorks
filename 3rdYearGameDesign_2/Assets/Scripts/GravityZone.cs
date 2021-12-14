using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{
    [SerializeField] float m_GravityScale = 1f;
    [SerializeField] Color m_GravityUpColor;
    [SerializeField] Color m_GravityDownColor;

    List<Collider2D> m_CollidersInField = new List<Collider2D>();

    private void Update()
    {
        ChangeObjectGravity();
    }

    public void SwitchGravity()
    {
        SoundPlayer.Instance.PlaySFX(1);
        m_GravityScale *= -1;
        ChangeObjectGravity();
    }

    public void SetGravity(int scale)
    {
        SoundPlayer.Instance.PlaySFX(1);
        m_GravityScale = scale;
        ChangeObjectGravity();
    }

    public void ChangeObjectGravity()
    {
        if (m_GravityScale == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().color = m_GravityDownColor;
        }
        else if(m_GravityScale == -1)
        {
            gameObject.GetComponent<SpriteRenderer>().color = m_GravityUpColor;
        }

        foreach (Collider2D col in m_CollidersInField)
        {
            if (col.gameObject.TryGetComponent(out EffectByGravity eff))
            {
                //eff.SwitchGravity(m_GravityScale);
                eff.SetGravityFlag(m_GravityScale);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!m_CollidersInField.Contains(collision))
        {
            m_CollidersInField.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_CollidersInField.Remove(collision);
    }

}
