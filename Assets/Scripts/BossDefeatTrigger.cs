using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDefeatTrigger : MonoBehaviour
{
    public Damageable bossDamageable;
    public string nextSceneName = "Menu"; // Tên màn sau khi qua

    void Start()
    {
        if (bossDamageable != null)
        {
            bossDamageable.damageableDeath.AddListener(OnBossDefeated);
        }
    }

    void OnBossDefeated()
    {
        Debug.Log("Boss defeated! Loading next scene...");

        // Tuỳ chọn delay vài giây
        StartCoroutine(LoadNextSceneAfterDelay(2f));
    }

    IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextSceneName);
    }
}
