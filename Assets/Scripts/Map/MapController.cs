using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    public LayerMask terrainMask;

    public float spawnDistance = 26f;  // Grid size or distance between chunks
    public float chunkBuffer = 5f;     // Buffer distance beyond the edge of the screen for chunk spawning
    public float removalDistance = 60f;  // Distance beyond which chunks will be disabled

    private Vector3 lastChunkPosition;
    private Dictionary<Vector3, GameObject> spawnedChunks = new Dictionary<Vector3, GameObject>();
    PlayerMovement pm;

    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        lastChunkPosition = player.transform.position;
        StartCoroutine(CleanupChunks());  // Start the coroutine to periodically clean up chunks
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
    }

    void ChunkChecker()
    {
        // Get the boundaries of the camera in world space
        Vector3 screenBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 screenTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        // Calculate the buffer area in all directions (left, right, top, bottom)
        float leftEdge = screenBottomLeft.x - chunkBuffer;
        float rightEdge = screenTopRight.x + chunkBuffer;
        float bottomEdge = screenBottomLeft.y - chunkBuffer;
        float topEdge = screenTopRight.y + chunkBuffer;

        // Check all cardinal and diagonal directions for chunk spawning

        SpawnChunkIfNecessary(new Vector3(
            Mathf.Round((rightEdge) / spawnDistance) * spawnDistance,
            Mathf.Round(player.transform.position.y / spawnDistance) * spawnDistance, 0));

        SpawnChunkIfNecessary(new Vector3(
            Mathf.Round((leftEdge) / spawnDistance) * spawnDistance,
            Mathf.Round(player.transform.position.y / spawnDistance) * spawnDistance, 0));

        SpawnChunkIfNecessary(new Vector3(
            Mathf.Round(player.transform.position.x / spawnDistance) * spawnDistance,
            Mathf.Round((topEdge) / spawnDistance) * spawnDistance, 0));

        SpawnChunkIfNecessary(new Vector3(
            Mathf.Round(player.transform.position.x / spawnDistance) * spawnDistance,
            Mathf.Round((bottomEdge) / spawnDistance) * spawnDistance, 0));

        // Diagonal directions

        SpawnChunkIfNecessary(new Vector3(
            Mathf.Round((rightEdge) / spawnDistance) * spawnDistance,
            Mathf.Round((topEdge) / spawnDistance) * spawnDistance, 0));

        SpawnChunkIfNecessary(new Vector3(
            Mathf.Round((leftEdge) / spawnDistance) * spawnDistance,
            Mathf.Round((topEdge) / spawnDistance) * spawnDistance, 0));

        SpawnChunkIfNecessary(new Vector3(
            Mathf.Round((rightEdge) / spawnDistance) * spawnDistance,
            Mathf.Round((bottomEdge) / spawnDistance) * spawnDistance, 0));

        SpawnChunkIfNecessary(new Vector3(
            Mathf.Round((leftEdge) / spawnDistance) * spawnDistance,
            Mathf.Round((bottomEdge) / spawnDistance) * spawnDistance, 0));
    }

    // Spawns a chunk at the specified position if necessary
    void SpawnChunkIfNecessary(Vector3 position)
    {
        if (spawnedChunks.ContainsKey(position))
        {
            // If the chunk already exists but is disabled, enable it again
            if (!spawnedChunks[position].activeSelf)
            {
                spawnedChunks[position].SetActive(true);
            }
        }
        else if (!Physics2D.OverlapCircle(position, checkerRadius, terrainMask))
        {
            // If the chunk doesn't exist, instantiate a new one and add it to the dictionary
            int rand = Random.Range(0, terrainChunks.Count);
            GameObject newChunk = Instantiate(terrainChunks[rand], position, Quaternion.identity);
            spawnedChunks.Add(position, newChunk);  // Track the chunk with its position
        }
    }

    // Coroutine to periodically clean up chunks that are far from the player
    IEnumerator CleanupChunks()
    {
        while (true)
        {
            foreach (var chunk in spawnedChunks)
            {
                // If the chunk is too far from the player, disable it
                if (Vector3.Distance(player.transform.position, chunk.Key) > removalDistance)
                {
                    if (chunk.Value.activeSelf)
                    {
                        chunk.Value.SetActive(false);  // Disable the chunk instead of destroying it
                    }
                }
            }

            // Run cleanup every few seconds to avoid checking every frame (optimization)
            yield return new WaitForSeconds(2f);  // Cleanup every 2 seconds
        }
    }
}
