using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSpringAnim : StateMachineBehaviour
{


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Stop the animation of the spring
       animator.SetBool("Animate",false);
    }

}
