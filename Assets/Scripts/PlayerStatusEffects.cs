using UnityEngine;
using System.Collections;

public class PlayerStatusEffects : MonoBehaviour
{
    private bool isSlowed = false;
    private bool isBlinded = false;
    private bool isLackingFuel = false;

    private float originalSpeed;
    private PlayerController2D playerMovement; // reference to your player's movement script

    [Header("Status Effect Visuals")]
    public GameObject slowEffect;   // assign in Inspector (e.g. blue aura)
    public GameObject blindEffect;  // assign in Inspector (e.g. dark vignette)
    public GameObject fuelEffect;   // assign in Inspector (e.g. low-fuel warning)

    void Start()
    {
        playerMovement = GetComponent<PlayerController2D>();
        if (playerMovement != null)
            originalSpeed = playerMovement.moveSpeed;

        // make sure all effects are off at the start
        if (slowEffect != null) slowEffect.SetActive(false);
        if (blindEffect != null) blindEffect.SetActive(false);
        if (fuelEffect != null) fuelEffect.SetActive(false);
    }

    // Called by enemies or hazards when applying a debuff
    public void ApplyDebuff(string debuffType, float duration)
    {
        switch (debuffType)
        {
            case "Slow":
                if (!isSlowed) StartCoroutine(SlowDebuff(duration));
                break;

            case "Blind":
                if (!isBlinded) StartCoroutine(BlindDebuff(duration));
                break;

            case "Fuel":
                if (!isLackingFuel) StartCoroutine(FuelDebuff(duration));
                break;
        }
    }

    private IEnumerator SlowDebuff(float duration)
    {
        isSlowed = true;
        Debug.Log("Player is slowed!");
        if (slowEffect != null) slowEffect.SetActive(true);

        if (playerMovement != null)
            playerMovement.moveSpeed *= 0.5f; // example: half speed

        yield return new WaitForSeconds(duration);

        if (playerMovement != null)
            playerMovement.moveSpeed = originalSpeed;

        if (slowEffect != null) slowEffect.SetActive(false);

        isSlowed = false;
        Debug.Log("Slow debuff ended.");
    }

    private IEnumerator BlindDebuff(float duration)
    {
        isBlinded = true;
        Debug.Log("Player’s field of vision is shrinking...");
        if (blindEffect != null) blindEffect.SetActive(true);

        Camera cam = Camera.main;
        if (cam != null)
        {
            float originalSize = cam.orthographicSize;
            float targetSize = originalSize * 0.5f; // shrink vision to 50%
            float halfDuration = duration / 1f;
            float t = 0f;

            // Smooth zoom IN
            while (t < halfDuration)
            {
                t += Time.deltaTime;
                cam.orthographicSize = Mathf.Lerp(originalSize, targetSize, t / halfDuration);
                yield return null;
            }

            // Stay zoomed briefly
            yield return new WaitForSeconds(8f);

            // Smooth zoom OUT
            t = 0f;
            while (t < halfDuration)
            {
                t += Time.deltaTime;
                cam.orthographicSize = Mathf.Lerp(targetSize, originalSize, t / halfDuration);
                yield return null;
            }

            cam.orthographicSize = originalSize;
        }

        if (blindEffect != null) blindEffect.SetActive(false);

        isBlinded = false;
        Debug.Log("Player’s vision returned to normal!");
    }

    private IEnumerator FuelDebuff(float duration)
    {
        isLackingFuel = true;
        Debug.Log("Player is losing grip on a fragment!");
        if (fuelEffect != null) fuelEffect.SetActive(true);

        yield return new WaitForSeconds(duration);

        if (fuelEffect != null) fuelEffect.SetActive(false);

        isLackingFuel = false;
        Debug.Log("Lack of Fuel ended!");
    }
}
