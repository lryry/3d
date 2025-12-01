using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("플레이어 데이터")]
    public int playerMaxHP = 10;
    public int playerCurrentHP = 10;

    [Header("인벤토리 데이터")]
    public Sprite[] inventoryItems = new Sprite[10];
    public Sprite emptySlotSprite;

    [Header("현재 전투 데이터")]
    public string currentEnemyName = "기본 몬스터";
    public Sprite currentEnemyBattleSprite;
    public int currentEnemyHP = 5;

    [Header("게임 진행 플래그")]
    public bool isDungeonDoorUnlocked = false;

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
}