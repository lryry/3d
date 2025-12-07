using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("체력 설정")]
    public int maxHP = 10;
    public int currentHP;

    [Header("UI 연결")]
    public Slider hpSlider;

    void Start()
    {
        // ▼▼▼ [추가된 부분] 1. 체력바 자동 연결 (연결 끊김 방지!) ▼▼▼
        // 만약 인스펙터에서 연결이 끊겨있다면(null), 스스로 찾습니다.
        if (hpSlider == null)
        {
            // 씬에서 "HP_Bar"라는 이름의 오브젝트를 찾습니다.
            // (주의: 하이에라키 창에서 슬라이더 이름을 꼭 "HP_Bar"로 맞춰주세요!)
            GameObject sliderObj = GameObject.Find("HP_Bar");
            if (sliderObj != null)
            {
                hpSlider = sliderObj.GetComponent<Slider>();
            }
        }
        // ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

        // 2. GameManager에서 체력 데이터 불러오기
        if (GameManager.instance != null)
        {
            currentHP = GameManager.instance.playerCurrentHP;
        }
        else
        {
            currentHP = maxHP;
        }

        // 3. [안전장치] 죽어서 돌아왔을 때 HP가 0으로 시작하는 문제 해결
        if (currentHP <= 0)
        {
            currentHP = maxHP; // 강제로 풀피로 만듦
            if (GameManager.instance != null)
            {
                GameManager.instance.playerCurrentHP = maxHP; // 매니저에도 저장
            }
        }

        UpdateSlider();
        Debug.Log("HP: " + currentHP);
    }

    void UpdateSlider()
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // ▼▼▼ 죽음 처리 ▼▼▼
        if (currentHP <= 0)
        {
            currentHP = 0;

            // 1. 데이터 초기화 (인벤토리 비우기 & HP 복구 준비)
            if (GameManager.instance != null)
            {
                GameManager.instance.playerCurrentHP = maxHP;
                GameManager.instance.inventoryItems = new Sprite[10];
            }

            // 2. 게임 오버 화면 띄우기 (StartManager에 요청)
            if (GameStartController.instance != null)
            {
                GameStartController.instance.ShowGameOver();
            }
        }
        // ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

        else // 안 죽었으면 현재 체력 저장
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.playerCurrentHP = currentHP;
            }
        }

        UpdateSlider();
        Debug.Log("HP가 " + currentHP + " 남았습니다.");
    }

    public void HealToFull()
    {
        currentHP = maxHP;
        if (GameManager.instance != null)
        {
            GameManager.instance.playerCurrentHP = currentHP;
        }
        UpdateSlider();
        Debug.Log("풀피로 회복되었습니다!");
    }
}