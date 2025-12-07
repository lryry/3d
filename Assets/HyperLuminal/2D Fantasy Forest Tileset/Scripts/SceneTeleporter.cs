using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동을 위해 필수!

public class SceneTeleporter : MonoBehaviour
{
    [Header("이동할 씬 이름")]
    public string targetSceneName; // 인스펙터에서 직접 적을 수 있어요!

    // 플레이어가 닿으면 실행되는 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 닿은 게 '플레이어'라면?
        if (collision.CompareTag("Player"))
        {
            // 설정한 씬으로 이동!
            SceneManager.LoadScene(targetSceneName);
        }
    }
}