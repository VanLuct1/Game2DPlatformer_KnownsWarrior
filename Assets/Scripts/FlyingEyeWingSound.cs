using UnityEngine;

public class FlyingEyeWingSound : StateMachineBehaviour
{
    private FlyingEyeSound flySound;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (flySound == null)
            flySound = animator.GetComponent<FlyingEyeSound>();

        flySound?.PlayWingFlap();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flySound?.StopWingFlap();
    }
}
