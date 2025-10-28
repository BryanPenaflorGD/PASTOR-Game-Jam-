using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakunawaController : MonoBehaviour
{

    [Header("Bakunawa States")]
    public float stunDuration = 5f;
    private bool isStunned = false;
    private bool isDefeated = false;

    public void StunBakunawa()
    {
        if (isDefeated) return;
        if(!isStunned)
        {
            StartCoroutine(StunCoroutine());
        }
    }

    private IEnumerator StunCoroutine()
    {
        isStunned = true;
        Debug.Log("Bakunawa is Stunned");

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
        Debug.Log("Bakunawa has recovered");
        //resume movement or attack patterns
    }

    public void StopCompletely()
    {
        if (isDefeated) return;

        isDefeated = true;
        isStunned = false;
        Debug.Log("Bakunawa has been stopped");

        //additionals e.g. death animation or effects. trigger victory state
    }
}
