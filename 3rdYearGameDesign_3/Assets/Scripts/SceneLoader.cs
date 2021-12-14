using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string SceneName;

    public void LoadScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneName);
    }


}
