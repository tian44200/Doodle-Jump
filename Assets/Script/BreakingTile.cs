using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingTile : MonoBehaviour
{
    private Animator anim;
    public AudioSource audioSource; // Reference to the AudioSource

    void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Doodle") && other.transform.GetComponent<Rigidbody2D>().velocity.y < 1e-6)
        {
            anim.SetBool("Animate", true);
            PlayBreakingSound(); // Call the method to play sound
        }
    }

    void PlayBreakingSound()
    {
        if (audioSource != null)
        {
            audioSource.Play(); // Play the sound
        }
    }
}
