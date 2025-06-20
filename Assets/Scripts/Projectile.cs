using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public Vector2 moveSpeed = new Vector2(3f, 0);
    public Vector2 knockback = new Vector2(0, 0);

    public AudioClip explosionSound; // Kéo file âm thanh vào trong Inspector
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private Animator anm;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anm = GetComponent<Animator>();

        // Nếu chưa có AudioSource, thêm vào
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Đảm bảo audio không loop
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        // Bay theo hướng nhân vật
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            bool gotHit = damageable.Hit(damage, deliveredKnockback);

            if (gotHit)
            {
                Debug.Log(collision.name + " hit for " + damage);

                // Dừng di chuyển
                rb.velocity = Vector2.zero;

                // Phát âm thanh nếu có
                if (explosionSound != null)
                {
                    audioSource.PlayOneShot(explosionSound);
                }

                // Ẩn hình ảnh và collider để tránh va chạm thêm
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;

                // Hủy sau khi phát xong âm thanh
                StartCoroutine(DestroyAfterSound());
            }
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        if (explosionSound != null)
        {
            yield return new WaitForSeconds(explosionSound.length);
        }
        Destroy(gameObject);
    }
}
