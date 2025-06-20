using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Thoát game"); // Để kiểm tra khi chạy trong Editor
        Application.Quit();
    }
}
