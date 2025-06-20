using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BossHealthBar : MonoBehaviour
{
    public Damageable bossDamageable;      // Gán Damageable trên Boss
    public Slider healthSlider;            // Gán Slider máu
    public Text healthText;                // Gán Text để hiển thị số máu
    public Vector3 offset;                 // Nếu canvas là overlay, có thể bỏ offset

    void Start()
    {
        if (bossDamageable != null)
        {
            bossDamageable.healthChanged.AddListener(UpdateHealthUI);
            UpdateHealthUI(bossDamageable.Health, bossDamageable.MaxHealth);
        }
    }

    void Update()
    {
        if (bossDamageable != null && Camera.main != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(bossDamageable.transform.position + offset);
            transform.position = screenPos;
        }
    }

    void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth + " / " + maxHealth;
        }
    }
}
