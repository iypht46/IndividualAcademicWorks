using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpUI : MonoBehaviour
{
    [SerializeField] GameObject[] m_HPs;

    [SerializeField] GameObject[] m_Life;

    hpSystem m_PlayerHP;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<hpSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHP();

        UpdateLife();
    }

    void UpdateHP()
    {
        int CurrHp = (int) m_PlayerHP.HP;

        foreach (GameObject hp in m_HPs)
        {
            hp.SetActive(false);
        }

        for(int i = 0; i < CurrHp; i++)
        {
            m_HPs[i].SetActive(true);
        }
    }

    void UpdateLife()
    {
        if (m_Life.Length == 0)
            return;

        int CurrLife = GameController.Instance.TotalLife;

        foreach (GameObject life in m_Life)
        {
            life.SetActive(false);
        }

        for (int i = 0; i < CurrLife; i++)
        {
            m_Life[i].SetActive(true);
        }
    }
}
