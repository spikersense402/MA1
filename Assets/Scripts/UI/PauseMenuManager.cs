using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pausePanel;  // Assign the Pause Panel in the Inspector
    private bool isPaused = false;

    private void Update()
    {
        // Toggle the pause menu when the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);  // Show the pause menu
        Time.timeScale = 0;  // Freeze the game
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);  // Hide the pause menu
        Time.timeScale = 1;  // Resume the game
        isPaused = false;
    }


}
