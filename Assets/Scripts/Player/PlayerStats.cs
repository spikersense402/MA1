using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentRecovery;
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float currentMight;
    [HideInInspector] public float currentProjectileSpeed;

    public List<GameObject> spawnedWeapons = new List<GameObject>();  // Initialize the list

    [Header("I-Frames")]
    public float invincibilityDuration = 1f;  // Default to 1 second if not set
    private float invincibilityTimer;
    private bool isInvincible;

    private void Awake()
    {
        // Initialize player stats
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;

        // Spawn the starting weapon
        SpawnWeapon(characterData.StartingWeapon);
    }

    private void Update()
    {
        // Handle invincibility timer countdown
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }
    }

    public void TakeDamage(float damage)
    {
        // Only take damage if not currently invincible
        if (!isInvincible)
        {
            currentHealth -= damage;
            Debug.Log($"Player took {damage} damage. Remaining health: {currentHealth}");

            // Trigger invincibility frames
            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            // Check if player is dead
            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        Debug.Log("Player has died.");
        GameOverManager.Instance.ShowGameOver();  // Show Game Over UI

        // Disable player movement or input (if needed)
        gameObject.SetActive(false);  // Disable the player instead of destroying
        Destroy(gameObject);
        // Optional: Destroy the player object after a delay if needed
        // Destroy(gameObject, 2f);  // Adjust delay if necessary
    }

    public void SpawnWeapon(GameObject weapon)
    {
        if (weapon != null)
        {
            GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
            spawnedWeapon.transform.SetParent(transform);  // Parent to player
            spawnedWeapons.Add(spawnedWeapon);  // Track spawned weapons
        }
        else
        {
            Debug.LogError("No weapon assigned to spawn!");
        }
    }
}
