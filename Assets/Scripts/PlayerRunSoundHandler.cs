using UnityEngine;

public class PlayerRunSoundHandler : MonoBehaviour
{
    public AudioSource runAudioSource;
    public AudioClip runClip;

    private void Awake()
    {
        runAudioSource.clip = runClip;
        runAudioSource.loop = true;
        runAudioSource.playOnAwake = false;
    }

    public void PlayRunSound()
    {
        if (!runAudioSource.isPlaying)
            runAudioSource.Play();
    }

    public void StopRunSound()
    {
        if (runAudioSource.isPlaying)
            runAudioSource.Stop();
    }
}
