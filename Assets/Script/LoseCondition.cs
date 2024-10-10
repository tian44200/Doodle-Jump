using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    public GameObject uiManager; // Reference to the UIManager

    void OnTriggerEnter2D(Collider2D other)
    {
    
        if(other.transform.tag == "Doodle"){
            uiManager.GetComponent<UIManager>().ShowLoseMessage();
            // Optional: Disable player movement or other logic to handle game over
            other.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop player movement
        }
    }
}
