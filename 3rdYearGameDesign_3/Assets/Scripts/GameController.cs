using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController m_instance;
    public static GameController Instance { get { return m_instance; } }

    [SerializeField] int m_number_init_block = 2;

    [SerializeField] Text m_score_text;

    [SerializeField] GameObject m_Ending;
    [SerializeField] Text m_Ending_Score_Text;
    [SerializeField] Text m_Ending_Time_Text;
    [SerializeField] Text m_Ending_Coin_Text;
    [SerializeField] Text m_Ending_Block_Text;
    [SerializeField] Text m_Ending_Kill_Text;

    [SerializeField] GameObject m_Pause;

    [SerializeField] float m_Player_OutOfBound_Time = 2f;

    int m_score = 0;
    float m_time = 0;

    int m_yellow = 0;
    int m_red = 0;

    public int m_block = 0;
    public int m_kill = 0;

    float m_oobTime = 0;

    public bool m_isEnd = false;
    public bool m_isPause = false;

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

    void Start()
    {
        for(int i = 0;i < m_number_init_block; i++)
        {
            BlockSpawner.Instance.SpawnBlock();
        }

        m_score = 0;
        m_time = 0;
        m_yellow = 0;
        m_red = 0;
        m_block = 0;
        m_isEnd = false;
        m_Ending.SetActive(false);
    }

    void Update()
    {
        if (!m_isEnd)
        {
            m_time += Time.deltaTime;

            m_score_text.text = "Score: " + m_score.ToString();
            m_Ending_Score_Text.text = "Score: " + m_score.ToString();
            m_Ending_Time_Text.text = "Time: " + m_time.ToString();
            m_Ending_Coin_Text.text = "Red: " + m_red.ToString() + " Yellow: " + m_yellow.ToString();
            m_Ending_Block_Text.text = "Block: " + m_block.ToString();
            m_Ending_Kill_Text.text = "Kill: " + m_kill.ToString();

            if (PlayerController.Instance.m_OutOfBound)
            {
                m_oobTime += Time.deltaTime;

                if (m_oobTime > m_Player_OutOfBound_Time)
                {
                    m_isEnd = true;

                    m_Ending.SetActive(true);

                }
            }
            else
            {
                m_oobTime = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

    }

    public void AddScore(int add)
    {
        m_score += add;

        if (add == 50)
        {
            m_red++;
        }
        else if (add == 5)
        {
            m_yellow++;
        }
    }

    public void ResetScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void TogglePause()
    {
        m_isPause = !m_isPause;

        if (m_isPause)
        {
            Time.timeScale = 0;
            m_Pause.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            m_Pause.SetActive(false);
        }
    }
}
