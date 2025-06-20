using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalTrigger : MonoBehaviour
{
    public string sceneToLoad; // Tên màn hình cần chuyển

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered portal, loading scene: " + sceneToLoad);
            StartCoroutine(LoadNextScene());
        }
    }

    // 👉 Coroutine chờ 1 giây rồi chuyển scene
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneToLoad);
    }
}
