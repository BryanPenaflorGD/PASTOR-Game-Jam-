using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class LightToggle : MonoBehaviour
{
    [Header("Light Settings")]
    public Light2D[] lights;
    public KeyCode toggleKey = KeyCode.E;
    public bool lightsOn = true;

    [Header("Wave Settings")]
    public GameObject wavePrefab;
    public float waveSpeed = 5f;
    public float waveMaxScale = 5f;

    [Header("Haliya Meter Settings")]
    public float maxFuel = 100f;
    public float fuelDrainRate = 5f;
    private float currentFuel;
    private bool isDraining = false;

    [Header("References")]
    public PlayerInventory playerInventory;

    private bool isToggling = false;

    private void Start()
    {
        currentFuel = maxFuel;
        Debug.Log(currentFuel);
        // Ensure initial states are consistent
        foreach (var light in lights)
        {
            if (light != null)
                light.enabled = lightsOn;
        }
        isDraining = lightsOn; // start draining immediately if lights start on
    }
    void Update()
    {
        Debug.Log(currentFuel);
        if (Input.GetKeyDown(toggleKey) && !isToggling)
        {
            StartCoroutine(ToggleLightsAndCastWave());

           
        }
      
        if (lightsOn && isDraining)
        {
            DrainFuel();
        }
    }

    private IEnumerator ToggleLightsAndCastWave()
    {
        isToggling = true;

        // Try toggling ON
        if (!lightsOn)
        {
            // Check if we have fragments or fuel left
            if (currentFuel <= 0)
            {
                if (playerInventory != null && playerInventory.fragments > 0)
                {
                    playerInventory.SpendFragments(1);
                    currentFuel = maxFuel;
                    Debug.Log("Shard consumed to refuel light. Remaining: " + playerInventory.fragments);
                }
                else
                {
                    Debug.Log("Out of fragments! Can't turn light on.");
                    isToggling = false;
                    yield break;
                }
            }

            // Turn on the light
            lightsOn = true;
            isDraining = true;
            foreach (var light in lights)
                if (light != null)
                    light.enabled = true;

            if (wavePrefab)
                yield return StartCoroutine(SpawnWave());
        }
        // Toggling OFF
        else
        {
            TurnOffLights();
            if (wavePrefab)
                yield return StartCoroutine(SpawnWave());
        }

        isToggling = false;
    }

    private IEnumerator SpawnWave()
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
    private void DrainFuel()
    {
        currentFuel -= fuelDrainRate * Time.deltaTime;

        if (currentFuel <= 0)
        {
            currentFuel = 0;
            ConsumeShard();
            TurnOffLights();
        }
    }

    private void ConsumeShard()
    {
        Debug.Log("Shard Consumed");
    }

    private void TurnOffLights()
    {
        lightsOn = false;
        isDraining = false;

        foreach (var light in lights)
        {
            if (light != null)
                light.enabled = false;
        }
    }

}

