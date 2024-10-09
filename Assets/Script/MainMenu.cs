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

    public void ExitGame()
    {
        // Quit the application
        #if UNITY_EDITOR
            // If the game is running in the editor, stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // If running in a build, quit the application
            Application.Quit();
        #endif
    }


}
