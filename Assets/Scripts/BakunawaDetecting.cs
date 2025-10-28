using UnityEngine;
using System.Collections;

public class BakunawaDetecting : MonoBehaviour
{
    [Header("References")]
    public Transform player;             // Player reference
    public MonoBehaviour movementScript; // The dragon's movement script (e.g., RoamingDragon)

    [Header("Pause Settings")]
    public float alignThreshold = 0.5f;  // How close in X before considered "aligned"
    public float pauseDuration = 2f;     // How long to pause when aligned

    private bool isPaused = false;

    void Update()
    {
        if (player == null || movementScript == null) return;

        // Check horizontal alignment (within threshold)
        float xDiff = Mathf.Abs(transform.position.x - player.position.x);

        // If aligned and not currently paused ? trigger pause
        if (xDiff <= alignThreshold && !isPaused)
        {
            StartCoroutine(PauseMovement());
        }
    }

    IEnumerator PauseMovement()
    {
        isPaused = true;
        movementScript.enabled = false; // stop flying/moving
        yield return new WaitForSeconds(pauseDuration);
        movementScript.enabled = true;  // resume flying
        isPaused = false;
    }
}
