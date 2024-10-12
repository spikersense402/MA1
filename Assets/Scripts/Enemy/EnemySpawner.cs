using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public float spawnRadius = 5f;
    public int maxEnemyCount = 10;

    private int currentEnemyCount = 0;      // Track active enemies
    private int spawnCounter = 0;           // Track total spawns for unique naming

    private void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned in the Inspector!");
            return;
        }

        Debug.Log("Starting enemy spawner...");
        SpawnNextEnemy();  // Start by spawning the first enemy
    }

    private void SpawnNextEnemy()
    {
        if (currentEnemyCount >= maxEnemyCount)
        {
            Debug.Log($"Max enemy limit reached: {currentEnemyCount}/{maxEnemyCount}");
            return;
        }

        Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        // Use a counter to ensure a unique name for each enemy
        spawnCounter++;
        enemy.name = $"{enemyPrefab.name}_{spawnCounter}";

        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            enemyStats.OnEnemyKilled += HandleEnemyKilled;
            Debug.Log($"Spawned: {enemy.name}. Active enemies: {currentEnemyCount + 1}");
        }
        else
        {
            Debug.LogError("Enemy prefab is missing the EnemyStats component!");
        }

        currentEnemyCount++;
    }

    private void HandleEnemyKilled()
    {
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
        Debug.Log($"Enemy killed. Remaining enemies: {currentEnemyCount}");

        StartCoroutine(SpawnAfterDelay(1.5f));
    }

    private IEnumerator SpawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnNextEnemy();
    }
}
