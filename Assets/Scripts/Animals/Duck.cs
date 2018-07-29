using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : Animal, IFlammable
{
    public float upwardForceInWater = 10;

    public AudioClip[] audioClips;
    public float soundInterval;
    public float timeOfPreviousQuack;

    private void Update()
    {
        if (Time.time - timeOfPreviousQuack > soundInterval)
        {
            timeOfPreviousQuack = Time.time;

            if (audioClips.Length != 0)
            {
                audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
                audioSource.Play();
            }
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
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

    public void HandleFire()
    {
        Kill();

        GameManager.Instance.SpawnFireEffect(transform.position, transform.localScale);
    }
}
