using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using TMPro;

/// <summary>
/// The UIManager class is responsible for managing the user interface elements in the game,
/// specifically the end page panel that appears when the game ends. It handles the sliding
/// animation of the end page panel, pausing the game when the panel is fully visible, and
/// provides methods to navigate back to the main menu or restart the game.
/// </summary>
public class UIManager : MonoBehaviour
{
    // UI Elements
    public GameObject endPagePanel; // Reference to the End Page Panel
    // public Collider2D doodle; // Reference to the Doodle object

    // Slide parameters
    public float slideSpeed = 1000f; // Speed at which the panel slides up
    private RectTransform endPageRect; // RectTransform of the end page panel
    private bool isSliding = false; // To check if sliding is active

    // List of tags to scroll (you can add as many as you need)
    public List<string> scrollableTags = new List<string> { "Platform", "BlackHole", "Monster", "Projectile", "Spring", "Hat", "JetPack" };
    // Cache the objects that need to scroll
    private List<Transform> scrollableObjects = new List<Transform>();

    void Start()
    {
        // Get the RectTransform component of the EndPage Panel
        endPageRect = endPagePanel.GetComponent<RectTransform>();

        // Ensure the panel is positioned below the screen and adjust with the screen size
        endPageRect.anchoredPosition = new Vector2(0, -endPageRect.rect.height);
    }

    public void TriggerEndPage(string colliderTag)
    {
        endPagePanel.SetActive(true); // Ensure the panel is active
        // Trigger sliding when needed (e.g., doodle dies)
        if (!isSliding)
        {
            isSliding = true;
            endPagePanel.SetActive(true); // Ensure the panel is active
            // Find and cache all objects that have tags specified in scrollableTags
            foreach (string tag in scrollableTags)
            {
                GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject obj in objectsWithTag)
                {
                    scrollableObjects.Add(obj.transform); // Add their transforms to the list
                }
            }
            StartCoroutine(SlideEndPageUp(colliderTag));
        }
    }

    IEnumerator SlideEndPageUp(string colliderTag)
    {
        // Slide the panel upwards until it is fully visible
        while (endPageRect.anchoredPosition.y < 0)
        {
            Vector2 incrementedV = new Vector2(0, slideSpeed * Time.deltaTime);
            endPageRect.anchoredPosition += incrementedV;

            // Scroll all objects with the specified tags
            foreach (Transform obj in scrollableObjects)
            {
                if (obj == null) continue; // Skip if the object is null
                if (colliderTag == "FallCollider")
                {
                    obj.position += new Vector3(0, incrementedV.y / 25, 0); // Move upward by the same amount
                }
                else
                {
                    if (!obj.CompareTag("Doodle"))
                    {
                        obj.position += new Vector3(0, incrementedV.y / 25, 0); // Move upward by the same amount
                    }
                }

            }
            yield return null;
        }

        // Once the end page reaches the top, pause the game
        endPageRect.anchoredPosition = Vector2.zero; // Ensure it's exactly at the top
        // Wait for 2 seconds in order to let doodle finish its fall
        yield return new WaitForSeconds(2f);
        CleanSounds(); // Destroy all monsters
        Time.timeScale = 0; // Optional: Pause the game
    }

    public void CleanSounds()
    {
        // Destroy all objects with the tag "Monster"
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monst in monsters)
        {
            Destroy(monst);
        }
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
