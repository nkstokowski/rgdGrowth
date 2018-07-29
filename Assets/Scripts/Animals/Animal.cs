using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Animal : MonoBehaviour
{
    [Serializable]
    public struct AnimalParameters
    {
        public bool canBeCrushedByPlayer;
        public ShrinkGrow.SizeState minimumSizeToCrush;
        public bool damagesPlayer;
        public ShrinkGrow.SizeState minimumSizeToDamagePlayer;
        public ShrinkGrow.SizeState maximumSizeToDamagePlayer;
    }
    public AnimalParameters animalParameters;

    protected Rigidbody2D rb2d;
    protected Collider2D[] colliders;
    protected SpriteRenderer[] spriteRenderers;

    protected virtual void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    protected virtual void Kill()
    {
        enabled = false;

        for (int i = 0; i < colliders.Length; ++i)
        {
            colliders[i].enabled = false;
        }

        Color color;
        for (int i = 0; i < spriteRenderers.Length; ++i)
        {
            color = spriteRenderers[i].color;
            color.r *= 0.50f;
            color.g *= 0.50f;
            color.b *= 0.50f;
            color.a *= 0.25f;
            spriteRenderers[i].color = color;
        }

        if (rb2d != null)
            rb2d.isKinematic = false;

        transform.position += Vector3.forward * 100;

        Destroy(gameObject, 2.0f);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null)
            OnTriggerEnter2D(collision.collider);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if ((animalParameters.canBeCrushedByPlayer || animalParameters.damagesPlayer) && other.CompareTag("Player"))
        {
            ShrinkGrow.SizeState sizeState = other.GetComponentInParent<ShrinkGrow>().sizeState;

            if (animalParameters.canBeCrushedByPlayer
                && sizeState >= animalParameters.minimumSizeToCrush
                && other.bounds.min.y > transform.position.y)
            {
                Kill();
            }

            if (animalParameters.damagesPlayer
                && (animalParameters.minimumSizeToDamagePlayer <= sizeState && sizeState <= animalParameters.maximumSizeToDamagePlayer))
            {
                Character.Player.DamagePlayer();
            }
        }
    }
}