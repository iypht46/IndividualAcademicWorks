using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    [SerializeField] Slider PlayerHP;
    [SerializeField] Slider PlayerVP;
    [SerializeField] Image PlayerShieldIcon;

    [SerializeField] GameObject EnemyUIGroup;

    [SerializeField] Slider EnemyHP;
    [SerializeField] Slider EnemyVP;
    [SerializeField] Image EnemyShieldIcon;


    [SerializeField] GameObject lockTarget;

    GameObject Player;

    LockingSystem lockingSystem;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        lockingSystem = Player.GetComponent<LockingSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHP.value = Player.GetComponent<hpSystem>().GetHPRatio();
        PlayerVP.value = Player.GetComponent<CombatSystem>().GetVPRatio();

        if (Player.GetComponent<CombatSystem>().isVulnarable)
        {
            PlayerShieldIcon.color = Color.red;
        }
        else
        {
            PlayerShieldIcon.color = Color.white;
        }

        if (lockTarget != null)
        {
            EnemyHP.value = lockTarget.GetComponent<hpSystem>().GetHPRatio();
            EnemyVP.value = lockTarget.GetComponent<CombatSystem>().GetVPRatio();

            if (lockTarget.GetComponent<CombatSystem>().isVulnarable)
            {
                EnemyShieldIcon.color = Color.red;
            }
            else
            {
                EnemyShieldIcon.color = Color.white;
            }

            EnemyUIGroup.SetActive(true);
        }
        else
        {
            EnemyUIGroup.SetActive(false);
        }
    }
}
