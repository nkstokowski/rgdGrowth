using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    public float upwardForceInWater = 10;
    protected Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        WaterZone waterZone = collision.GetComponentInParent<WaterZone>();

        if (waterZone)
        {
            rb2d.gravityScale = 0;
            rb2d.velocity = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        WaterZone waterZone = collision.GetComponentInParent<WaterZone>();

        if (waterZone)
        {
            rb2d.gravityScale = 1.0f;
        }
    }
}
