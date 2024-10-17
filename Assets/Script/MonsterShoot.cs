using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShoot : MonoBehaviour
{
    public string projectileTag = "Projectile";

    // Detect when the monster is hit by a projectile
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(projectileTag))
        {
            Destroy(this.gameObject); // Destroy the monster
            Destroy(other.gameObject); // Destroy the projectile
            Debug.Log("Monster destroyed by projectile!");
        }
    }
}
