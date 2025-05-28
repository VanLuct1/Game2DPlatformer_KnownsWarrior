using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFloatBehavior : StateMachineBehaviour
{
    public string floatName;
    public bool updateOnStateEnter, updateOnStateExit;
    public bool updateOnStateMachineEnter, updateOnStateMachineExit;
    public float valueOnEnter, valueOnExit;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnStateEnter)
        {
            animator.SetFloat(floatName, valueOnEnter);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(updateOnStateExit)
        {
            animator.SetFloat(floatName, valueOnExit);
        }
    }

    //On
    override public void OnStateMachineEnter(Animator animator, int StateMachinePathHash)
    {
        // Implement code that processes and affects root motion
        if (updateOnStateMachineEnter)
        {
            animator.SetFloat(floatName, valueOnEnter);
        }
    }
    override public void OnStateMachineExit(Animator animator, int StateMachinePathHash)
    {
        // Implement code that processes and affects root motion
        if (updateOnStateMachineExit)
        {
            animator.SetFloat(floatName, valueOnExit);
        }
    }
    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
