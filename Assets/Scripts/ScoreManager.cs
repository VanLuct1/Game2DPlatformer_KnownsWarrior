using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI text;
    int score;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Gán lại text nếu null
        TryAssignText();
    }

    void OnEnable()
    {
        TryAssignText();
    }

    private void TryAssignText()
    {
        if (text == null)
        {
            GameObject textObj = GameObject.Find("CoinsSoluong"); // đúng tên object hiển thị số coin
            if (textObj != null)
            {
                text = textObj.GetComponent<TextMeshProUGUI>();
                text.text = "X" + score.ToString();
            }
        }
    }

    public void ChangeScore(int coinValue)
    {
        score += coinValue;
        if (text != null)
        {
            text.text = "X" + score.ToString();
        }
    }
}
