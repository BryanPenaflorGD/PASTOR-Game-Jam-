using UnityEngine;
using UnityEngine.UI;

public class FuelGaugeUI : MonoBehaviour
{
    [Header("References")]
    public LightToggle lightToggle; // Reference to your LightToggle script
    public Image fuelBarImage;       // The UI image to fill/deplete

    private void Update()
    {
        if (lightToggle == null || fuelBarImage == null) return;

        // Calculate the fill percentage based on current fuel
        float fillAmount = Mathf.Clamp01(lightToggle.GetFuelPercent());
        fuelBarImage.fillAmount = fillAmount;
    }
}
