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
    public string projectileTag = "Projectile"; // Tag for the projectile object

    public void SetUIManager(GameObject ui)
    {
        uiManager = ui;
    }

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

    // Handles entry into a black hole
    void HandleBlackHoleEntry(Collider2D doodle)
    {
        Debug.Log("Doodle entered the black hole!");
        HandleLoseCondition(doodle);
    }
    /**********************************/
    /****** HandleMonsterCollision ****/
    /**********************************/

    // Handles collision with a monster
    void HandleMonsterCollision(Collider2D doodle)
    {
        if (IsHitFromAbove(doodle))
        {
            Destroy(gameObject); // Destroy the monster
            ApplyJumpForce(doodle);
        }
        else
        {
            HandleLoseCondition(doodle);
        }
    }

    // Checks if the Doodle is hitting the monster from above
    bool IsHitFromAbove(Collider2D doodle)
    {
        float doodleBottomY = doodle.transform.position.y - doodle.bounds.extents.y;
        float monsterTopY = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;
        return doodleBottomY >= monsterTopY;
    }

    // Applies jump force to the Doodle
    void ApplyJumpForce(Collider2D doodle)
    {
        Rigidbody2D doodleRb = doodle.GetComponent<Rigidbody2D>();
        if (doodleRb != null)
        {
            doodleRb.velocity = new Vector2(doodleRb.velocity.x, 0); // Reset vertical velocity
            doodleRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Apply the jump force
        }
    }

    /**************************************/
    /******** Handle Other Functions ******/
    /**************************************/
    // Handles the lose condition and triggers the end page
    void HandleLoseCondition(Collider2D doodle)
    {
        Debug.Log("Doodle lost!");
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.OnPlayerDeath();
        uiManager.GetComponent<UIManager>().TriggerEndPage();
        doodle.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop player movement
    }

    // Method to handle when a projectile hits the monster
    void HandleProjectileCollision(Collider2D projectile)
    {
        Destroy(this.gameObject); // Destroy the monster
        Destroy(projectile.gameObject); // Destroy the projectile
        Debug.Log("Monster destroyed by projectile!");
    }
}
