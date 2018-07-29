using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bee : Animal, IFlammable
{
    Collider2D playerCollider;
    public float radius = 2;
    public float speed = 0;
    public ShrinkGrow.SizeState maxSizeState = ShrinkGrow.SizeState.SMALL;

    protected override void Awake()
    {
        base.Awake();

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

    public void HandleFire()
    {
        Destroy(gameObject, 2);
        this.enabled = false;

        GameManager.Instance.SpawnFireEffect(transform.position, transform.localScale);
    }
}
