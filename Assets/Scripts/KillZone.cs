using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            RestartLevel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.CompareTag("Player"))
        {
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(KillZone))]
public class KillZoneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Add this to a hierarchy with colliders.");
        EditorGUILayout.LabelField("When a player collides, the level restarts.");
    }
}
#endif