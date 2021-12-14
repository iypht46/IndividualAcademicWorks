using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public enum FadeType { FadeIn = 0, FadeOut};

    [SerializeField] GameObject m_Pause_ui;


    [SerializeField] Image m_FadeImage;
    [SerializeField] float m_FadeTime = 1;


    public bool m_Fading = false;
    public bool m_FadeFinish = false;
    FadeType m_Type;

    float i;

    bool m_pause = false;

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (m_Fading)
        {
            if (m_Type == FadeType.FadeIn)
            {
                i -= Time.deltaTime;
                m_FadeImage.color = new Color(m_FadeImage.color.r, m_FadeImage.color.g, m_FadeImage.color.b, i / m_FadeTime);

                if (i <= 0)
                {
                    m_FadeFinish = true;
                    m_Fading = false;
                }
            }
            else
            {
                i += Time.deltaTime;
                m_FadeImage.color = new Color(m_FadeImage.color.r, m_FadeImage.color.g, m_FadeImage.color.b, i / m_FadeTime);

                if (i >= m_FadeTime)
                {
                    m_FadeFinish = true;
                    m_Fading = false;
                }
            }
        }

        if (!m_Fading)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_pause = !m_pause;

                if (m_pause)
                {
                    Time.timeScale = 0;
                    m_Pause_ui.SetActive(true);
                }
                else
                {
                    Time.timeScale = 1;
                    m_Pause_ui.SetActive(false);
                }
            }
        }
    }


    public void Fade(FadeType fadeType)
    {
        m_Type = fadeType;
        m_Fading = true;
        m_FadeFinish = false;

        if (m_Type == FadeType.FadeIn)
        {
            i = m_FadeTime;
        }
        else
        {
            i = 0;
        }
    }

    public bool isFinish()
    {
        return m_FadeFinish;
    }
}
