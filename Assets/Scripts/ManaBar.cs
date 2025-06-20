using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider manaSlider;
    public TMP_Text manaBarText;

    private PlayerController player;

    private void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("Player GameObject not found. Make sure the player has the 'Player' tag.");
        }
        player = playerObj.GetComponent<PlayerController>();
    }

    void Start()
    {
        UpdateManaUI();
    }

    void Update()
    {
        UpdateManaUI(); // Gọi mỗi frame nếu không có sự kiện thay đổi mana
    }

    void UpdateManaUI()
    {
        manaSlider.value = (float)player.currentMana / player.maxMana;
        manaBarText.text = "Mana " + player.currentMana + " / " + player.maxMana;
    }
}
