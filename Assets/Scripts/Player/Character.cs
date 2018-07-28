using System;
using UnityEngine;


public class Character : MonoBehaviour
{
    [SerializeField] public float maxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] public float jumpSpeed = 400f;                  // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    public Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    [SerializeField] private bool isGrounded;            // Whether or not the player is grounded.
    public Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        isGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.05f, m_WhatIsGround);
        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].collider.CompareTag("Player"))
                isGrounded = true;
        }
    }


    public void Move(float move, bool jump)
    {
        //only control the player if grounded or airControl is turned on
        if (isGrounded || m_AirControl)
        {
            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * maxSpeed, m_Rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (isGrounded && jump)
        {
            // Add a vertical force to the player.
            isGrounded = false;
            m_Rigidbody2D.velocity += new Vector2(0f, jumpSpeed);
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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
        }

    }
}
