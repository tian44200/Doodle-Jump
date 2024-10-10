using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject toFollow;
    private void FixedUpdate() {
        if(toFollow.transform.position.y > transform.position.y){
            if(toFollow.transform.GetComponent<Rigidbody2D>().velocity.y >0){
                Vector3 newPos = new Vector3(transform.position.x, toFollow.transform.position.y, transform.position.z);

                transform.position = newPos;
            }
        }
    }
}
