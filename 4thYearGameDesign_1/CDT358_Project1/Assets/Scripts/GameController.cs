using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField] Text BigText;

    [SerializeField] GameObject Player;
    [SerializeField] GameObject Enemy;

    [SerializeField] PlayerInput playerInput;

    bool isPause = false;

    public bool isEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        isPause = false;
        isEnd = false;

        playerInput.enabled = true;

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;

            Cursor.visible = isPause;

            BigText.gameObject.SetActive(isPause);
            BigText.text = "Pause";
            BigText.color = Color.white;

            playerInput.enabled = !isPause;

            if (isPause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        if (Player.GetComponent<hpSystem>().isDead)
        {
            isEnd = true;
            Cursor.visible = true;
            playerInput.enabled = false;

            BigText.gameObject.SetActive(true);
            BigText.text = "Defeat";
            BigText.color = Color.red;
        }
        else if(Enemy.GetComponent<hpSystem>().isDead)
        {
            isEnd = true;
            Cursor.visible = true;
            playerInput.enabled = false;

            BigText.gameObject.SetActive(true);
            BigText.color = Color.yellow;
            BigText.text = "Victory"; 
        }

    }
}
