using UnityEngine;

public class RunSoundState : StateMachineBehaviour
{
    private PlayerRunSoundHandler soundHandler;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (soundHandler == null)
        {
            soundHandler = animator.GetComponent<PlayerRunSoundHandler>();
        }

        soundHandler?.PlayRunSound();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        soundHandler?.StopRunSound();
    }
}
