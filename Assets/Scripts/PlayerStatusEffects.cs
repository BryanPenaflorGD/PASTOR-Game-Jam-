using UnityEngine;
using System.Collections;

public class PlayerStatusEffects : MonoBehaviour
{
    private bool isSlowed = false;
    private bool isBlinded = false;
    private bool isLackingFuel = false;

    private float originalSpeed;
    private PlayerController2D playerMovement; // reference to your player's movement script

    void Start()
    {
        playerMovement = GetComponent<PlayerController2D>();
        if (playerMovement != null)
            originalSpeed = playerMovement.moveSpeed;
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
                if(!isLackingFuel) StartCoroutine(FuelDebuff(duration));
                break;
        }
    }

    private IEnumerator SlowDebuff(float duration)
    {
        isSlowed = true;
        Debug.Log("Player is slowed!");

        if (playerMovement != null)
            playerMovement.moveSpeed *= 0.5f; // example: half speed

        yield return new WaitForSeconds(duration);

        if (playerMovement != null)
            playerMovement.moveSpeed = originalSpeed;

        isSlowed = false;
        Debug.Log("Slow debuff ended.");
    }

    private IEnumerator BlindDebuff(float duration)
    {
        isBlinded = true;
        Debug.Log("Player is blinded!");
        // e.g., dim player’s light or add screen overlay

        yield return new WaitForSeconds(duration);

        isBlinded = false;
        Debug.Log("Blindness ended!");
    }
    private IEnumerator FuelDebuff(float duration)
    {
        isBlinded = true;
        Debug.Log("Player is losing grip on a fragment!");
        // moon fragment usage shortened

        yield return new WaitForSeconds(duration);

        isBlinded = false;
        Debug.Log("Lack of Fuel ended!");
    }
}
