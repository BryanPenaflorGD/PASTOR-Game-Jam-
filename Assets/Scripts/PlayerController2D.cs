using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController2D : MonoBehaviour
{

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private bool grounded;
    private bool wasGrounded;

    private Animator anim;
    private Rigidbody2D body;

    public Transform spriteChild;

    [SerializeField] private Transform spriteTransform;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        wasGrounded = isGrounded;
        Vector3 scale = spriteChild.localScale;

        moveInput = Input.GetAxis("Horizontal");
        //if (moveInput > 0.01f)
        //    spriteTransform.localRotation = Quaternion.Euler(0, 0, 0);
        //else if (moveInput < -0.01f)
        //    spriteTransform.localRotation = Quaternion.Euler(0, 180, 0);

        if (moveInput > 0.01f)
            scale.x = Mathf.Abs(scale.x);
        else if (moveInput < -0.01f)
            scale.x = -Mathf.Abs(scale.x);

        spriteChild.localScale = scale;
        anim.SetFloat("Y Velocity", rb.velocity.y);
        anim.SetBool("Run", moveInput != 0);
        anim.SetBool("Grounded", grounded);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
            grounded = false;
        }
        //if(!wasGrounded && isGrounded)
        //{
        //    anim.ResetTrigger("Land");
        //}
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        //Ground Check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
}
