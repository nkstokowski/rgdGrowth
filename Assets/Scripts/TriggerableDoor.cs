using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableDoor : MonoBehaviour {
    public Collider2D[] doorColliders;
    public SpriteRenderer doorSprite;

    public void Awake()
    {
        doorColliders = GetComponentsInChildren<Collider2D>();
    }

    public void OpenDoor()
    {
        for (int i = 0; i < doorColliders.Length; ++i)
        {
            doorColliders[i].enabled = false;
        }
        doorSprite.color = new Color(0.5f, 0.5f, 0.5f, 0.25f);
    }

    public void CloseDoor()
    {
        for (int i = 0; i < doorColliders.Length; ++i)
        {
            doorColliders[i].enabled = true;
        }
        doorSprite.color = Color.white;
    }
}
