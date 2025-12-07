using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyEncounter : MonoBehaviour
{
    [System.Serializable]
    public class MonsterInfo
    {
        public string monsterName; // 몬스터 이름
        public Sprite battleSprite; // 전투 때 쓸 사진 (필수!)
        public int hp;              // 체력

        // ▼▼▼ [추가됨] 이제 돈이랑 경험치도 설정할 수 있어요! ▼▼▼
        public int goldReward;      // 줄 돈
        public int expReward;       // 줄 경험치
    }

    [Header("이 자리에서 나올 몬스터 목록 (여러 개 넣으면 랜덤)")]
    public MonsterInfo[] monsterPool;

    [Header("이동할 씬 이름")]
    public string battleSceneName = "next";

    // (참고) 충돌했을 때 전투 시작하는 기능 (Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartEncounter();
        }
    }

    // (참고) 버튼으로 연결했다면 이 함수를 버튼에 연결하세요!
    public void StartEncounter()
    {
        if (monsterPool.Length == 0) return;

        // 1. 목록에 있는 애들 중 랜덤으로 한 마리 뽑기
        int randomIndex = Random.Range(0, monsterPool.Length);
        MonsterInfo selectedMonster = monsterPool[randomIndex];

        // 2. GameManager에 "이번엔 얘랑 싸웁니다!" 하고 완벽하게 보고하기
        if (GameManager.instance != null)
        {
            // 기본 정보 전달
            GameManager.instance.currentEnemyName = selectedMonster.monsterName;

            // ★ [중요] 여기서 뽑힌 녀석(토끼면 토끼)의 사진을 넘겨줘야 사진이 바뀝니다!
            GameManager.instance.currentEnemySprite = selectedMonster.battleSprite;

            GameManager.instance.currentEnemyHP = selectedMonster.hp;

            // ★ [중요] 돈이랑 경험치 정보 전달 (이게 없어서 안 줬던 것!)
            GameManager.instance.currentRewardGold = selectedMonster.goldReward;
            GameManager.instance.currentRewardExp = selectedMonster.expReward;

            // 끝나고 돌아올 정보 저장
            GameManager.instance.previousSceneName = SceneManager.GetActiveScene().name;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                GameManager.instance.playerPositionBeforeBattle = player.transform.position;
            }
        }

        // 3. 전투 씬으로 이동!
        SceneManager.LoadScene(battleSceneName);
    }
}