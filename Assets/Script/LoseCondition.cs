using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// The LoseCondition class is responsible for handling various lose conditions in the game.
/// It detects collisions with different objects such as black holes, monsters, and projectiles,
/// and triggers appropriate responses. When the player (Doodle) collides with these objects,
/// the class determines whether the player should lose the game or if other actions should be taken,
/// such as destroying a monster or applying a jump force. It also interacts with the UIManager
/// to trigger the end page and with the ScoreManager to handle score updates when the player loses.
/// </summary>
public class LoseCondition : MonoBehaviour
{
    public GameObject uiManager; // Reference to the UIManager
    public string blackHoleTag = "BlackHole"; // Tag for the Black Hole object
    public string monsterTag = "Monster"; // Tag for the Monster object
    public float jumpForce = 30f; // The jump force when Doodle destroys a monster (can be set via Inspector)
    public string projectileTag = "Projectile"; // Tag for the projectile object

    /// <summary>
    /// Sets the UIManager reference.
    /// </summary>
    /// <param name="ui">The UIManager GameObject.</param>
    public void SetUIManager(GameObject ui)
    {
        uiManager = ui;
    }

    /// <summary>
    /// Called when another collider enters the trigger collider attached to this object.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Doodle"))
        {
            if (CompareTag(blackHoleTag))
            {
                HandleBlackHoleEntry(other);
            }
            else if (CompareTag(monsterTag))
            {
                HandleMonsterCollision(other);
            }
            else
            {
                HandleLoseCondition(other);
            }
        }
        else if (other.CompareTag(projectileTag))
        {
            HandleProjectileCollision(other);
        }
    }

    /// <summary>
    /// Handles entry into a black hole.
    /// </summary>
    /// <param name="doodle">The Doodle collider.</param>
    void HandleBlackHoleEntry(Collider2D doodle)
    {
        Debug.Log("Doodle entered the black hole!");
        HandleLoseCondition(doodle);
    }

    /// <summary>
    /// Handles collision with a monster.
    /// </summary>
    /// <param name="doodle">The Doodle collider.</param>
    void HandleMonsterCollision(Collider2D doodle)
    {
        if (IsHitFromAbove(doodle))
        {
            Destroy(gameObject); // Destroy the monster
            ApplyJumpForce(doodle);
        }
        else
        {
            // 从doodle的子对象中找到StarEffects
            Transform starEffectsTransform = doodle.transform.Find("StarEffects");
            if (starEffectsTransform != null)
            {
                GameObject starEffects = starEffectsTransform.gameObject;
                starEffects.SetActive(true);  // 激活StarEffects效果
            }
            else
            {
                Debug.LogError("StarEffects not found under Doodle.");
            }
            HandleLoseCondition(doodle);
        }
    }

    /// <summary>
    /// Checks if the Doodle is hitting the monster from above.
    /// </summary>
    /// <param name="doodle">The Doodle collider.</param>
    /// <returns>True if the Doodle is hitting from above, false otherwise.</returns>
    bool IsHitFromAbove(Collider2D doodle)
    {
        float doodleBottomY = doodle.transform.position.y - doodle.bounds.extents.y;
        float monsterTopY = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;
        return doodleBottomY >= monsterTopY;
    }

    /// <summary>
    /// Applies jump force to the Doodle.
    /// </summary>
    /// <param name="doodle">The Doodle collider.</param>
    void ApplyJumpForce(Collider2D doodle)
    {
        Rigidbody2D doodleRb = doodle.GetComponent<Rigidbody2D>();
        if (doodleRb != null)
        {
            doodleRb.velocity = new Vector2(doodleRb.velocity.x, 0); // Reset vertical velocity
            doodleRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Apply the jump force
        }
    }

    /// <summary>
    /// Handles the lose condition and triggers the end page.
    /// </summary>
    /// <param name="doodle">The Doodle collider.</param>
    void HandleLoseCondition(Collider2D doodle)
    {
        Debug.Log("Doodle lost!");
        // 禁用 Doodle 的碰撞体，避免进一步的碰撞
        Collider2D doodleCollider = doodle.GetComponent<Collider2D>();
        if (doodleCollider != null)
        {
            doodleCollider.enabled = false;  // 禁用碰撞
        }
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.OnPlayerDeath();
        uiManager.GetComponent<UIManager>().TriggerEndPage();
        doodle.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop player movement
    }

    /// <summary>
    /// Handles when a projectile hits the monster.
    /// </summary>
    /// <param name="projectile">The projectile collider.</param>
    void HandleProjectileCollision(Collider2D projectile)
    {
        if (this.CompareTag(monsterTag))
        {
            Destroy(this.gameObject); // Destroy the monster
            Destroy(projectile.gameObject); // Destroy the projectile
            Debug.Log("Monster destroyed by projectile!");
        }
    }
}