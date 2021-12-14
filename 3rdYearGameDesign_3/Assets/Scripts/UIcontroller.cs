using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontroller : MonoBehaviour
{
    [SerializeField] GameObject UI;

    public void SetUI(bool active)
    {
        UI.gameObject.SetActive(active);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
