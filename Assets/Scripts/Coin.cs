using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1; // Giá trị của đồng xu
    public AudioClip pickupSound; // Gắn clip âm thanh trong Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Phát âm thanh tại vị trí đồng xu
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Cộng điểm
            ScoreManager.instance.ChangeScore(coinValue);

            // Hủy đối tượng coin
            Destroy(gameObject);
        }
    }
}
