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
    
    public SpriteRenderer spriteRenderer; // Reference to the player's SpriteRenderer
    public float spriteRevertDelay = 0.5f; // Time to wait before reverting back to default sprite
    public GameObject shootImage; // Reference to the shooting image of the doodle

    // Projectile-related variables
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform projectileSpawnPoint; // The point where the projectile should be spawned
    public float projectileSpeed = 5f; // Speed at which the projectile is launched
    private Animator animator;
    private float lastDirection = -1; // 1 for right, -1 for left
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        if (LeftRight >= 0) {
            transform.rotation = Quaternion.Euler(180, 0, 180); // Moving Left
        } else if (LeftRight < 0) {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Moving Right
        }

        // Apply velocity to the rigidbody to move the player
        rb.velocity = new Vector2(LeftRight, rb.velocity.y);

        // Call the function to check for screen wrapping
        WrapAroundBackground();

    }

    private void Update(){
        // Handle shooting sprite change
        // CheckShootInput();
        HandleMovementInput();
        HandleJumping();
        HandleShooting();
    }

/************************/
/**** Movement logic ****/
/************************/
void HandleMovementInput()
    {
    float horizontalInput = Input.GetAxisRaw("Horizontal");

    if (horizontalInput < 0)  // 按下左键
    {
        lastDirection = -1;  // 记录最后的方向
        transform.rotation = Quaternion.Euler(0, 0, 0);  // 朝左
    }
    else if (horizontalInput > 0)  // 按下右键
    {
        lastDirection = 1;  // 记录最后的方向
        transform.rotation = Quaternion.Euler(180, 0, 180);  // 朝右
    }
    else
    {
        // 没有输入时，保持最后的方向
        if (lastDirection == 1)
        {
            transform.rotation = Quaternion.Euler(180, 0, 180);  // 保持朝右
        }
        else if (lastDirection == -1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);  // 保持朝左
        }
    }

    // 应用水平移动速度
    rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
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

/************************/
/**** Shooting logic ****/
/************************/

   void HandleJumping()
{
    // If the player is moving upwards (jumping), set isJumping to true
    if (rb.velocity.y > 0.1f)
    {
        animator.SetBool("isJumping", true);
    }
    // If the player is not moving upwards, set isJumping to false
    else
    {
        animator.SetBool("isJumping", false);
    }
}

void HandleShooting()
{
    // Trigger shooting when the up arrow is pressed
    if (Input.GetKeyDown(KeyCode.UpArrow))
    {
        animator.SetBool("isShooting", true);
        LaunchProjectile(); // Launch the projectile
        Invoke("StopShooting", spriteRevertDelay); // Reset shooting animation after a delay
    }
}

void StopShooting()
{
    // Stop the shooting animation
    animator.SetBool("isShooting", false);
}

private void LaunchProjectile()
{
    // Create a projectile at the player's position
    GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

    // Set the target position (above the player)
    Vector3 targetPosition = new Vector3(transform.position.x, background.position.y + 5f, transform.position.z);

    // Start moving the projectile
    StartCoroutine(MoveProjectile(projectile, targetPosition));
}

private IEnumerator MoveProjectile(GameObject projectile, Vector3 targetPosition)
{
    float time = 0;
    Vector3 startPosition = projectile.transform.position;

    // Move the projectile towards the target over time
    while (time < 1)
    {
        projectile.transform.position = Vector3.Lerp(startPosition, targetPosition, time);
        time += Time.deltaTime * projectileSpeed;
        yield return null;
    }

    // Destroy the projectile after it reaches the target
    Destroy(projectile);
}


}
