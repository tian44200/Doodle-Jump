using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 10f;

    private float LeftRight;
    public Transform background; // Reference to the background object
    private float backgroundHalfWidth;
    public float loseThreshold; // Variable to set the lose threshold
    public GameObject uiManager; // Reference to the UIManager

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();

        // Calculate half of the background's width in world units
        SpriteRenderer bgRenderer = background.GetComponent<SpriteRenderer>();
        backgroundHalfWidth = bgRenderer.bounds.size.x / 2f;
        loseThreshold = background.position.y - bgRenderer.bounds.size.y / 2f; // Set the threshold for losing
    }

    private void FixedUpdate() {
        // Get the horizontal input
        LeftRight = Input.GetAxis("Horizontal") * moveSpeed;

        // Flip the player's sprite based on movement direction
        if (LeftRight < 0) {
            transform.rotation = Quaternion.Euler(180, 0, 180); // Moving Left
        } else if (LeftRight > 0) {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Moving Right
        }

        // Apply velocity to the rigidbody to move the player
        rb.velocity = new Vector2(LeftRight, rb.velocity.y);

        // Call the function to check for screen wrapping
        WrapAroundBackground();

        // Check if the player has fallen below the threshold
        CheckLoseCondition();
    }

    private void WrapAroundBackground() {
        // Check if the player has moved beyond the left side of the background
        if (transform.position.x < -backgroundHalfWidth) {
            transform.position = new Vector3(backgroundHalfWidth, transform.position.y, transform.position.z); // Move to the right side
        }
        // Check if the player has moved beyond the right side of the background
        else if (transform.position.x > backgroundHalfWidth) {
            transform.position = new Vector3(-backgroundHalfWidth, transform.position.y, transform.position.z); // Move to the left side
        }
    }

    private void CheckLoseCondition() {
        // If the player falls below the lose threshold
        if (transform.position.y < loseThreshold) {
            // Notify the UIManager to show the lose message
            uiManager.GetComponent<UIManager>().ShowLoseMessage();
            // Optional: Disable player movement or other logic to handle game over
            rb.velocity = Vector2.zero; // Stop player movement
        }
    }
}
