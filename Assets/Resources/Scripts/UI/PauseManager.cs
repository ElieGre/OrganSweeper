using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : Singleton<PauseManager> {
    public GameObject pauseMenu;
    public GameObject player; // Assuming you have a player GameObject

    private bool isPaused = false;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    private void Start()
    {
        pauseMenu.SetActive(false); // Ensure the PauseMenu starts as inactive
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            return; // Don't allow pausing in other scenes
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Pause the game
            pauseMenu.SetActive(true); // Activate the PauseMenu
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            pauseMenu.SetActive(false); // Deactivate the PauseMenu
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
