using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour, IFlammable
{
    public enum PlayerState { ON_GROUND, IN_AIR, IN_AIR_AND_HOVERING }
    public PlayerState state = PlayerState.ON_GROUND;

    [SerializeField] public LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    [SerializeField] public float maxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] public float jumpSpeed = 400f;                  // Amount of force added when the player jumps.

    private Rigidbody2D rb2d;
    private bool isFacingRight = true;  // For determining which way the player is currently facing.

    [Header("Hovering")]
    public float maxHoverTime = 1.0f;
    public bool hasHoveredSinceGrounded = false;
    public float hoverLeft = 0;

    [Header("Fire")]
    public Transform fireBallLaunchPosition;
    public float fireBallSpeed = 2.0f;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Check if is on ground.
        bool isOnGround = CheckIfOnGround();

        switch (state)
        {
            case PlayerState.ON_GROUND:
                if (!isOnGround)
                {
                    state = PlayerState.IN_AIR;
                }
                break;
            case PlayerState.IN_AIR:
                if (isOnGround)
                {
                    state = PlayerState.ON_GROUND;
                    hasHoveredSinceGrounded = false;
                }
                break;
            case PlayerState.IN_AIR_AND_HOVERING:
                if (isOnGround)
                {
                    StopHovering();
                    state = PlayerState.ON_GROUND;
                    hasHoveredSinceGrounded = false;
                }
                else
                {
                    hoverLeft -= Time.fixedDeltaTime;
                    if (hoverLeft <= 0)
                    {
                        StopHovering();
                    }
                }
                break;
        }
    }

    private bool CheckIfOnGround()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.05f, m_WhatIsGround);
        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].collider.CompareTag("Player"))
                return true;
        }
        return false;
    }

    public void Move(float move, bool doJump)
    {
        // Move the character
        rb2d.velocity = new Vector2(move * maxSpeed, rb2d.velocity.y);

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (move < 0 && isFacingRight)
        {
            Flip();
        }

        // If the player should jump...
        if (state == PlayerState.ON_GROUND && doJump)
        {
            rb2d.velocity += new Vector2(0f, jumpSpeed);
            state = PlayerState.IN_AIR;
        }
        else if (state == PlayerState.IN_AIR && doJump && !hasHoveredSinceGrounded)
        {
            StartHovering();
        }
        else if (state == PlayerState.IN_AIR_AND_HOVERING && doJump)
        {
            StopHovering();
            state = PlayerState.IN_AIR;
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void StartHovering()
    {
        state = PlayerState.IN_AIR_AND_HOVERING;
        hasHoveredSinceGrounded = true;
        hoverLeft = maxHoverTime;

        Vector2 currentVelocity = rb2d.velocity;
        currentVelocity.y = 0;
        rb2d.velocity = currentVelocity;


        rb2d.gravityScale = 0;
    }

    private void StopHovering()
    {
        rb2d.gravityScale = 1;
    }

    public void Enter()
    {
        BoxCollider2D playerBox = GetComponentInChildren<BoxCollider2D>();

        Vector2 center = transform.position;
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(center, playerBox.size, 0);

        for (int i = 0; i < hitColliders.Length; ++i)
        {
            if (hitColliders[i].CompareTag("Portal"))
            {
                Portal portalCollision = hitColliders[i].GetComponent<Portal>();
                transform.position = portalCollision.destinationObject.transform.position + portalCollision.destinationOffset;
                Move(0, false);
            }
            else if (hitColliders[i].CompareTag("Exit"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    public void LaunchFireBall()
    {
        GameManager.Instance.LaunchFireBall(
            (Vector3)rb2d.position + fireBallLaunchPosition.localPosition, 
            Mathf.Sign(transform.localScale.x) == 1.0f ? Vector2.right : Vector2.left, 
            fireBallSpeed, 
            FireBall.Team.DAMAGES_ENEMY);
    }

    void IFlammable.HandleFire()
    {
        SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
    }
}
