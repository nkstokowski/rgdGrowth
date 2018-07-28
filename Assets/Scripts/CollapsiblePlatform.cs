using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsiblePlatform : MonoBehaviour
{
    public ShrinkGrow.SizeState minimumSizeState = ShrinkGrow.SizeState.MEDIUM;
    public Collider2D[] colliders;
    public SpriteRenderer sprite;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider2D>();
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ShrinkGrow shrinkGrow = collision.collider.GetComponentInChildren<ShrinkGrow>() ?? collision.collider.GetComponentInParent<ShrinkGrow>();

            if (shrinkGrow.sizeState >= minimumSizeState)
            {
                for(int i = 0; i < colliders.Length; ++i)
                {
                    colliders[i].enabled = false;
                }

                sprite.color = new Color(0.5f, 0.5f, 0.5f, 0.25f);

                GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }
    }
}
