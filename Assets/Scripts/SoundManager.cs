using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource; // Trường để gán AudioSource trong Inspector

    public void PlayGameOverSound(AudioClip gameOverSound)
    {
        if (gameOverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(gameOverSound, 0.8f);
        }
        else
        {
            Debug.LogWarning("GameOverSound hoặc AudioSource không được gán!");
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Giữ SoundManager khi chuyển scene
    }
}