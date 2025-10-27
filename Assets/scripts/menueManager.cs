using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject creditsPanel; // Reference to the credits panel GameObject
    public GameObject controlsPanel; // Reference to the controls panel GameObject
    // Function to start the game (loads a new scene)
    public void StartGame()
    {
        SceneManager.LoadScene("main lvl"); // Replace "GameScene" with your actual game scene name
    }

    // Function to quit the game
    public void QuitGame()
    {
        Debug.Log("Game Quit"); // Logs a message in the editor
        Application.Quit(); // Quits the application (works in a built game, not in the editor)
    }

    // Function to show the credits panel
    public void ShowCredits()
    {
        creditsPanel.SetActive(true); // Sets the credits panel to active
    }

    // Function to hide the credits panel
    public void HideCredits()
    {
        creditsPanel.SetActive(false); // Sets the credits panel to inactive
    }

    // Function to show the controls panel
    public void ShowControls()
    {
        controlsPanel.SetActive(true); // Sets the controls panel to active
    }

    // Function to hide the controls panel
    public void HideControls()
    {
        controlsPanel.SetActive(false); // Sets the controls panel to inactive
    }
}
