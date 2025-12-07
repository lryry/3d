using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isFirstLoading = true;

    [Header("플레이어 데이터")]
    public int playerMaxHP = 10;
    public int playerCurrentHP = 10;
    public int playerMoney = 0;

    [Header("레벨 시스템")]
    public int level = 0;
    public int currentExp = 0;
    public int maxExp = 10;

    [Header("인벤토리 데이터")]
    public Sprite[] inventoryItems = new Sprite[10];
    public int[] inventoryItemCounts = new int[10];
    public Sprite emptySlotSprite;

    [Header("현재 전투 데이터")]
    public string currentEnemyName = "기본 몬스터";
    public int currentEnemyHP = 5;
    public Sprite currentEnemySprite;

    // ▼▼▼ [추가] 이번 몬스터가 줄 경험치 기억하는 곳 ▼▼▼
    public int currentRewardExp = 0;
    // ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

    public int currentRewardGold = 0;
    public string previousSceneName;

    public Vector3 playerPositionBeforeBattle;
    public Sprite currentEnemyBattleSprite;

    [Header("게임 진행 플래그")]
    public bool isDungeonDoorUnlocked = false;

    public AudioSource musicPlayer;
    public AudioClip villageMusic;
    public AudioClip dungeonMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ClearInventory()
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            inventoryItems[i] = emptySlotSprite;
        }
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log("경험치 획득! +" + amount);

        while (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            level++;
            maxExp += 10;

            playerMaxHP += 2;
            playerCurrentHP = playerMaxHP;

            Debug.Log("레벨 업! 현재 레벨: " + level);
        }
    }
}