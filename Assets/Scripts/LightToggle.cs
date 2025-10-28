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

        // Determine next light state, but prevent enabling if out of fuel
        bool nextLightsOn = !lightsOn;
        if (nextLightsOn && currentFuel <= 0f)
        {
            // Cannot turn on lights with no fuel
            isToggling = false;
            yield break;
        }

        // Apply toggle
        lightsOn = nextLightsOn;
        foreach (var light in lights)
        {
            if (light != null)
                light.enabled = lightsOn;
        }

        //manage fuel drain state
        if (lightsOn)
        {
            isDraining = true;
        }
        else
        {
            isDraining = false;
        }

        //cast wave effect
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

        isToggling = false;
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

