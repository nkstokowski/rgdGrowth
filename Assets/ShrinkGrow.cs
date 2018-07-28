using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class ShrinkGrow : MonoBehaviour
{
    public enum SizeState { SMALL, MEDIUM, LARGE }
    public SizeState sizeState;

    [Serializable]
    public class SizeProperties
    {
        public float mass;
        public float maxSpeed;
        public float height;
        public float jumpHeight;
        public float cameraHalfHeight;
    }
    public SizeProperties smallProperties;
    public SizeProperties mediumProperties;
    public SizeProperties largeProperties;

    protected PlatformerCharacter2D character;
    protected Rigidbody2D rb2d;

    void Start()
    {
        character = GetComponent<PlatformerCharacter2D>();
        rb2d = GetComponent<Rigidbody2D>();

        SetSizeProperties(smallProperties);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            sizeState = SizeState.SMALL;
            SetSizeProperties(smallProperties);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            sizeState = SizeState.MEDIUM;
            SetSizeProperties(mediumProperties);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            sizeState = SizeState.LARGE;
            SetSizeProperties(largeProperties);
        }
    }

    private void SetSizeProperties(SizeProperties sizeProperties)
    {
        rb2d.mass = sizeProperties.mass;
        character.maxSpeed = sizeProperties.maxSpeed;
        transform.localScale = new Vector2(sizeProperties.height / 1.75f, sizeProperties.height / 1.75f);
        character.jumpSpeed = Mathf.Sqrt(sizeProperties.jumpHeight * 2 * Physics2D.gravity.magnitude);
        Camera.main.orthographicSize = sizeProperties.cameraHalfHeight;
    }
}
