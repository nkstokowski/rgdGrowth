﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    public string SceneName = "Level1Test";

    public void StartGame()
    {
        SceneManager.LoadScene(SceneName);
    }
}
