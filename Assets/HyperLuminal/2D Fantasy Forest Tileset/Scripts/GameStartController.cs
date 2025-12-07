using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartController : MonoBehaviour
{
    [Header("UI 패널 연결")]
    public GameObject startScreenPanel;
    public GameObject gameOverPanel;

    // 싱글톤 (어디서든 부를 수 있게)
    public static GameStartController instance;

    void Awake() { instance = this; }

    void Start()
    {
        // 처음 켰을 때만 스타트 화면 띄움 (던전 갔다 왔을 땐 안 띄움)
        if (GameManager.instance != null && GameManager.instance.isFirstLoading == true)
        {
            ShowStartScreen();
            GameManager.instance.isFirstLoading = false;
        }
        else
        {
            // 게임 중이면 화면 끄고 진행
            if (startScreenPanel != null) startScreenPanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void ShowStartScreen()
    {
        Time.timeScale = 0;
        if (startScreenPanel != null) startScreenPanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void OnClickStart()
    {
        Time.timeScale = 1;
        if (startScreenPanel != null) startScreenPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void OnClickRetry()
    {
        // 다시 시작하니까 "처음인 척" 설정
        if (GameManager.instance != null) GameManager.instance.isFirstLoading = true;

        // ▼▼▼ 무조건 마을로 이동! (씬 이름 확인하세요) ▼▼▼
        SceneManager.LoadScene("DayTime_DemoScene");
    }
}