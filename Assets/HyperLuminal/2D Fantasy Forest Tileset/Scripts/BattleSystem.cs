using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{
    [Header("연결 대상")]
    public GameObject player;
    public GameObject enemy;

    [Header("배경 설정")]
    public SpriteRenderer backgroundRenderer;
    public Sprite dungeonSprite;
    public Sprite nightSprite;

    // ▼▼▼ [중요] 여기에 코인 아이템 그림(동전)을 꼭 넣으세요! ▼▼▼
    [Header("보상 아이템")]
    public Sprite coinItemSprite;
    // ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

    [Header("UI 연결")]
    public TextMeshProUGUI battleText;
    public Button attackButton;
    public Button runButton;

    int enemyHP;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null && player == null) player = playerHealth.gameObject;

        // 배경 교체 (밤/낮)
        if (GameManager.instance != null && backgroundRenderer != null)
        {
            if (GameManager.instance.previousSceneName == "NightTime_DemoScene")
                backgroundRenderer.sprite = nightSprite;
            else
                backgroundRenderer.sprite = dungeonSprite;
        }

        // 적 정보 불러오기
        if (GameManager.instance != null)
        {
            string encounteredEnemyName = GameManager.instance.currentEnemyName;
            Sprite encounteredEnemySprite = GameManager.instance.currentEnemySprite;
            enemyHP = GameManager.instance.currentEnemyHP;

            if (enemy != null && encounteredEnemySprite != null)
            {
                enemy.GetComponent<SpriteRenderer>().sprite = encounteredEnemySprite;
            }
            battleText.text = encounteredEnemyName + " (이)가 나타났다!";
        }

        attackButton.onClick.AddListener(OnAttack);
        runButton.onClick.AddListener(OnRun);
    }

    void OnAttack()
    {
        attackButton.interactable = false;
        runButton.interactable = false;

        int damage = Random.Range(1, 3);
        enemyHP -= damage;
        battleText.text = "공격! " + damage + " 데미지를 입혔다";

        if (enemyHP <= 0)
        {
            // ====================================================
            // 1. [코인 보상 지급] 
            // ====================================================
            int dropCount = 0;
            if (GameManager.instance != null)
            {
                dropCount = GameManager.instance.currentRewardGold; // 몬스터별 설정된 골드 개수
            }

            int actualAdded = 0; // 실제로 가방에 들어간 개수

            if (GameManager.instance != null && coinItemSprite != null)
            {
                // 설정된 개수(5개면 5번)만큼 반복해서 넣기
                for (int k = 0; k < dropCount; k++)
                {
                    // 빈칸 찾기
                    for (int i = 0; i < GameManager.instance.inventoryItems.Length; i++)
                    {
                        Sprite currentItem = GameManager.instance.inventoryItems[i];
                        Sprite emptySprite = GameManager.instance.emptySlotSprite;

                        // 빈칸이면 넣기
                        if (currentItem == null || (emptySprite != null && currentItem.name == emptySprite.name))
                        {
                            GameManager.instance.inventoryItems[i] = coinItemSprite;
                            actualAdded++;
                            break; // 빈칸 하나 채웠으면 다음 코인으로
                        }
                    }
                }
            }

            // ====================================================
            // 2. [경험치 보상 지급]
            // ====================================================
            int expAmount = 0;
            if (GameManager.instance != null)
            {
                expAmount = GameManager.instance.currentRewardExp; // 몬스터별 설정된 경험치
                GameManager.instance.AddExp(expAmount);
            }

            // ====================================================
            // 3. [결과 메시지 출력]
            // ====================================================
            battleText.text = "승리!\n";

            if (actualAdded > 0)
                battleText.text += "코인 " + actualAdded + "개 ";
            else
                battleText.text += "(가방 꽉 찼다) ";

            battleText.text += "/ Exp +" + expAmount;

            Invoke("ReturnToMainScene", 2.0f);
        }
        else
        {
            Invoke("EnemyTurn", 1.0f);
        }
    }

    void EnemyTurn()
    {
        int damage = Random.Range(1, 3);
        if (playerHealth != null) playerHealth.TakeDamage(damage);
        battleText.text = "반격! " + damage + " 데미지를 입었다";

        if (playerHealth != null && playerHealth.currentHP <= 0)
        {
            attackButton.interactable = false;
            runButton.interactable = false;

            // 죽으면 가방 비우기
            if (GameManager.instance != null)
            {
                GameManager.instance.ClearInventory();
            }

            Invoke("GameOver", 2.0f);
        }
        else
        {
            attackButton.interactable = true;
            runButton.interactable = true;
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("DayTime_DemoScene");
    }

    void OnRun()
    {
        battleText.text = "도망쳤다";
        attackButton.interactable = false;
        runButton.interactable = false;
        Invoke("ReturnToMainScene", 1.5f);
    }

    void ReturnToMainScene()
    {
        if (GameManager.instance != null && !string.IsNullOrEmpty(GameManager.instance.previousSceneName))
        {
            SceneManager.LoadScene(GameManager.instance.previousSceneName);
        }
        else
        {
            SceneManager.LoadScene("DayTime_DemoScene");
        }
    }
}