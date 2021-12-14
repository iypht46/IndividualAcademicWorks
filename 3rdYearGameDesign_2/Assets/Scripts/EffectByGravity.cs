using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectByGravity : MonoBehaviour
{
    Rigidbody2D m_rb;

    bool m_inUpGrav;
    bool m_inDownGrav;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        SetGravity();   
    }

    public void SetGravity()
    {
        if (m_inUpGrav && m_inDownGrav)
        {
            m_rb.gravityScale = 0;
            //m_rb.velocity = Vector2.zero;
            m_inUpGrav = false;
            m_inDownGrav = false;
        }
        else if(m_inUpGrav)
        {
            m_rb.gravityScale = -1;
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, Mathf.Abs(gameObject.transform.localScale.y) * -1, gameObject.transform.localScale.z);
            m_inUpGrav = false;
        }
        else if (m_inDownGrav)
        {
            m_rb.gravityScale = 1;
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, Mathf.Abs(gameObject.transform.localScale.y), gameObject.transform.localScale.z);
            m_inDownGrav = false;
        }
    }

    public void SetGravityFlag(float scale)
    {
        if (scale == 1)
        {
            m_inDownGrav = true;
        }
        else if(scale == -1)
        {
            m_inUpGrav = true;
        }
    }


    public void SwitchGravity(float scale)
    {
        m_rb.gravityScale = scale;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, Mathf.Abs(gameObject.transform.localScale.y) * scale, gameObject.transform.localScale.z);
    }
}
