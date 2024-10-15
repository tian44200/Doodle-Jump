using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioSource audioSource; // AudioSource for jump sound


    public GameObject mouth;  

    public GameObject hat;
    public float hatForce;
    public float itemTimer;
    public float hatTime;

    public GameObject jetPack;
    public float jetPackForce;
    public float jetPackTime;
    private Animator jetPackAnimator;


    public float moveSpeed = 10f;
    private float LeftRight;
    public Transform background; // Reference to the background object
    private float backgroundHalfWidth;
    public float jumpForce; // Jump force to apply to the player
    public float springForce; // Jump force to apply to the player
    
    public float spriteRevertDelay = 0.5f; // Time to wait before reverting back to default sprite
    public GameObject shootImage; // Reference to the shooting image of the doodle

    // Projectile-related variables
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform projectileSpawnPoint; // The point where the projectile should be spawned
    public float projectileSpeed = 5f; // Speed at which the projectile is launched
    private Animator animator;
    private float lastDirection = -1; // 1 for right, -1 for left
    public AudioClip jumpSound; 
    public AudioClip shootSound; 
    public AudioClip springSound;
    public AudioClip jetPackSound;
    public AudioClip hatSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Fetch the AudioSource component attached to the player
        audioSource = GetComponent<AudioSource>();

        // Calculate half of the background's width in world units
        SpriteRenderer bgRenderer = background.GetComponent<SpriteRenderer>();
        backgroundHalfWidth = bgRenderer.bounds.size.x / 2f;
        jetPackAnimator = jetPack.gameObject.GetComponent<Animator>();

        shootImage.SetActive(false);
    }

    private void FixedUpdate()
    {
        // Get the horizontal input
        LeftRight = Input.GetAxis("Horizontal") * moveSpeed;

        // Flip the player's sprite based on movement direction
        if (LeftRight >= 0)
        {
            transform.rotation = Quaternion.Euler(180, 0, 180); // Moving Left
        }
        else if (LeftRight < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Moving Right
        }

        // Apply velocity to the rigidbody to move the player
        rb.velocity = new Vector2(LeftRight, rb.velocity.y);

        if(hat.activeSelf == true){
            
            itemTimer -= Time.deltaTime;

            PlayHatSound();
            
            if(itemTimer < 0){
                hat.SetActive(false);
            }
        }

        if(jetPack.activeSelf == true){
            itemTimer-= Time.deltaTime;

            PlayJetPackSound();

            if(itemTimer <1f){
                jetPackAnimator.SetBool("endJetPack",true);
            }
            if( itemTimer < 0){
                jetPack.SetActive(false);
            }
        }

        // Call the function to check for screen wrapping
        WrapAroundBackground();

    }

    private void PlayHatSound(){
        if(hatSound != null && audioSource != null){
            audioSource.volume = Mathf.Clamp(0.005f, 0f, 1f);
            audioSource.PlayOneShot(hatSound);
            // audioSource.volume = Mathf.Clamp(0.1f, 0f, 1f);

        }
    }

    private void Update()
    {
        HandleMovementInput();
        HandleJumping();
        HandleShooting();
    }

    /********************************/
    /******** Movement logic ********/
    /********************************/
    void HandleMovementInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Check input direction and rotate the player accordingly
        if (horizontalInput < 0)  // Moving left
        {
            lastDirection = -1;  // Record last direction as left
            transform.rotation = Quaternion.Euler(0, 0, 0);  // Face left
        }
        else if (horizontalInput > 0)  // Moving right
        {
            lastDirection = 1;  // Record last direction as right
            transform.rotation = Quaternion.Euler(180, 0, 180);  // Face right
        }
        else
        {
            // If no input is given, keep the last facing direction
            if (lastDirection == 1)
            {
                transform.rotation = Quaternion.Euler(180, 0, 180);  // Keep facing right
            }
            else if (lastDirection == -1)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);  // Keep facing left
            }
        }

        // Apply horizontal movement
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    private void WrapAroundBackground()
    {
        // If the player moves beyond the left side, move them to the right side of the background
        if (transform.position.x < -backgroundHalfWidth)
        {
            transform.position = new Vector3(backgroundHalfWidth, transform.position.y, transform.position.z); // Move to the right side
        }
        // If the player moves beyond the right side, move them to the left side of the background
        else if (transform.position.x > backgroundHalfWidth)
        {
            transform.position = new Vector3(-backgroundHalfWidth, transform.position.y, transform.position.z); // Move to the left side
        }
    }

    /********************************/
    /********* Jumping logic ********/
    /********************************/
    void HandleJumping()
    {
        // If the player is moving upwards, set isJumping to true
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

    // Handle collisions with platforms to apply jump force
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the player is colliding with a platform
        if (other.gameObject.CompareTag("Platform"))
        {
            // If the player is falling or not moving upwards, apply jump force
            if (rb.velocity.y <= 0.2f)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                PlayJumpSound(); // Play sound when jumping

            }
        }
    }


    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            // If the player is falling or not moving upwards, apply jump force
            if (rb.velocity.y <= 0.2f)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }


    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.gameObject.CompareTag("Spring")){
    //         if(rb.velocity.y <= 0.2f){
    //             rb.AddForce(Vector2.up * springForce, ForceMode2D.Impulse);
    //         }
    //     }else if(other.gameObject.CompareTag("Hat")){
    //         if(rb.velocity.y <= 0.2f){
    //             hat.SetActive(true);
    //             BoostUp(hatTime, hatForce);
    //             Destroy(other.gameObject);
    //         }
    //     }else if(other.gameObject.CompareTag("JetPack")){
    //         if(rb.velocity.y <= 0.2f){
    //             jetPack.SetActive(true);
    //             BoostUp(jetPackTime, jetPackForce);
    //             Destroy(other.gameObject);
    //         }
    //     }
    // }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Spring")){
            if(rb.velocity.y <= 0.2f){
                rb.AddForce(Vector2.up * springForce, ForceMode2D.Impulse);
                PlaySpringSound(); // Play the spring sound when bouncing on a spring

            }
        }else if(other.gameObject.CompareTag("Hat")){
            if(rb.velocity.y <= 0.2f){
                hat.SetActive(true);
                BoostUp(hatTime, hatForce);
                Destroy(other.gameObject);
            }
        }else if(other.gameObject.CompareTag("JetPack")){
            if(rb.velocity.y <= 0.2f){
                jetPack.SetActive(true);
                BoostUp(jetPackTime, jetPackForce);
                Destroy(other.gameObject);
            }
        }
    }

    // Method to play the jump sound
    private void PlayJumpSound()
    {
        if (jumpSound != null && audioSource != null)
        {
            audioSource.volume = Mathf.Clamp(0.1f, 0f, 1f);
            audioSource.PlayOneShot(jumpSound);
        }
    }

    private void PlaySpringSound()
    {
        if (springSound != null && audioSource != null)
        {
            audioSource.volume = Mathf.Clamp(0.1f, 0f, 1f);
            audioSource.PlayOneShot(springSound);
        }
    }

    private void PlayJetPackSound(){
        if(jetPackSound != null && audioSource != null){
            audioSource.volume = Mathf.Clamp(0.005f, 0f, 1f);
            audioSource.PlayOneShot(jetPackSound);
            // audioSource.volume = Mathf.Clamp(0.1f, 0f, 1f);

        }
    }

    /************************/
    /**** Shooting logic ****/
    /************************/
    void HandleShooting()
    {
        // Trigger shooting when the up arrow is pressed
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            animator.SetBool("isShooting", true);
            mouth.SetActive(true);
            PlayShootSound(); // Play the shoot sound
            LaunchProjectile(); // Launch the projectile
            Invoke("StopShooting", spriteRevertDelay); // Reset shooting animation after a delay
        }
    }

    private void PlayShootSound()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.volume = Mathf.Clamp(0.1f, 0f, 1f);
            audioSource.PlayOneShot(shootSound);
        }
    }

    void StopShooting()
    {
        // Stop the shooting animation
        animator.SetBool("isShooting", false);
        mouth.SetActive(false);
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
            // Check if the projectile is null before trying to move it
            if (projectile == null)
            {
                yield break; // Exit the coroutine if the projectile is destroyed
            }

            projectile.transform.position = Vector3.Lerp(startPosition, targetPosition, time);
            time += Time.deltaTime * projectileSpeed;
            yield return null;
        }

        // Destroy the projectile after it reaches the target
        if (projectile != null)
        {
            Destroy(projectile);
        }
    }

    /************************/
    /**** Picking Item logic ****/
    /************************/

    private void BoostUp(float duration, float boostForce){
        rb.velocity = new Vector2(rb.velocity.x, boostForce);
        itemTimer = duration;
    }


}
