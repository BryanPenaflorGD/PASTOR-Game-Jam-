using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShardCounterUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI shardText;   // Assign your TMP text here
    // OR if you're using legacy UI, replace with:
    // public Text shardText;

    [Header("Player Reference")]
    public PlayerInventory playerInventory; // Drag your player here

    private int lastShardCount = -1; // for efficient updates

    private void Start()
    {
        // Auto-find the player inventory if not assigned
        if (playerInventory == null)
            playerInventory = FindObjectOfType<PlayerInventory>();

        UpdateUI(); // Initial display
    }

    private void Update()
    {
        // Only update the UI when the count changes (more efficient)
        if (playerInventory != null && playerInventory.fragments != lastShardCount)
        {
            lastShardCount = playerInventory.fragments;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (shardText != null && playerInventory != null)
            shardText.text = $" {playerInventory.fragments}";
    }
}
