using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class UIManager : MonoBehaviour
{
    public GameObject loseMessagePanel; // Reference to the panel or UI element to display the image
    public GameObject mainMenuButton; // Reference to the Main Menu button
    public GameObject replayButton; // Reference to the Replay button

    void Start()
    {
        // Initially hide the lose message panel and buttons
        loseMessagePanel.SetActive(false);
    }

    public void ShowLoseMessage()
    {
        // Show the lose message panel and buttons
        loseMessagePanel.SetActive(true);
        mainMenuButton.SetActive(true); // Show Main Menu button
        replayButton.SetActive(true); // Show Replay button
        Time.timeScale = 0; // Optional: Pause the game
    }

    // Method to return to the main menu
    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Resume time if it was paused
        SceneManager.LoadScene("MainMenu"); // Load your main menu scene
    }

    // Method to restart the game
    public void ReplayGame()
    {
        Time.timeScale = 1; // Resume time if it was paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
