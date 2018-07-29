using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit_Button : MonoBehaviour
{
    public string SceneName = "Start Screen";

    public void StartGame()
    {
        SceneManager.LoadScene(SceneName);
    }
}
