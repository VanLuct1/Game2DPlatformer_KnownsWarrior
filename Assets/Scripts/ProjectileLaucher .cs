using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLaucher : MonoBehaviour
{
    public Transform launchPoint; // The point from which the projectile will be launched
    public GameObject projectilePrefab;

    public AudioClip fireSound; // Âm thanh khi bắn
    private AudioSource audioSource; // Component phát âm

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.transform.position, projectilePrefab.transform.rotation);
        Vector3 origScale = projectile.transform.localScale;


        //Flip the projectile based on the direction the launcher is facing
        projectile.transform.localScale = new Vector3(
            transform.localScale.x * origScale.x > 0 ? 1 : -1, 
            origScale.y, 
            origScale.z
            );
        // Phát âm thanh
        if (fireSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }
}
