using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public float jumpForce;


    private void OnCollisionEnter2D(Collision2D other) {
        Rigidbody2D doodleRB = other.transform.GetComponent<Rigidbody2D>();

        //If Doodle is falling 
        if(doodleRB.velocity.y <= 1e-6){
            doodleRB.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);

        }

    }
}
