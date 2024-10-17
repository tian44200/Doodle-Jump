using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LoseCondition2 : MonoBehaviour
{
    public GameObject uiManager; // Reference to the UIManager
    public string blackHoleTag = "BlackHole"; // Tag for the Black Hole object
    public string monsterTag = "Monster"; // Tag for the Monster object
    public float jumpForce = 30f; // The jump force when Doodle destroys a monster (can be set via Inspector)
    public string projectileTag = "Projectile"; // Tag for the projectile object

    // Detect when Doodle collides with any object
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(blackHoleTag))
        {
            HandleBlackHoleEntry();
        }
        else if (other.CompareTag(monsterTag))
        {
            HandleMonsterCollision(other);
        }
        else if (other.CompareTag(projectileTag))
        {
            HandleProjectileCollision(other);
        }
    }

    void HandleBlackHoleEntry()
    {
        Debug.Log("Doodle entered the black hole!");
        HandleLoseCondition();
    }

    void HandleMonsterCollision(Collider2D monster)
    {
        if (IsHitFromAbove(monster))
        {
            Destroy(monster.gameObject); // Destroy the monster
            ApplyJumpForce();
        }
        else
        {
            HandleLoseCondition();
        }
    }

    bool IsHitFromAbove(Collider2D monster)
    {
        float doodleBottomY = transform.position.y - GetComponent<Collider2D>().bounds.extents.y;
        float monsterTopY = monster.transform.position.y + monster.bounds.extents.y;
        return doodleBottomY >= monsterTopY;
    }

    void ApplyJumpForce()
    {
        Rigidbody2D doodleRb = GetComponent<Rigidbody2D>();
        if (doodleRb != null)
        {
            doodleRb.velocity = new Vector2(doodleRb.velocity.x, 0); // Reset vertical velocity
            doodleRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Apply the jump force
        }
    }

    void HandleLoseCondition()
    {
        Debug.Log("Doodle lost!");
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.SetIsDead(true);
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.OnPlayerDeath();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop player movement
        uiManager.GetComponent<UIManager>().TriggerEndPage(gameObject.tag);
    }

    void HandleProjectileCollision(Collider2D projectile)
    {
        // Handle the collision with the projectile (if necessary, based on design)
    }
}
