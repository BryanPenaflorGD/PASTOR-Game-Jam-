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

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();

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

        // Toggle channeling on key press
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isChanneling)
            {
                if (playerInventory.HasEnoughFragments(requiredFragments))
                {
                    channelRoutine = StartCoroutine(ChannelBeacon());
                    anim.SetBool("isChanneling", true);
                }
                else
                {
                    Debug.Log("Not enough fragments to activate beacon!");
                }
            }
            else
            {
                StopChanneling("Channeling stopped manually.");
            }
        }
    }

    private IEnumerator ChannelBeacon()
    {
        isChanneling = true;
        float elapsed = 0f;
        Debug.Log("Channeling started...");

        while (elapsed < channelTime)
        {
            // Interrupted manually
            if (!isChanneling)
            {
                StopChanneling("Channeling interrupted.");
                yield break;
            }

            // Player moved away
            if (!playerInRange)
            {
                StopChanneling("Player left beacon range.");
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // ✅ Channeling complete — now spend fragments
        playerInventory.SpendFragments(requiredFragments);
        Debug.Log($"Beacon activated! Spent {requiredFragments} fragments. Remaining: {playerInventory.fragments}");

        ActivateBeacon();
        isChanneling = false;
        anim.SetBool("isChanneling", false);
    }

    private void StopChanneling(string reason)
    {
        if (channelRoutine != null)
            StopCoroutine(channelRoutine);

        isChanneling = false;
        channelRoutine = null;
        anim.SetBool("isChanneling", false);

        Debug.Log(reason);
    }

    private void ActivateBeacon()
    {
        if (isActivated) return;
        isActivated = true;

        if (beaconLight != null)
            beaconLight.SetActive(true);

        onBeaconActivated?.Invoke();

        Debug.Log("✅ Beacon successfully activated!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
