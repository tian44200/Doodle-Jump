using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LoseCondition : MonoBehaviour
{
    public GameObject uiManager; // Reference to the UIManager
    public string blackHoleTag = "BlackHole"; // Tag for the Black Hole object
    public string monsterTag = "Monster"; // Tag for the Monster object
    public float jumpForce = 30f; // The jump force when Doodle destroys a monster (can be set via Inspector)
    public float suckSpeed = 0.5f; // The speed at which the Doodle is sucked into the black hole
    private bool isBeingSucked = false; // Flag to check if the Doodle is being sucked into the black hole
    private Collider2D other; // Reference to the Doodle's collider
    private string destroyerTag = "FallCollider"; // Tag for the destroyer object
    public AudioSource jumpSound; // Reference to the AudioSource for the jump sound
    private bool isHandlingBlackHole = false;


    void Update()
    {
        if (isBeingSucked)
        {
            Debug.Log("Doodle's scale: " + transform.localScale);
            MoveTowardsBlackHole();
        }
    }

    // Called when another collider enters the trigger collider attached to this object.
    void OnTriggerEnter2D(Collider2D other)
    {
        this.other = other;
        if (other.CompareTag(blackHoleTag) && !isHandlingBlackHole)
        {
            isHandlingBlackHole = true;
            PlayerControl playerControl = GetComponent<PlayerControl>();
            playerControl.SetDead(true);
            StartCoroutine(HandleBlackHoleEntry(other.transform));
        }
        else if (other.CompareTag(monsterTag))
        {
            HandleMonsterCollision(other);

        }
        else if (other.CompareTag(destroyerTag))
        {
            HandleLoseCondition();
        }
    }

    IEnumerator HandleBlackHoleEntry(Transform blackHoleTransform)
    {
        Debug.Log("Doodle entered the black hole!");

        // Disable Doodle's gravity and movement
        if (TryGetComponent<Rigidbody2D>(out var doodleRb))
        {
            doodleRb.gravityScale = 0;
            doodleRb.velocity = Vector2.zero;
            doodleRb.isKinematic = true; // Ensure physics don't interfere
        }

        Vector3 startPosition = transform.position;
        Vector3 startScale = transform.localScale;
        float duration = 1f; // Total duration of the effect
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Move towards black hole
            transform.position = Vector3.Lerp(startPosition, blackHoleTransform.position, t);

            // Shrink Doodle
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            yield return null;
        }

        // Ensure final position and scale
        transform.position = blackHoleTransform.position;
        transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(0.1f); // Short additional delay

        HandleLoseCondition();
    }

    // Moves Doodle towards the black hole center.
    void MoveTowardsBlackHole()
    {
        Transform doodleTransform = transform;
        Transform blackHoleTransform = other.transform;

        if (doodleTransform != null && blackHoleTransform != null)
        {
            // Lerp Doodle towards the black hole
            doodleTransform.position = Vector2.Lerp(doodleTransform.position, blackHoleTransform.position, suckSpeed * Time.deltaTime);

            // When close enough to the black hole, stop the movement
            float distance = Vector2.Distance(doodleTransform.position, blackHoleTransform.position);
            if (distance < 0.1f)
            {
                Debug.Log("Doodle reached the black hole!");
                isBeingSucked = false; // Stop sucking
                // HandleLoseCondition();
            }
        }
    }

    // Handles collision with a monster.
    void HandleMonsterCollision(Collider2D monster)
    {
        if (IsHitFromAbove(monster))
        {
            Destroy(monster.gameObject); // Destroy the monster
            ApplyJumpForce();
            // Play the sound before destroying the object
            if (jumpSound != null)
            {
                jumpSound.Play();
            }
            // Destroy the monster with a delay to allow the sound to play
            Destroy(monster.gameObject, jumpSound != null ? jumpSound.clip.length : 0f); // Delay destruction by the length of the audio clip
        }
        else
        {
            // Activate StarEffects under Doodle
            Transform starEffectsTransform = transform.Find("StarEffects");
            if (starEffectsTransform != null)
            {
                GameObject starEffects = starEffectsTransform.gameObject;
                starEffects.SetActive(true);  // Activate StarEffects effect
            }
            else
            {
                Debug.LogError("StarEffects not found under Doodle.");
            }

            HandleLoseCondition();
        }
    }




// Checks if Doodle is hitting the monster from above.
bool IsHitFromAbove(Collider2D monster)
{
    float doodleBottomY = transform.position.y - GetComponent<Collider2D>().bounds.extents.y;
    float monsterTopY = monster.transform.position.y + monster.bounds.extents.y;
    return doodleBottomY >= monsterTopY;
}

// Applies jump force to Doodle after killing a monster.
void ApplyJumpForce()
{
    Rigidbody2D doodleRb = GetComponent<Rigidbody2D>();
    if (doodleRb != null)
    {
        doodleRb.velocity = new Vector2(doodleRb.velocity.x, 0); // Reset vertical velocity
        doodleRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Apply the jump force
    }
}

// Handles the lose condition and triggers the end page.
void HandleLoseCondition()
{
    Debug.Log("Doodle lost!");
    GameManager gameManager = FindObjectOfType<GameManager>();
    gameManager.SetIsDead(true);
    PlayerControl playerControl = GetComponent<PlayerControl>();
    playerControl.SetDead(true);
    GetComponent<Collider2D>().enabled = false; // Disable further collisions

    ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
    scoreManager.OnPlayerDeath();
    GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop player movement

    UIManager uiManager = FindObjectOfType<UIManager>();
    if (uiManager != null)
    {
        uiManager.TriggerEndPage("BlackHole");
    }
    else
    {
        Debug.LogError("UIManager not found!");
    }

}

} 
