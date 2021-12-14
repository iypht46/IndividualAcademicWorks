using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGroup : MonoBehaviour
{
    private static WaypointGroup m_instance;
    public static WaypointGroup Instance { get { return m_instance; } }

    [SerializeField] private GameObject waypointUI;

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_instance = this;
        }
    }

    public GameObject CreateWaypoint()
    {
        GameObject wp = Instantiate(waypointUI, this.transform);

        wp.GetComponent<WaypointUI>().waypointGroup = this.gameObject;
        wp.SetActive(false);

        return wp;
    }
}
