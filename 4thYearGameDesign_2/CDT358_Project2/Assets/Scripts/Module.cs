using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    public bool isDepleted = true;

    public bool Attachable = true;

    [SerializeField] float m_DisableTime = 5.0f;

    float Tdisable = 0;

    [SerializeField] float m_BlinkTime = 0.1f;

    float TBlink = 0;

    [SerializeField] float m_StartBlinking = 3.0f;

    Renderer[] renderers;

    // Start is called before the first frame update
    void Start()
    {
        renderers = gameObject.GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Body_Module") && isDepleted)
        {
            Tdisable += Time.deltaTime;

            if (Tdisable > m_StartBlinking)
            {
                TBlink += Time.deltaTime;

                if (TBlink > m_BlinkTime)
                {
                    TBlink = 0;
                    ToggleRenderer();
                }
            }

            if (Tdisable > m_DisableTime)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            SetRenderer(true);
            Tdisable = 0;
        }
        
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
