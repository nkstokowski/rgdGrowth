using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeightedButton : MonoBehaviour
{
    public int minimumMass = 1;
    public ShrinkGrow.SizeState minimumSize = ShrinkGrow.SizeState.MEDIUM;

    public List<Rigidbody2D> collidingObjects = new List<Rigidbody2D>();

    public GameObject buttonGraphic;

    public bool isPressed;

    public UnityEvent OnPressed;
    public UnityEvent OnUnpressed;

    private void Awake()
    {
        if (buttonGraphic != null)
        {
            const float BUTTON_DEPRESSION = 0.15f;
            OnPressed.AddListener(() =>
            {
                buttonGraphic.transform.localPosition += Vector3.down * BUTTON_DEPRESSION;
            });

            OnUnpressed.AddListener(() =>
            {
                buttonGraphic.transform.localPosition += Vector3.up * BUTTON_DEPRESSION;
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody != null)
        {
            if (collision.CompareTag("Player"))
            {
                ShrinkGrow shrinkGrow = collision.GetComponentInParent<ShrinkGrow>() ?? collision.GetComponentInChildren<ShrinkGrow>();

                if (shrinkGrow.sizeState >= minimumSize)
                {
                    Press();
                    collidingObjects.Add(collision.attachedRigidbody);
                }
            }
            else
            {
                if (collision.attachedRigidbody.mass >= minimumMass)
                {
                    Press();
                    collidingObjects.Add(collision.attachedRigidbody);
                }
            }
        }
    }

    private void Press()
    {
        if (!isPressed)
        {
            isPressed = true;
            OnPressed.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collidingObjects.Remove(collision.attachedRigidbody);

        if (isPressed && collidingObjects.Count == 0)
        {
            Unpress();
        }
    }

    private void Unpress()
    {
        if (isPressed)
        {
            isPressed = false;
            OnUnpressed.Invoke();
        }
    }
}
