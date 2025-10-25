using UnityEngine;
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

    private bool isToggling = false;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && !isToggling)
        {
            StartCoroutine(ToggleLightsAndCastWave());
        }
    }

    private System.Collections.IEnumerator ToggleLightsAndCastWave()
    {
        isToggling = true;

        
        lightsOn = !lightsOn;
        foreach (var light in lights)
        {
            if (light != null)
                light.enabled = lightsOn;
        }

        
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
}

