using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int damage = 20; // Số máu trừ

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Damageable playerDamage = other.GetComponent<Damageable>();
            if (playerDamage != null)
            {
                playerDamage.TakeDamage(damage);
                Debug.Log("Player trúng bẫy, mất " + damage + " máu");
            }
        }
    }
}
