using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PressureSwitch : MonoBehaviour
{
    [SerializeField] UnityEvent OnPress;
    [SerializeField] UnityEvent OnOut;
    [SerializeField] GameObject[] m_DisplayLight;

    [SerializeField] Color m_DisplayLightOnColor;

    [SerializeField] int RequiredAmount = 1;

    GravityZone[] m_gravityZones;

    Color[] m_DisplayLightOgColor;

    [SerializeField] int m_ObjOnTop = 0;

    [SerializeField] bool m_isActive = false;

    private void Start()
    {
        m_gravityZones = FindObjectsOfType<GravityZone>() as GravityZone[];
        m_DisplayLightOgColor = new Color[m_DisplayLight.Length];

        for (int i = 0; i < m_DisplayLight.Length; i++)
        {
            m_DisplayLightOgColor[i] = m_DisplayLight[i].GetComponent<SpriteRenderer>().color;
        }
    }

    public void CallOnPress()
    {
        OnPress.Invoke();
    }

    public void SwitchAllGravityZone()
    {
        foreach (GravityZone gz in m_gravityZones)
        {
            gz.SwitchGravity();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_ObjOnTop += 1;

        if ((m_ObjOnTop == RequiredAmount) && !m_isActive)
        {
            foreach (GameObject obj in m_DisplayLight)
            {
                if (obj.TryGetComponent(out SpriteRenderer sprite))
                {
                    sprite.color = m_DisplayLightOnColor;
                }
            }

            m_isActive = true;
            CallOnPress();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_ObjOnTop -= 1;

        if (m_ObjOnTop == 0)
        {
            for (int i = 0; i < m_DisplayLight.Length; i++)
            {
                m_DisplayLight[i].GetComponent<SpriteRenderer>().color = m_DisplayLightOgColor[i];
            }

            m_isActive = false;
            OnOut.Invoke();
        }
    }




}
