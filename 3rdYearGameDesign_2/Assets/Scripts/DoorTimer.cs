using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTimer : MonoBehaviour
{
    [SerializeField] GameObject m_Fill;
    [SerializeField] float m_TimeRequire;
    [SerializeField] SceneLoader m_SL;
    [SerializeField] Fader m_Fader;

    bool m_isCounting;
    bool m_stageProceed = false;

    float T;

    float m_FillMaxScale;

    void Start()
    {
        T = 0;
        m_FillMaxScale = m_Fill.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (!m_stageProceed)
            {
                m_Fader.Fade(Fader.FadeType.FadeOut);
                SoundPlayer.Instance.PlaySFX(2);
            }

            m_stageProceed = true;
        }

        if (m_isCounting)
        {
            T += Time.deltaTime;

            if (T >= m_TimeRequire)
            {
                if (!m_stageProceed)
                {
                    m_Fader.Fade(Fader.FadeType.FadeOut);
                    SoundPlayer.Instance.PlaySFX(2);
                }

                m_stageProceed = true;
            }
        }
        else
        {
            T = 0;
        }

        float NewScale = (T * m_FillMaxScale) / m_TimeRequire;

        if (NewScale > m_FillMaxScale)
        {
            NewScale = m_FillMaxScale;
        }

        m_Fill.transform.localScale = new Vector3(NewScale, m_Fill.transform.localScale.y, m_Fill.transform.localScale.z);

        if (m_stageProceed)
        {
            if (m_Fader.isFinish())
            {
                m_stageProceed = false;
                m_SL.LoadScene();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_isCounting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_isCounting = false;
        }
    }
}
