using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BeaconActivator : MonoBehaviour
{
    [Header("Beacon Settings")]
    public int requiredFragments = 5;
    public float channelTime = 3f;
    public GameObject beaconLight;

    [Header("Detection Settings")]
    public float detectionRadius = 2f;
    public LayerMask playerLayer;

    public UnityEvent onBeaconActivated;

    private bool isActivated = false;
    private bool isChanneling = false; 
    private Coroutine channelRoutine;
    private PlayerInventory playerInventory;
    private bool playerInRange = false;

    private void Awake()
    {
        // Initialize UnityEvent if it's null
        if (onBeaconActivated == null)
            onBeaconActivated = new UnityEvent();
    }

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    private void Update()
    {
        // Check if player is within radius
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        playerInRange = (playerCollider != null);

        if (!playerInRange || isActivated) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isChanneling)
                channelRoutine = StartCoroutine(ChannelBeacon());
            else
                StopChanneling("Channeling stopped manually.");
        }
    }

    private IEnumerator ChannelBeacon()
    {
        if (!playerInventory.HasEnoughFragments(requiredFragments))
        {
            Debug.Log("Not enough fragments to activate beacon!");
            yield break;
        }

        isChanneling = true;
        float elapsed = 0f;
        Debug.Log("Channeling started...");

        while (elapsed < channelTime)
        {
            if (!isChanneling) yield break;

            // Player left range mid-channel
            if (!playerInRange)
            {
                StopChanneling("Player left beacon range.");
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // CHANNEL COMPLETE ✅
        playerInventory.SpendFragments(requiredFragments);
        Debug.Log($"Spent {requiredFragments} fragments. Remaining: {playerInventory.fragments}");
        ActivateBeacon();
        isChanneling = false;
    }

    private void StopChanneling(string reason)
    {
        if (channelRoutine != null)
            StopCoroutine(channelRoutine);

        isChanneling = false;
        channelRoutine = null;
        Debug.Log(reason);
    }

    private void ActivateBeacon()
    {
        if (isActivated) return;
        isActivated = true;

        Debug.Log("Beacon Activated!");
        if (beaconLight != null)
            beaconLight.SetActive(true);

        onBeaconActivated?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
