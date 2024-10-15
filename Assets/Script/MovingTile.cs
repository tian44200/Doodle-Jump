using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTile : MonoBehaviour
{

    public float speed;
    private float levelWidth = 2.5f;
    private int direction = 1;


    private void Update(){
        //Move the blue tile between the screen width
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
        if (transform.position.x > levelWidth || transform.position.x < -levelWidth)
        {
            direction *= -1;
        }
    }
}
