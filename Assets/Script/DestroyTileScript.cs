using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTileScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        //destroy all the element below the screen
        Destroy(other.gameObject);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        Destroy(other.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Monster"|| other.tag == "BlackHole" || other.tag == "Platform" || other.tag == "Spring" || other.tag == "Hat" ||other.tag == "JetPack"){
            Destroy(other.gameObject);
        }
    }
}
