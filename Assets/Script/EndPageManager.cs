using System.Collections;
using UnityEngine;

public class EndPageManager : MonoBehaviour
{
    public GameObject endPagePanel;  // Reference to the End Page Panel
    public float slideSpeed = 1000f; // Speed at which the panel slides up
    private RectTransform endPageRect; // Reference to the RectTransform of the panel
    private bool isSliding = false;

    void Start()
    {
        // Get the RectTransform component of the EndPage Panel
        endPageRect = endPagePanel.GetComponent<RectTransform>();

        // Optionally set the starting position of the end page to off-screen
        endPageRect.anchoredPosition = new Vector2(0, -Screen.height); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 可以换成任何触发条件
        {
            TriggerEndPage();
        }
    }

    public void TriggerEndPage()
    {
        // Trigger sliding when the doodle dies
        isSliding = true;
        StartCoroutine(SlideEndPageUp());
    }

    IEnumerator SlideEndPageUp()
    {
        while (endPageRect.anchoredPosition.y < 0)
        {
            Debug.Log("EndPage Y Position: " + endPageRect.anchoredPosition.y);
            Debug.Log("Slide speed: "+ slideSpeed);
            Debug.Log("Time.deltaTime: "+ Time.deltaTime);
            Vector2 incrementedV = new Vector2(0, slideSpeed * Time.deltaTime);
            endPageRect.anchoredPosition += incrementedV;
            Debug.Log("incremented" + incrementedV);
            yield return null;
        }
        Debug.Log("EndPage reached the top");
        Time.timeScale = 0; // Pause the game
    }

}
