using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private float moveInput;

    private Animator anim;
    public Transform spriteChild;
    private PlayerSFX sfx;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private float groundedTimeBuffer = 0f;
    public float groundedDelay = 0.05f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sfx = GetComponent<PlayerSFX>();
    }

    void Update()
    {

        // Movement input
        moveInput = Input.GetAxis("Horizontal");

        // Flip sprite
        Vector3 scale = spriteChild.localScale;
        if (moveInput > 0.01f)
            scale.x = Mathf.Abs(scale.x);
        else if (moveInput < -0.01f)
            scale.x = -Mathf.Abs(scale.x);

        spriteChild.localScale = scale;

      

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            sfx.PlayJump();
            anim.SetBool("isJumping", true);
        }

        // Play footstep sounds
        if (isGrounded && Mathf.Abs(moveInput) > 0.01f)
        {
            sfx.PlayWalk();
        }

        bool checkGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (checkGround)
        {
            groundedTimeBuffer = groundedDelay;  // reset timer when touching ground
        }
        else
        {
            groundedTimeBuffer -= Time.deltaTime;
        }

        isGrounded = groundedTimeBuffer > 0f;

        anim.SetBool("isJumping", !isGrounded);

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        anim.SetFloat("X Velocity", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Y Velocity", rb.velocity.y);
    }
}
