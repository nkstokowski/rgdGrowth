using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spider : MonoBehaviour, IFlammable
{
    public float maxDistance = 20;
    public float speed = 1;
    public bool waitForPlayer;
    private LineRenderer lineRenderer;
    private Vector2 startPosition;

    private void Awake()
    {
        startPosition = transform.position;

        lineRenderer = GetComponentInChildren<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, lineRenderer.transform.position);
        lineRenderer.SetPosition(1, startPosition);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 target = GetTargetPosition();

        transform.position = transform.position + ((Vector3)target - transform.position).normalized * speed * Time.deltaTime;

        lineRenderer.SetPosition(1, lineRenderer.transform.position);
    }

    private Vector2 GetTargetPosition()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition, Vector2.down, maxDistance);

        for (int i = 0; i < hits.Length; ++i)
        {
            if (waitForPlayer)
            {
                if (hits[i].collider.CompareTag("Player"))
                {
                    return hits[i].point;
                }
            }
            else if (hits[i].collider.gameObject != gameObject)
            {
                return hits[i].point;
            }
        }

        if (waitForPlayer)
        {
            return startPosition;
        }
        else
        {
            return startPosition + Vector2.down * maxDistance;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }

    public void HandleFire()
    {
        Destroy(gameObject, 2);
        this.enabled = false;

        GameManager.Instance.SpawnFireEffect(transform.position, transform.localScale);
    }
}
