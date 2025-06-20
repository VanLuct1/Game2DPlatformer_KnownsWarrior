using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage = 30;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Damageable playerDamageable = collision.GetComponent<Damageable>();
            if (playerDamageable != null)
            {
                playerDamageable.TakeDamage(damage); // Gây sát thương
            }

            Destroy(gameObject); // Hủy cầu lửa sau khi gây đam
        }

        // Nếu trúng đất cũng hủy luôn (tuỳ chọn)
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
