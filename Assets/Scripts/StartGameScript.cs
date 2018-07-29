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
    public string CreditsName = "Credits";

    public void Credits()
    {
        SceneManager.LoadScene(CreditsName);
    }
}
