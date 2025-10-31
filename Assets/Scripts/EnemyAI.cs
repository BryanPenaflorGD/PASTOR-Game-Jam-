using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float moveSpeed = 2f;
    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;
    private bool movingRight = true;

    [Header("Detection Settings")]
    public Transform player;
    public float detectionRange = 8f;
    public float chaseSpeed = 3.5f;
    public LightToggle lightToggle; // Reference to your existing LightToggle script

    [Header("Components")]
    private Rigidbody2D rb;
    private AutoRestartWithFade gameManager;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<AutoRestartWithFade>();
    }

    private void Update()
    {
        // If the light is on and the player is close, chase the player
        if (lightToggle != null && lightToggle.lightsOn && PlayerInRange())
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        if (!groundInfo.collider)
        {
            Flip();
        }
    }

    void ChasePlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);

            if ((direction.x > 0 && !movingRight) || (direction.x < 0 && movingRight))
            {
                Flip();
            }
        }
    }

    bool PlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) <= detectionRange;
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.eulerAngles = new Vector3(0, movingRight ? 0 : 180, 0);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Trigger Game Over
            if (gameManager != null)
                gameManager.TriggerGameOver();

            // Optional: play effect or animation here before disappearing
            // e.g., Destroy(gameObject, 0.1f); instead of instantly
            Destroy(gameObject, 0.1f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager != null)
                gameManager.TriggerGameOver();

            Destroy(gameObject, 0.1f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
