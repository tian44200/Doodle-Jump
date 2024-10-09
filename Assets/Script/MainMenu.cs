using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Camera mainMenuCamera; // Assign your Main Menu camera here

    public void PlayGame()
    {
        // Disable MainMenu Audio Listener before loading GameScene
        if (mainMenuCamera != null)
        {
            AudioListener listener = mainMenuCamera.GetComponent<AudioListener>();
            if (listener != null)
            {
                listener.enabled = false;
            }
        }

        // Load GameScene
        SceneManager.LoadScene("GameScene");
    }
}
