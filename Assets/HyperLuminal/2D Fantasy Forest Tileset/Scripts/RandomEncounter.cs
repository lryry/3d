using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomEncounterManager : MonoBehaviour
{
    [System.Serializable]
    public class MonsterData
    {
        public string name;
        public Sprite sprite;
        public int hp;
        public int goldReward;
        public int expReward;
        [Range(1, 100)] public int spawnChance; // 몬스터끼리의 등장 비율
    }

    [Header("몬스터 목록")]
    public MonsterData[] monsters;

    [Header("전투 씬 이름")]
    public string battleSceneName = "next";

    // ▼▼▼ [여기가 핵심!] 전투가 걸릴 확률 (0 ~ 1000) ▼▼▼
    [Header("전투 발생 확률 (높을수록 자주 나옴)")]
    [Range(0, 1000)]
    public int encounterRate = 10; // 기본값 10 (1%)

    void Update()
    {
        // 1. 걷고 있을 때만 (키보드 누름)
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            // 2. 주사위 굴리기 (0 ~ 1000 사이 숫자 뽑기)
            // 뽑은 숫자가 encounterRate보다 작으면 당첨!
            if (Random.Range(0, 1000) < encounterRate)
            {
                StartBattle();
            }
        }
    }

    public void StartBattle()
    {
        // (기존과 동일한 몬스터 뽑기 로직)
        MonsterData selectedMonster = null;
        int totalChance = 0;
        foreach (var m in monsters) totalChance += m.spawnChance;

        int randomPoint = Random.Range(0, totalChance);

        foreach (var m in monsters)
        {
            if (randomPoint < m.spawnChance)
            {
                selectedMonster = m;
                break;
            }
            randomPoint -= m.spawnChance;
        }
        if (selectedMonster == null) selectedMonster = monsters[0];

        // 데이터 전달
        if (GameManager.instance != null)
        {
            GameManager.instance.currentEnemyName = selectedMonster.name;
            GameManager.instance.currentEnemySprite = selectedMonster.sprite;
            GameManager.instance.currentEnemyHP = selectedMonster.hp;
            GameManager.instance.currentRewardGold = selectedMonster.goldReward;
            GameManager.instance.currentRewardExp = selectedMonster.expReward;

            GameManager.instance.previousSceneName = SceneManager.GetActiveScene().name;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                GameManager.instance.playerPositionBeforeBattle = player.transform.position;
            }
        }

        SceneManager.LoadScene(battleSceneName);
    }
}