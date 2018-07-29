using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using DG.Tweening;

public class ShrinkGrow : MonoBehaviour
{
    public enum SizeState { SMALL, MEDIUM, LARGE }
    public SizeState sizeState;

    public LayerMask sizeChangeStopingLayers;

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

    protected Character character;
    protected Rigidbody2D rb2d;

    public AudioSource soundSource;
    public AudioClip growSound;
    public AudioClip shrinkSound;

    void Start()
    {
        character = GetComponent<Character>();
        rb2d = GetComponent<Rigidbody2D>();

        //SetSizeProperties(smallProperties);
        SetSizeProperties(mediumProperties);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetSizeState(SizeState.SMALL);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetSizeState(SizeState.MEDIUM);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetSizeState(SizeState.LARGE);
        }
    }

    private bool CanPlayerGrowToThisSize(SizeProperties sizeProperties)
    {
        float height = sizeProperties.height;
        float halfHeight = height / 2;
        float width = sizeProperties.height * 0.30f;
        float halfWidth = width / 2;


        Vector2 center = (Vector2)transform.position + new Vector2(0, halfHeight);
        Vector2 size = new Vector2(width - 0.2f, height - 0.2f);
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(center, size, 0, sizeChangeStopingLayers);

        #region Debug
        const int Duration = 5;
        Vector2 halfSize = new Vector2(halfWidth, halfHeight);

        Vector2 topLeftCorner = new Vector2(center.x - halfSize.x, center.y + halfSize.y);
        Vector2 topRightCorner = new Vector2(center.x + halfSize.x, center.y + halfSize.y);
        Vector2 bottomLeftCorner = new Vector2(center.x - halfSize.x, center.y - halfSize.y);
        Vector2 bottomRightCorner = new Vector2(center.x + halfSize.x, center.y - halfSize.y);

        Debug.DrawLine(topLeftCorner, topRightCorner, Color.gray, Duration);
        Debug.DrawLine(topRightCorner, bottomRightCorner, Color.gray, Duration);
        Debug.DrawLine(bottomRightCorner, bottomLeftCorner, Color.gray, Duration);
        Debug.DrawLine(bottomLeftCorner, topLeftCorner, Color.gray, Duration);
        #endregion

        for (int i = 0; i < hitColliders.Length; ++i)
        {
            if (!hitColliders[i].CompareTag("Player") && !hitColliders[i].CompareTag("PlayerHead"))
                return false;
        }
        return true;
    }

    private void SetSizeState( SizeState sizeState)
    {
        if (this.sizeState < sizeState)
        {
            soundSource.clip = growSound;
            soundSource.Play();
        }
        else if (sizeState < this.sizeState)
        {
            soundSource.clip = shrinkSound;
            soundSource.Play();
        }

        switch (sizeState)
        {
            case SizeState.SMALL:
                if (CanPlayerGrowToThisSize(smallProperties))
                {
                    SetSizeProperties(smallProperties);

                    this.sizeState = SizeState.SMALL;
                }
                break;
            case SizeState.MEDIUM:
                if (CanPlayerGrowToThisSize(mediumProperties))
                {
                    SetSizeProperties(mediumProperties);
                    this.sizeState = SizeState.MEDIUM;
                }
                break;
            case SizeState.LARGE:
                if (CanPlayerGrowToThisSize(largeProperties))
                {
                    SetSizeProperties(largeProperties);
                    this.sizeState = SizeState.LARGE;
                }
                break;
        }
    }

    private void SetSizeProperties(SizeProperties sizeProperties)
    {
        rb2d.mass = sizeProperties.mass;
        character.maxSpeed = sizeProperties.maxSpeed;
        //transform.localScale = new Vector2(sizeProperties.height, sizeProperties.height);
        transform.DOScale(new Vector2(sizeProperties.height, sizeProperties.height),.3f);
        character.jumpSpeed = Mathf.Sqrt(sizeProperties.jumpHeight * 2 * Physics2D.gravity.magnitude);
        //Camera.main.orthographicSize = sizeProperties.cameraHalfHeight;
        Camera.main.DOOrthoSize(sizeProperties.cameraHalfHeight, 0.3f);
    }
}
