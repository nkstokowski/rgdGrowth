using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour, IFlammable
{
    public static Character Player { get; private set; }

    public enum PlayerState { ON_GROUND, IN_AIR, IN_AIR_AND_HOVERING }
    public PlayerState state = PlayerState.ON_GROUND;

    [SerializeField] public LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    [SerializeField] public float maxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] public float jumpSpeed = 400f;                  // Amount of force added when the player jumps.

    private Rigidbody2D rb2d;
    private bool isFacingRight = true;  // For determining which way the player is currently facing.
    private SpriteRenderer sr;
    private Animator anim;

    [Header("Hovering")]
    public float maxHoverTime = 1.0f;
    public bool hasHoveredSinceGrounded = false;
    public float hoverLeft = 0;
    public AudioClip hoverSound;

    [Header("Fire")]
    public Transform fireBallLaunchPosition;
    public float fireBallSpeed = 2.0f;
    public AudioClip fireBallSound;

    [Header("Water Splash")]
    public float wateringRadius = 2.0f;
    public AudioClip waterSplashSound;

    [Header("Audio Sources")]
    public AudioSource abilitySoundSource;

    private void Awake()
    {
        Player = this;

        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
        anim.SetFloat("speed", Mathf.Abs(rb2d.velocity.x));

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

        // Fixes paper mario turn 
        sr.flipX = !isFacingRight;

        // Multiply the player's x local scale by -1.
        //Vector3 theScale = transform.localScale;
        //theScale.x *= -1;
        //transform.localScale = theScale;
    }

    private void StartHovering()
    {        
        state = PlayerState.IN_AIR_AND_HOVERING;
        hasHoveredSinceGrounded = true;
        hoverLeft = maxHoverTime;

        Vector2 currentVelocity = rb2d.velocity;
        currentVelocity.y = 0;
        rb2d.velocity = currentVelocity;

        anim.SetBool("hovering", true);
        rb2d.gravityScale = 0;

        abilitySoundSource.clip = hoverSound;
        abilitySoundSource.Play();
    }

    private void StopHovering()
    {
        rb2d.gravityScale = 1;
        anim.SetBool("hovering", false);
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
        StartCoroutine("AnimShoot");

        GameManager.Instance.LaunchFireBall(
            fireBallLaunchPosition.position,
            isFacingRight ? Vector2.right : Vector2.left, 
            fireBallSpeed, 
            FireBall.Team.DAMAGES_ANIMAL);

        abilitySoundSource.clip = fireBallSound;
        abilitySoundSource.Play();
    }

    private IEnumerator AnimShoot()
    {
        anim.SetBool("shooting", true);
        yield return new WaitForSeconds(2);
        anim.SetBool("shooting", false);

    }
    public void WaterPlants()
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(wateringRadius, 0.5f), 0);
        for (int i = 0; i < nearbyColliders.Length; ++i)
        {
            IWaterable waterable =
                nearbyColliders[i].GetComponentInParent<IWaterable>() ?? nearbyColliders[i].GetComponentInChildren<IWaterable>();
            if (waterable != null)
            {
                waterable.HandleWatering();
            }
        }

        for (float f = -wateringRadius; f <= wateringRadius; f += 0.2f)
        {
            GameManager.Instance.SpawnWaterEffect(transform.position + Vector3.right * f, Vector2.one);
        }

        abilitySoundSource.clip = waterSplashSound;
        abilitySoundSource.Play();
    }

    public void DamagePlayer()
    {
        SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
    }

    void IFlammable.HandleFire()
    {
        SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
    }
}
