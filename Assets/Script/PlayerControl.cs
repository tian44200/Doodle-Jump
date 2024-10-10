using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 10f;

    private float LeftRight;
    public Transform background; // Reference to the background object
    private float backgroundHalfWidth;
    public float loseThreshold; // Variable to set the lose threshold
    public GameObject uiManager; // Reference to the UIManager
    
    public SpriteRenderer spriteRenderer; // Reference to the player's SpriteRenderer
    public Sprite defaultSprite; // The normal Doodle sprite
    public Sprite shootSprite; // The sprite to use when shooting
    public float spriteRevertDelay = 0.5f; // Time to wait before reverting back to default sprite
    public GameObject shootImage; // Reference to the shooting image of the doodle

    // Projectile-related variables
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform projectileSpawnPoint; // The point where the projectile should be spawned
    public float projectileSpeed = 5f; // Speed at which the projectile is launched

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();

        // Calculate half of the background's width in world units
        SpriteRenderer bgRenderer = background.GetComponent<SpriteRenderer>();
        backgroundHalfWidth = bgRenderer.bounds.size.x / 2f;
        loseThreshold = background.position.y - bgRenderer.bounds.size.y / 2f; // Set the threshold for losing

        shootImage.SetActive(false);
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

        // // Handle shooting sprite change
        // CheckShootInput();
    }

    private void Update(){
        // Handle shooting sprite change
        CheckShootInput();
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

    private void CheckShootInput() {
        // Check if the up key is pressed
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            // Change the sprite to the jump sprite
            spriteRenderer.sprite = shootSprite;
            // Show the shooting image
            shootImage.SetActive(true);
            // Launch the projectile
            LaunchProjectile();
            // Revert to the default sprite after a delay
            Invoke("RevertSprite", spriteRevertDelay);
        }
    }

    private void LaunchProjectile() {
        // Instantiate the projectile at the player's position
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Determine the target position (above the player)
        Vector3 targetPosition = new Vector3(transform.position.x, background.position.y + 5f, transform.position.z); // Adjust '5f' as needed

        // Start moving the projectile towards the target
        StartCoroutine(MoveProjectile(projectile, targetPosition));
    }

    private IEnumerator MoveProjectile(GameObject projectile, Vector3 targetPosition) {
        float time = 0;

        Vector3 startPosition = projectile.transform.position;

        while (time < 1) {
            // Move the projectile over time to the target position
            projectile.transform.position = Vector3.Lerp(startPosition, targetPosition, time);
            time += Time.deltaTime * projectileSpeed;
            yield return null;
        }

        // Optionally, destroy the projectile after it reaches the target position
        Destroy(projectile);
    }

    private void RevertSprite() {
        // Revert the sprite back to the default sprite
        spriteRenderer.sprite = defaultSprite;
        shootImage.SetActive(false);
    }
}
