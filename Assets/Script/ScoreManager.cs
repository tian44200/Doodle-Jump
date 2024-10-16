using UnityEngine;
using UnityEngine.UI;  // For handling UI elements
using TMPro;

/// <summary>
/// The ScoreManager class is responsible for managing the player's score in the game.
/// It tracks the highest point the player has reached, updates the score display in real-time,
/// and handles the saving and displaying of the high score when the player dies.
/// It interacts with UI elements to show the current score, the high score, and the final score on the end page.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public Transform player;  // Reference to the player
    public TextMeshProUGUI scoreText;    // UI Text for displaying the current score
    public TextMeshProUGUI highScoreGlobalText; // UI Text to display the highest score on the end page
    public TextMeshProUGUI endScoreText; // UI Text to display the final score on the end page

    private float highestPoint = 0f;  // Track the highest point the player has reached
    private int scoreInGame = 0;  // Current score

    void Start()
    {
        // Load the global high score
        int highScoreGlobal = PlayerPrefs.GetInt("HighScoreGlobal", 0);

        // Initialize score display
        scoreText.text = scoreInGame.ToString();

        // Display the highest score on the end page (initially hidden)
        highScoreGlobalText.text = "your high score: " + highScoreGlobal.ToString();

        // Display the final score on the end page (initially hidden)
        endScoreText.text = "your score: " + scoreInGame.ToString();
    }

    void Update()
    {
        // Check if the player has reached a new height
        if (player.position.y > highestPoint)
        {
            highestPoint = player.position.y;  // Update the highest point
            scoreInGame = Mathf.FloorToInt(highestPoint);  // Convert height to an integer score
            scoreText.text = scoreInGame.ToString();  // Update the score UI
        }
    }

    // Call this method when the player dies
    public void OnPlayerDeath()
    {
        // Load the saved high score
        int highScoreGlobal = PlayerPrefs.GetInt("highScoreGlobal", 0);

        // If the current score is higher than the saved high score, update it
        if (scoreInGame > highScoreGlobal)
        {
            PlayerPrefs.SetInt("highScoreGlobal", scoreInGame);
            PlayerPrefs.Save();  // Ensure the new high score is saved to disk
        }

        // Update the high score text for the end page
        highScoreGlobalText.text = "your high score: " + PlayerPrefs.GetInt("highScoreGlobal").ToString();

        // Update the final score text on the end page
        endScoreText.text = "your score: " + scoreInGame.ToString();
    }

    public float gethighestPoint(){
        return highestPoint;
    }
}