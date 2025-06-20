using TMPro;
using UnityEngine;

public class SceneUIManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    void Start()
    {
        if (ScoreManager.instance != null && coinText != null)
        {
            ScoreManager.instance.text = coinText;
            ScoreManager.instance.ChangeScore(0); // cập nhật lại hiển thị
        }
    }
}
