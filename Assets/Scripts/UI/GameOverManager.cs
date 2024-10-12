using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }
    public GameObject gameOverPanel;  // Reference to the Game Over UI Panel

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);  // Activate the Game Over panel
            Time.timeScale = 0;  // Pause the game
            Debug.Log("Game Over. Game paused.");
        }
        else
        {
            Debug.LogError("Game Over Panel not assigned in the inspector!");
        }
    }

    public void RestartGame()
    {
        // Reset the game by restarting the current scene
        Time.timeScale = 1;  // Resume game time
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
