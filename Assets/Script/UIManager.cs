using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using System.Collections;

public class UIManager : MonoBehaviour
{
    // UI Elements
    public GameObject endPagePanel; // Reference to the End Page Panel

    // Slide parameters
    public float slideSpeed = 1000f; // Speed at which the panel slides up
    private RectTransform endPageRect; // RectTransform of the end page panel
    private bool isSliding = false; // To check if sliding is active

    void Start()
    {
        // Get the RectTransform component of the EndPage Panel
        endPageRect = endPagePanel.GetComponent<RectTransform>();

        // Set the initial position off-screen
        endPageRect.anchoredPosition = new Vector2(0, -Screen.height); 
    }

    void Update()
    {
        // Temporary trigger for testing (space bar)
        if (Input.GetKeyDown(KeyCode.Space)) // Replace this with the actual trigger (e.g., doodle's death)
        {
            TriggerEndPage();
        }
    }

    public void TriggerEndPage()
    {
        // Trigger sliding when needed (e.g., doodle dies)
        if (!isSliding)
        {
            isSliding = true;
            StartCoroutine(SlideEndPageUp());
        }
    }

    IEnumerator SlideEndPageUp()
    {
        // Slide the panel upwards until it is fully visible
        while (endPageRect.anchoredPosition.y < 0)
        {
            Vector2 incrementedV = new Vector2(0, slideSpeed * Time.deltaTime);
            endPageRect.anchoredPosition += incrementedV;
            yield return null;
        }
        
        // Once the end page reaches the top, pause the game
        endPageRect.anchoredPosition = Vector2.zero; // Ensure it's exactly at the top
        Time.timeScale = 0; // Optional: Pause the game
    }

    // Method to go back to the main menu
    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Resume time before loading the new scene
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }

    // Method to restart the current game
    public void ReplayGame()
    {
        Time.timeScale = 1; // Resume time before reloading the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
