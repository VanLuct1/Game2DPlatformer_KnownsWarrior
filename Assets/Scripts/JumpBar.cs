using UnityEngine;
using TMPro;

public class JumpBar : MonoBehaviour
{
    public GameObject tutorialPopup; // UI hộp thoại
    public string message; // Nội dung hiển thị
    public TextMeshProUGUI textUI;   // TMP_Text để đổi nội dung

    private void Start()
    {
        if (tutorialPopup != null)
            tutorialPopup.SetActive(false);

        if (textUI == null)
            Debug.LogWarning("Text UI is not assigned in JumpBar.");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (textUI != null)
                textUI.text = message;

            if (tutorialPopup != null)
                tutorialPopup.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutorialPopup != null)
                tutorialPopup.SetActive(false);
        }
    }

}
