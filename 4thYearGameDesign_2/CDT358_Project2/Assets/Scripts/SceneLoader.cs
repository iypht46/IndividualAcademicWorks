using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string SceneName;

    public void LoadScene(string scenename)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(scenename);
    }

    public void LoadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneName);
    }


}
