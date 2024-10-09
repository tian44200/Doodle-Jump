using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public float jumpForce = 10;


    private void OnCollisionEnter2D(Collision2D other) {
        Rigidbody2D doodleRB = other.transform.GetComponent<Rigidbody2D>();


        if(doodleRB.velocity.y < 0){
            doodleRB.AddForce(Vector2.up*jumpForce);
        }

    }
}
