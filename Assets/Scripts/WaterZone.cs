using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHead"))
        {
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }
}
