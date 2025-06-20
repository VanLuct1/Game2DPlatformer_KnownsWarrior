using UnityEngine;

public class SkeletonRunSound : StateMachineBehaviour
{
    private SkeletonSound sound;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (sound == null)
            sound = animator.GetComponent<SkeletonSound>();

        sound?.PlayFootsteps();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sound?.StopFootsteps();
    }
}
