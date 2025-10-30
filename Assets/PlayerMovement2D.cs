using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("References")]
    public Transform characterVisual;  // The child object that has the Animator
    public Animator animator;          // Animator on that child

    private Rigidbody2D rb;
    private float moveInput;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Auto-assign if possible
        if (characterVisual == null && transform.childCount > 0)
            characterVisual = transform.GetChild(0);

        if (animator == null && characterVisual != null)
            animator = characterVisual.GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Update animation
        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Flip the visual, not the whole player (so physics isn’t affected)
        if (moveInput > 0 && !facingRight)
            Flip();
        else if (moveInput < 0 && facingRight)
            Flip();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        facingRight = !facingRight;

        // Multiply local scale X by -1 to flip the child visually
        Vector3 scale = characterVisual.localScale;
        scale.x *= -1;
        characterVisual.localScale = scale;
    }
}
