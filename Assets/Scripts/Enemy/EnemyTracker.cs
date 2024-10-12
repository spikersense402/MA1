using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public delegate void EnemyDestroyedHandler();
    public event EnemyDestroyedHandler onEnemyDestroyed;

    private void OnDestroy()
    {
        // Notify the spawner that this enemy is destroyed
        onEnemyDestroyed?.Invoke();
    }
}
