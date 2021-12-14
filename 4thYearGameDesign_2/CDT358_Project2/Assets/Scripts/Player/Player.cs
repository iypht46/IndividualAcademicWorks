using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int m_TotalModule = 0;

    [SerializeField] float m_BlinkTotal = 5.0f;
    float TTotal = 0;

    [SerializeField] float m_BlinkTime = 0.1f;
    float TBlink = 0;

    public bool m_Blinking = false;

    Renderer[] renderers;

    hpSystem hp;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        hp = GetComponent<hpSystem>();
    }

    private void Update()
    {
       m_TotalModule = GetTotalModule();

        if (m_Blinking)
        {
            hp.invulnerable = true;

            TTotal += Time.deltaTime;

            TBlink += Time.deltaTime;

            if (TBlink > m_BlinkTime)
            {
                TBlink = 0;
                ToggleRenderer();
            }

            if (TTotal > m_BlinkTotal)
            {
                TTotal = 0;

                hp.invulnerable = false;
                m_Blinking = false;
            }
        }
        else
        {
            SetRenderer(true);
        }
    }

    public void DetachAll()
    {
        BroadcastMessage("Detach");
    }

    public int GetTotalModule()
    {
        return GetComponentsInChildren<AttachedPointGroup>().Length - 1;
    }

    void SetRenderer(bool active)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = active;
        }
    }

    void ToggleRenderer()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = !renderer.enabled;
        }
    }
}
