using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChecker : MonoBehaviour
{
    [SerializeField] bool m_PlayTutorial = false;
    public bool isTutorial { get { return m_PlayTutorial; } }

    Player m_player;
    PlayerMovement m_movement;

    [SerializeField] float m_InputDelay = 1f;
    float T_input = 0;

    [SerializeField] int m_RequireModule = 3;

    bool m_FinishedMovement = false;
    bool m_FinishedRotate = false;
    bool m_FinishedAttach = false;
    bool m_FinishedDetach = false;

    [SerializeField] GameObject m_TutorialUI;
    [SerializeField] GameObject[] m_SequenceUIs;

    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        m_PlayTutorial = SoundPlayer.Instance.playTutorial;


    }

    void Update()
    {
        m_TutorialUI.SetActive(m_PlayTutorial);

        if (!m_PlayTutorial)
            return;

        if (!m_FinishedMovement)
        {
            m_SequenceUIs[0].SetActive(true);

            if (m_movement.MoveDir.magnitude != 0)
            {
                T_input += Time.deltaTime;
            }

            if (T_input > m_InputDelay)
            {
                T_input = 0;
                m_FinishedMovement = true;

                m_SequenceUIs[0].SetActive(false);
            }

            Debug.Log("Checking Movement !!");
            return;
        }

        if (!m_FinishedRotate)
        {
            m_SequenceUIs[1].SetActive(true);

            if (m_movement.RotateDir != 0)
            {
                T_input += Time.deltaTime;
            }

            if (T_input > m_InputDelay)
            {
                T_input = 0;
                m_FinishedRotate = true;

                m_SequenceUIs[1].SetActive(false);
            }

            Debug.Log("Checking Rotate !!");
            return;
        }

        if (!m_FinishedAttach)
        {
            m_SequenceUIs[2].SetActive(true);

            if (m_player.GetTotalModule() >= m_RequireModule)
            {
                m_FinishedAttach = true;

                m_SequenceUIs[2].SetActive(false);
            }

            Debug.Log("Checking Attach !!");
            return;
        }

        if (!m_FinishedDetach)
        {
            m_SequenceUIs[3].SetActive(true);

            if (m_player.GetTotalModule() == 0)
            {
                m_FinishedDetach = true;

                m_SequenceUIs[3].SetActive(false);
            }

            Debug.Log("Checking Detach !!");
            return;
        }

        if (m_FinishedDetach)
            m_PlayTutorial = false;

    }
}
