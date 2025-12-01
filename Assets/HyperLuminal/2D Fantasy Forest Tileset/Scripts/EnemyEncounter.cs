using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class MonsterInfo
{
    public string monsterName;
    public Sprite battleSprite;
    public int hp;
}

public class EnemyEncounter : MonoBehaviour
{
    [Header("이 자리에서 나올 몬스터 목록")]
    public List<MonsterInfo> monsterPool = new List<MonsterInfo>();

    [Header("잠금 설정")]
    public bool requiresUnlock = false;

    [Header("문 그림 설정 (선택 사항)")]
    public Sprite OpenSprite;
    public Sprite ClosedSprite;
    private SpriteRenderer spriteRenderer;

    private bool hasTriggeredBattle = false;

    // ▼▼▼ [추가됨!] '현재' 문 상태 (열림/닫힘) ▼▼▼
    private bool isDoorOpen = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isDoorOpen = false; // '시작'은 '닫힘' 상태

        if (requiresUnlock && spriteRenderer != null && ClosedSprite != null)
        {
            spriteRenderer.sprite = ClosedSprite;
        }
    }

    // ▼▼▼ [추가됨!] 'Trigger.cs'(상자)가 '원격'으로 '호출'할 '그림 변경' 함수! ▼▼▼
    public void ToggleObject()
    {
        // '닫혀'있었다면
        if (!isDoorOpen)
        {
            isDoorOpen = true; // '열림' 상태로 변경
            if (spriteRenderer != null && OpenSprite != null)
            {
                spriteRenderer.sprite = OpenSprite; // '열린' 그림으로
            }
        }
        else // '열려'있었다면
        {
            isDoorOpen = false; // '닫힘' 상태로 변경
            if (spriteRenderer != null && ClosedSprite != null)
            {
                spriteRenderer.sprite = ClosedSprite; // '닫힌' 그림으로
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggeredBattle)
        {
            if (requiresUnlock)
            {
                // 'GameManager'에 '신호'가 '없다면' (상자를 안 먹었다면)
                if (GameManager.instance != null && !GameManager.instance.isDungeonDoorUnlocked)
                {
                    Debug.Log("문이 잠겨있다. 상자를 먼저 찾아야 한다.");
                    return;
                }

                // '상자'를 '이미' 먹었으니 '그림'을 '열린' 상태로 '유지' (혹시 모르니)
                if (spriteRenderer != null && OpenSprite != null && !isDoorOpen)
                {
                    isDoorOpen = true;
                    spriteRenderer.sprite = OpenSprite;
                }
            }

            if (monsterPool.Count == 0)
            {
                Debug.LogError("EnemyEncounter: 'Monster Pool'에 몬스터가 1마리도 없습니다!");
                return;
            }

            MonsterInfo chosenMonster = monsterPool[Random.Range(0, monsterPool.Count)];

            if (GameManager.instance != null)
            {
                GameManager.instance.currentEnemyName = chosenMonster.monsterName;
                GameManager.instance.currentEnemyBattleSprite = chosenMonster.battleSprite;
                GameManager.instance.currentEnemyHP = chosenMonster.hp;
            }
            else
            {
                Debug.LogError("GameManager가 씬에 없습니다!");
                return;
            }

            Debug.Log(chosenMonster.monsterName + "을(를) 만났습니다.");

            hasTriggeredBattle = true;
            SceneManager.LoadScene("Next");
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "DayTime_DemoScene")
        {
            hasTriggeredBattle = false;

            // '마을'로 '돌아오면' '문'을 '다시' '닫습니다'!
            if (requiresUnlock && spriteRenderer != null && ClosedSprite != null)
            {
                isDoorOpen = false; // '닫힘' 상태로
                spriteRenderer.sprite = ClosedSprite;
            }
        }
    }
}