using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class BakunawaDetecting : MonoBehaviour
{
    [Header("References")]
    public Transform player;               // Player reference
    public MonoBehaviour movementScript;   // The movement script (e.g., RoamingDragon)

    [Header("Pause Settings")]
    public float alignThreshold = 0.5f;    // How close in X before considered "aligned"
    public float pauseDuration = 2f;       // How long to pause when aligned

    [Header("Light & Wave Settings")]
    public Light2D[] lights;               // Lights to toggle
    public GameObject wavePrefab;          // Wave effect prefab
    public float waveSpeed = 5f;           // How fast the wave expands
    public float waveMaxScale = 5f;        // Maximum wave scale

    private bool isPaused = false;
    private bool hasCastedThisPause = false;

    void Update()
    {
        if (player == null || movementScript == null) return;

        // Check if horizontally aligned with player
        float xDiff = Mathf.Abs(transform.position.x - player.position.x);

        if (xDiff <= alignThreshold && !isPaused)
        {
            StartCoroutine(PauseAndCast());
        }
    }

    private IEnumerator PauseAndCast()
    {
        isPaused = true;
        hasCastedThisPause = false;

        // Stop dragon movement
        movementScript.enabled = false;

        // Perform action once during the pause
        if (!hasCastedThisPause)
        {
            hasCastedThisPause = true;
            yield return StartCoroutine(ToggleLightsAndCastWave());
        }

        // Wait for the pause duration
        yield return new WaitForSeconds(pauseDuration);

        // Resume movement
        movementScript.enabled = true;
        isPaused = false;
    }

    private IEnumerator ToggleLightsAndCastWave()
    {
        // Toggle all lights
        bool lightsOn = false;
        foreach (var light in lights)
        {
            if (light != null)
                light.enabled = lightsOn;
        }

        // Cast expanding wave
        if (wavePrefab)
        {
            GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
            float currentScale = 0.1f;

            while (currentScale < waveMaxScale)
            {
                currentScale += Time.deltaTime * waveSpeed;
                wave.transform.localScale = Vector3.one * currentScale;
                yield return null;
            }

            Destroy(wave);
        }
    }
}
