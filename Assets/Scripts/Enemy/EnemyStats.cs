using UnityEngine;
using System;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentDamage;

    public event Action OnEnemyKilled;

    private bool isAlive = true;

    private void Awake()
    {
        EnemyController controller = GetComponent<EnemyController>();
        if (controller == null || controller.enemyData == null)
        {
            Debug.LogError("EnemyController or EnemyScriptableObject is missing!");
            return;
        }

        enemyData = controller.enemyData;
        InitializeStats();
    }

    private void InitializeStats()
    {
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        Debug.Log($"{gameObject.name} initialized with {currentHealth} health and {currentDamage} damage.");
    }

    public void TakeDamage(float damage)
    {
        if (!isAlive) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        if (!isAlive) return;

        isAlive = false;
        Debug.Log($"{gameObject.name} has been killed.");

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;

        OnEnemyKilled?.Invoke();
        Destroy(gameObject, 0.1f);
    }
}
