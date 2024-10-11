using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Doodle" && other.transform.GetComponent<Rigidbody2D>().velocity.y < 1e-6){
            anim.SetBool("Animate",true);
        }
    }
}
