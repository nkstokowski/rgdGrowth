using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    public string SceneName = "level1";

    public void StartGame()
    {
        SceneManager.LoadScene(SceneName);
    }
}
