using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public TextMeshProUGUI battleText;
    public Button attackButton;
    public Button runButton;

    int enemyHP;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("BattleSystem: '씬'에서 'PlayerHealth' 스크립트를 찾을 수 없습니다!");
        }

        if (GameManager.instance != null)
        {
            string encounteredEnemyName = GameManager.instance.currentEnemyName;
            Sprite encounteredEnemySprite = GameManager.instance.currentEnemyBattleSprite;
            enemyHP = GameManager.instance.currentEnemyHP;

            if (enemy != null && encounteredEnemySprite != null)
            {
                SpriteRenderer enemySpriteRenderer = enemy.GetComponent<SpriteRenderer>();
                if (enemySpriteRenderer != null)
                {
                    enemySpriteRenderer.sprite = encounteredEnemySprite;
                }
            }

            battleText.text = encounteredEnemyName + " 이(가) 나타났다!";
        }
        else
        {
            Debug.LogError("GameManager가 씬에 없습니다! 적 정보를 불러올 수 없습니다.");
            battleText.text = "몬스터다!";
            enemyHP = 5;
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
        battleText.text = "공격! 몬스터에게 " + damage + "데미지를 입혔다";
        if (enemyHP <= 0)
        {
            battleText.text = "몬스터를 물리쳤다!";
            Invoke("ReturnToMainScene", 1.5f);
        }
        else
        {
            Invoke("EnemyTurn", 1.0f);
        }
    }

    void EnemyTurn()
    {
        int damage = Random.Range(1, 3);
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
        battleText.text = "몬스터 반격 " + damage + "데미지를 입었다";
        if (GameManager.instance.playerCurrentHP <= 0)
        {
            battleText.text = "쓰러졌다";
            Invoke("ReturnToMainScene", 1.5f);
        }
        else
        {
            attackButton.interactable = true;
            runButton.interactable = true;
        }
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
        SceneManager.LoadScene("DayTime_DemoScene");
    }
}