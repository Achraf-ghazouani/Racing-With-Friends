using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSystem : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign the Pause Menu UI Panel in Inspector
    public GameObject creditsPanelUI; // Assign the Credits Panel UI Panel in Inspector
    public GameObject[] objectsToDisable; // Array of game objects to disable when paused
    private bool isPaused = false;
    private bool[] objectsWereActive; // Track which objects were active

    private void Start()
    {
        // Initialize the tracking array
        objectsWereActive = new bool[objectsToDisable.Length];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Show Pause Menu
        creditsPanelUI.SetActive(false); // Hide Credits Panel
        Time.timeScale = 0f; // Pause Game
        isPaused = true;

        // Disable specified game objects and track their active state
        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            if (objectsToDisable[i].activeSelf)
            {
                objectsWereActive[i] = true;
                objectsToDisable[i].SetActive(false);
            }
            else
            {
                objectsWereActive[i] = false;
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide Pause Menu
        Time.timeScale = 1f; // Resume Game
        isPaused = false;

        // Do not re-enable objects that were not active before pausing
        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            if (objectsWereActive[i])
            {
                objectsToDisable[i].SetActive(true);
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Ensure game runs normally
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }

    public void ShowCredits()
    {
        pauseMenuUI.SetActive(false); // Hide Pause Menu
        creditsPanelUI.SetActive(true); // Show Credits Panel
    }

    public void BackToPauseMenu()
    {
        creditsPanelUI.SetActive(false); // Hide Credits Panel
        pauseMenuUI.SetActive(true); // Show Pause Menu
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the game
        Debug.Log("Game Quit!"); // This only appears in the editor
    }
}