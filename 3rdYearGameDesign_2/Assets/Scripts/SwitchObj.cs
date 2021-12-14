using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SwitchObj : MonoBehaviour
{
    [SerializeField] UnityEvent OnPress;

    GravityZone[] m_gravityZones;

    private void Start()
    {
        m_gravityZones = FindObjectsOfType<GravityZone>() as GravityZone[];
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

}
