using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 10f;

    private float LeftRight;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate() {
        // Get the horizontal input
        LeftRight = Input.GetAxis("Horizontal") * moveSpeed;

        if(LeftRight < 0){
            //Moving Left
            transform.rotation = Quaternion.Euler(180,0,180);
        }else if(LeftRight > 0){
            //Moving Right
            transform.rotation = Quaternion.Euler(0,0,0);
        }

        rb.velocity = new Vector2(LeftRight,rb.velocity.y);
    }
}
