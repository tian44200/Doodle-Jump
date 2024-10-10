using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTileScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        //destroy all the element below the screen
        Destroy(other.gameObject);
    }
}
