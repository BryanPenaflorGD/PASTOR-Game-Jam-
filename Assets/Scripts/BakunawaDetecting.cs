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
    public Light2D[] lights;               
    public GameObject wavePrefab;          
    public float waveSpeed = 5f;           
    public float waveMaxScale = 5f;        

    private bool isPaused = false;
    private bool hasCastedThisPause = false;

    private int detectCount = 0;

    void Update()
    {
        if (player == null || movementScript == null) return;

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

            WaveHitDetector detector = wave.GetComponent<WaveHitDetector>();
            if (detector != null)
            {
                detector.Initialize(player);
                detector.OnPlayerHit += OnPlayerHitByWave;
                
            }

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
    private void OnPlayerHitByWave()
    {
        Debug.Log("Player was hit by Bakunawa’s search wave!");
        // TODO: Add debuffs

        PlayerStatusEffects playerEffects = player.GetComponent<PlayerStatusEffects>();
        int debuffValue = Random.Range(1, 3);
        if (playerEffects != null)
        {
            if (debuffValue == 1)
                playerEffects.ApplyDebuff("Slow", 15f);
            if (debuffValue == 2)
                playerEffects.ApplyDebuff("Blind", 10f);
            if (debuffValue == 3)
                playerEffects.ApplyDebuff("Fuel", 15f);
        }

        detectCount++;
        Debug.Log("Times Detected: " + detectCount);
    }
}
