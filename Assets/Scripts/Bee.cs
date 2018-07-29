using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bee : MonoBehaviour
{
    Collider2D playerCollider;
    public float radius = 2;
    public float speed = 0;
    public ShrinkGrow.SizeState maxSizeState = ShrinkGrow.SizeState.SMALL;

    private void Awake()
    {
        Character player = FindObjectOfType<Character>();
        playerCollider = player.GetComponentInChildren<Collider2D>();
    }

    private void Update()
    {
        Vector2 target = playerCollider.bounds.center;

        if (Vector2.Distance(transform.position, target) < radius)
        {
            transform.position = transform.position + ((Vector3)target - transform.position).normalized * speed * Time.deltaTime;
            transform.right = ((Vector3)target - transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShrinkGrow shrinkGrow = other.GetComponentInParent<ShrinkGrow>() ?? other.GetComponentInChildren<ShrinkGrow>();
            if (shrinkGrow != null)
            {
                if (shrinkGrow.sizeState <= maxSizeState)
                {
                    SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
                }
            }
        }
    }
}
