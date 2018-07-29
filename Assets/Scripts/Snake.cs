using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour, IFlammable
{
    public Transform firePosition;
    public bool fireRight;
    public float fireInterval;
    public float timeLastFired = 0;
    public float fireBallSpeed = 1.75f;

    public void HandleFire()
    {
        Destroy(gameObject, 2);
        this.enabled = false;

        GameManager.Instance.SpawnFireEffect(transform.position, transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timeLastFired > fireInterval)
        {
            GameManager.Instance.LaunchFireBall(firePosition.position, fireRight ? Vector2.right : Vector2.left, fireBallSpeed, FireBall.Team.DAMAGES_PLAYER);
            timeLastFired = Time.time;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponentInParent<ShrinkGrow>().sizeState == ShrinkGrow.SizeState.LARGE)
            {
                Destroy(gameObject);
            }
        }
    }
}
