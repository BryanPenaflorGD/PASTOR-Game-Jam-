using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BeaconActivator : MonoBehaviour
{
    [Header("Beacon Settings")]
    public int requiredFragments = 5;
    public float channelTime = 3f;

    [Header("Detection Settings")]
    public float detectionRadius = 2f;
    public LayerMask playerLayer;

    [Header("Beacon Light")]
    public GameObject beaconLight; // child light or effect GameObject

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

        // Automatically find a child named "Light" or "BeaconLight"
        Transform child = transform.Find("Light") ?? transform.Find("BeaconLight");
        if (child != null)
        {
            beaconLight = child.gameObject;
            beaconLight.SetActive(false);
        }
    }

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    private void Update()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        bool wasInRange = playerInRange;
        playerInRange = (playerCollider != null);

        if (isActivated) return;

        // Start channeling when player enters
        if (playerInRange && !wasInRange && !isChanneling)
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

        // Stop channeling if player leaves range
        if (!playerInRange && isChanneling)
        {
            StopChanneling("Player left beacon range.");
        }
    }

    private IEnumerator ChannelBeacon()
    {
        isChanneling = true;
        float elapsed = 0f;
        Debug.Log("Channeling started...");

        while (elapsed < channelTime)
        {
            if (!isChanneling) yield break;
            if (!playerInRange)
            {
                StopChanneling("Player left beacon range.");
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        playerInventory.SpendFragments(requiredFragments);
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
