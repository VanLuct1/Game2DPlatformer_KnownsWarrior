using UnityEngine;

public class SkeletonSound : MonoBehaviour
{
    public AudioSource footstepAudio;

    public void PlayFootsteps()
    {
        if (!footstepAudio.isPlaying)
            footstepAudio.Play();
    }

    public void StopFootsteps()
    {
        if (footstepAudio.isPlaying)
            footstepAudio.Stop();
    }
}
