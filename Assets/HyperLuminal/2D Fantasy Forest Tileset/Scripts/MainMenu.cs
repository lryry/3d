using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동을 위해 필수!

public class MainMenu : MonoBehaviour
{
    public void GameStart()
    {
        // 1. 게임을 새로 시작하니까, 혹시 남아있을지 모를 데이터를 초기화합니다.
        if (GameManager.instance != null)
        {
            GameManager.instance.playerCurrentHP = 10; // 체력 복구
            GameManager.instance.inventoryItems = new Sprite[10]; // 인벤토리 비우기
            // GameManager.instance.playerMoney = 0; // (나중에 돈 기능 넣으면 주석 해제)
        }
        SceneManager.LoadScene("DayTime_DemoScene");
    }
}