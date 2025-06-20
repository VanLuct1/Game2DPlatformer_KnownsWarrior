using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 20; // Amount of health restored by this pickup
    public int manaRestore = 20;   // Amount of mana restored
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        PlayerController player = collision.GetComponent<PlayerController>();

        if (damageable)
        {
            damageable.Heal(healthRestore);
        }

        if (player)
        {
            player.currentMana = Mathf.Min(player.maxMana, player.currentMana + manaRestore);
        }

        Destroy(gameObject); // Destroy the pickup after use
    }

    void Update()
    {
        // Rotate the health pickup for visual effect
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}
