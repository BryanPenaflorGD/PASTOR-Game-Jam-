using UnityEngine;

public class MoonLeechAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;          // Assign the player here
    public LightToggle playerLight;   // Reference the LightToggle script on the player

    [Header("Detection Settings")]
    public float detectionRadius = 5f;
    public float dropDelay = 0.3f;

    [Header("Chase Settings")]
    public float chaseSpeed = 2f;
    public float chaseDuration = 2f;

    [Header("Gravity Settings")]
    public float fallGravityScale = 3f;

    [Header("Wiggle Settings")]
    public float wiggleAmount = 0.05f;   // How much it moves left-right
    public float wiggleSpeed = 8f;       // How fast it wiggles
    public float wiggleDuration = 0.5f;  // How long it wiggles before pausing
    public float wigglePause = 2.5f;     // How long it pauses before wiggling again

    private Rigidbody2D rb;
    private Vector3 initialPosition;
    private bool isFalling = false;
    private bool isChasing = false;
    private bool isDead = false;
    private bool isWiggling = true;
    private float wiggleTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Start attached to ceiling
        initialPosition = transform.position;
    }

    void Update()
    {
        if (isDead || player == null || playerLight == null) return;

        // Wiggle motion while still attached to the ceiling
        if (!isFalling && !isChasing)
        {
            HandleWigglePattern();
        }

        // Detect player within radius and if light is on
        float distance = Vector2.Distance(transform.position, player.position);
        if (!isFalling && playerLight.lightsOn && distance <= detectionRadius)
        {
            StartCoroutine(DropAfterDelay());
        }

        // Chase movement
        if (isChasing)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        }
    }

    void HandleWigglePattern()
    {
        wiggleTimer += Time.deltaTime;

        if (isWiggling)
        {
            float wiggleOffset = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;
            transform.position = initialPosition + new Vector3(wiggleOffset, 0, 0);

            if (wiggleTimer >= wiggleDuration)
            {
                isWiggling = false;
                wiggleTimer = 0f;
            }
        }
        else
        {
            transform.position = initialPosition;

            if (wiggleTimer >= wigglePause)
            {
                isWiggling = true;
                wiggleTimer = 0f;
            }
        }
    }

    System.Collections.IEnumerator DropAfterDelay()
    {
        isFalling = true;
        yield return new WaitForSeconds(dropDelay);
        rb.gravityScale = fallGravityScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Moon Leech hit the player!");
            Die();
        }
        else if (isFalling)
        {
            // Missed the player, start short chase
            StartCoroutine(ChaseThenDie());
        }
    }

    System.Collections.IEnumerator ChaseThenDie()
    {
        isFalling = false;
        isChasing = true;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(chaseDuration);
        Die();
    }

    void Die()
    {
        isDead = true;
        isChasing = false;
        rb.gravityScale = 0;
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
