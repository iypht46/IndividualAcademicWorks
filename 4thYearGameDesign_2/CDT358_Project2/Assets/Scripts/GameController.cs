using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController m_instance;
    public static GameController Instance { get { return m_instance; } }

    [SerializeField] float m_WaveDelay = 5.0f;
    [SerializeField] float m_WaveDelayCount = 0.0f;

    [SerializeField] int m_EnemyStartAmount = 3;
    int m_EnemyAmount = 0;
    int m_EnemyAmountDist = 0;

    [SerializeField] int m_RangeChange = 3;
    [SerializeField] int m_DecreaseWhenChange = 3;

    [SerializeField] int m_WaveNumber = 0;

    [SerializeField] Text m_WaveText;
    [SerializeField] Text m_TopText;
    [SerializeField] Text m_BottomText;

    [SerializeField] int m_IncreaseEnemy = 1;
    [SerializeField] int m_IncreaseHP = 1;


    [SerializeField] Text BigText;
    [SerializeField] Text BigWaveText;
    [SerializeField] Text BigTimeText;
    bool isPause = false;
    public bool isEnd = false;
    public bool isBreak = false;
    float m_GameTime = 0f;



    [Header("PlayerRespawn")]
    [SerializeField] int m_TotalLife = 3;
    [SerializeField] float m_RespawnDelay = 0.5f;
    float TRes = 0;

    public int TotalLife { get { return m_TotalLife; } }

    hpSystem m_PlayerHP;

    bool hasClearList = false;
    int rangeChangeCount = 0;

    TutorialChecker m_tutorialChecker;


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
        m_EnemyAmount = m_EnemyStartAmount;

        m_WaveDelayCount = m_WaveDelay;

        m_PlayerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<hpSystem>();
        m_tutorialChecker = GameObject.FindGameObjectWithTag("Player").GetComponent<TutorialChecker>();

        Cursor.visible = false;
        isPause = false;
        isEnd = false;

        Time.timeScale = 1;

        m_GameTime = 0;
    }

    void Update()
    {
        if (m_tutorialChecker.isTutorial)
        {
            m_TopText.gameObject.SetActive(false);
            m_BottomText.gameObject.SetActive(false);
        }

        if (!m_tutorialChecker.isTutorial)
            WaveControl();

        if(!isEnd && !m_tutorialChecker.isTutorial)
            m_GameTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;

            Cursor.visible = isPause;

            BigText.gameObject.SetActive(isPause);
            BigText.text = "Pause";
            BigText.color = Color.yellow;

            BigWaveText.text = "Wave " + m_WaveNumber;
            BigWaveText.color = Color.yellow;

            BigTimeText.text = "Total time = " + m_GameTime.ToString("#.00") + "s";
            BigTimeText.color = Color.yellow;

            if (isPause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        if (m_PlayerHP.isDead)
        {
            if (m_TotalLife <= 1)
            {
                m_TotalLife--;

                isEnd = true;
                Cursor.visible = true;

                BigText.gameObject.SetActive(true);
                BigText.text = "Defeat";
                BigText.color = Color.red;

                BigWaveText.text = "Wave " + m_WaveNumber;
                BigWaveText.color = Color.red;

                BigTimeText.text = "Total time = " + m_GameTime.ToString("#.00") + "s";
                BigTimeText.color = Color.red;
            }
            else
            {
                TRes += Time.deltaTime;

                if (TRes > m_RespawnDelay)
                {
                    TRes = 0;

                    m_TotalLife--;
                    m_PlayerHP.Respawn();
                    m_PlayerHP.GetComponent<Player>().m_Blinking = true;
                }
            }
        }
    }

    void WaveControl()
    {
        m_TopText.gameObject.SetActive(true);
        m_BottomText.gameObject.SetActive(true);

        m_WaveText.text = m_WaveNumber.ToString();

        if (EnemySpawner.Instance.AllDead() && EnemySpawner.Instance.Spawn)
        {
            isBreak = true;

            if (!hasClearList)
            {
                m_WaveNumber++;
                hasClearList = true;
                EnemySpawner.Instance.ClearList();

                m_PlayerHP.ModifyHPValue(m_IncreaseHP);
            }

            m_WaveDelayCount -= Time.deltaTime;

            m_TopText.text = "Prepare for the next wave !!";
            m_BottomText.text = m_WaveDelayCount.ToString("#.00") + "s";

            if (m_WaveDelayCount <= 0)
            {
                /*if(m_WaveNumber != 1)
                    RemoveUnuseModule();*/

                m_WaveDelayCount = m_WaveDelay;
                rangeChangeCount++;

                if (rangeChangeCount == m_RangeChange)
                {
                    rangeChangeCount = 0;

                    if (EnemySpawner.Instance.IncreaseEnemyRange())
                        m_EnemyAmount -= m_DecreaseWhenChange;

                    Debug.Log("Range Change !!");

                    EnemySpawner.Instance.SpawnBoss();

                    m_EnemyAmountDist = 1;
                    return;
                }

                m_EnemyAmount++;
                m_EnemyAmountDist = m_EnemyAmount;
                EnemySpawner.Instance.SetSpawnNumber(m_EnemyAmount);
            }
        }
        else
        {
            isBreak = false;

            m_TopText.text = "Destroy the Enemy";
            m_BottomText.text = EnemySpawner.Instance.GetNumberEnemy(false).ToString() + "/" + m_EnemyAmountDist.ToString();

            hasClearList = false;
            m_WaveDelayCount = m_WaveDelay;
        }
    }

    void RemoveUnuseModule()
    {
        GameObject[] modules = GameObject.FindGameObjectsWithTag("Module");

        foreach(GameObject obj in modules)
        {
            if (obj.layer == LayerMask.NameToLayer("Body_Module"))
            {
                Destroy(obj);
            }
        }
    }
}
