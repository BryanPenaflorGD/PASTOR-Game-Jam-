using System.Collections.Generic;
using UnityEngine;

public class ShardSpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform[] spawnPoints;       // Assign spawn points in the Inspector
    public GameObject shardPrefab;        // Prefab to spawn
    public int shardsPerBatch = 5;        // How many per spawn wave
    public float respawnDelay = 2f;       // Delay before next batch (seconds)

    private List<GameObject> activeShards = new List<GameObject>();
    private bool isSpawning = false;

    void Start()
    {
        SpawnNewBatch();
    }

    void Update()
    {
        // Clean up destroyed/null shards from the list
        activeShards.RemoveAll(item => item == null);

        // If all shards are collected and we’re not already spawning another batch
        if (activeShards.Count == 0 && !isSpawning)
        {
            StartCoroutine(RespawnBatchAfterDelay());
        }
    }

    private System.Collections.IEnumerator RespawnBatchAfterDelay()
    {
        isSpawning = true;
        yield return new WaitForSeconds(respawnDelay);
        SpawnNewBatch();
        isSpawning = false;
    }

    private void SpawnNewBatch()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        if (shardPrefab == null)
        {
            Debug.LogWarning("No shard prefab assigned!");
            return;
        }

        List<Transform> availablePoints = new List<Transform>(spawnPoints);
        int count = Mathf.Min(shardsPerBatch, availablePoints.Count);

        activeShards.Clear();

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, availablePoints.Count);
            Transform chosen = availablePoints[randomIndex];

            GameObject shard = Instantiate(shardPrefab, chosen.position, Quaternion.identity);
            activeShards.Add(shard);

            availablePoints.RemoveAt(randomIndex);
        }

        Debug.Log($"Spawned {count} shards in new batch.");
    }
}
