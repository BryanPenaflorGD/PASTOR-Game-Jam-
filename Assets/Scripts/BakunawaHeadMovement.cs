using UnityEngine;

public class BakunawaHeadMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 10f;      // How far the dragon travels before turning back
    public float moveSpeed = 3f;          // Horizontal speed
    public float verticalAmplitude = 1f;  // How high/low it goes while flying
    public float verticalFrequency = 2f;  // How fast it bobs up and down

    private Vector3 startPosition;
    private bool movingRight = true;
    private float horizontalOffset = 0f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Horizontal movement
        float direction = movingRight ? 1f : -1f;
        horizontalOffset += moveSpeed * direction * Time.deltaTime;

        // Reverse direction if limit reached
        if (Mathf.Abs(horizontalOffset) >= moveDistance)
        {
            movingRight = !movingRight;
        }

        // Vertical bobbing (like dragon wings flapping up/down)
        float verticalOffset = Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;

        // Update position
        transform.position = new Vector3(
            startPosition.x + horizontalOffset,
            startPosition.y + verticalOffset,
            startPosition.z
        );

        // Optional: Flip the sprite/model when turning (for 2D dragons)
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            sr.flipX = !movingRight;
        }
        else if (transform.localScale.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (movingRight ? 1 : -1);
            transform.localScale = scale;
        }
    }
}
