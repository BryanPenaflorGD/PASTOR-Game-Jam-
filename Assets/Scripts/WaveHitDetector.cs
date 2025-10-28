using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class WaveHitDetector : MonoBehaviour
{
    public event Action OnPlayerHit;
    private Transform player;
    private bool hasHit = false;

    public void Initialize(Transform targetPlayer)
    {
        player = targetPlayer;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return; // ensure it only fires once

        if (player != null && other.transform == player)
        {
            hasHit = true;
            OnPlayerHit?.Invoke();
        }
    }
}
