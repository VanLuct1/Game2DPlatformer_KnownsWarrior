using UnityEngine;

public class FlyingEyeSound : MonoBehaviour
{
    public AudioSource wingFlapSource;

    public void PlayWingFlap()
    {
        if (!wingFlapSource.isPlaying)
            wingFlapSource.Play();
    }

    public void StopWingFlap()
    {
        if (wingFlapSource.isPlaying)
            wingFlapSource.Stop();
    }
}
