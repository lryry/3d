using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trigger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Sprite OnSprite;
    public Sprite OffSprite;
    public List<GameObject> TriggeredObjects = new List<GameObject>();

    public enum TOGGLE
    {
        ON = 0,
        OFF = 1,
    }
    public TOGGLE Toggle;
    private bool hasBeenTriggered = false;


    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (Toggle == TOGGLE.OFF)
        {
            spriteRenderer.sprite = OffSprite;
        }
        else if (Toggle == TOGGLE.ON)
        {
            spriteRenderer.sprite = OnSprite;
        }
    }

    public void ToggleObject()
    {
        if (Toggle == TOGGLE.OFF)
        {
            Toggle = TOGGLE.ON;
            spriteRenderer.sprite = OnSprite;
        }
        else if (Toggle == TOGGLE.ON)
        {
            Toggle = TOGGLE.OFF;
            spriteRenderer.sprite = OffSprite;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // '플레이어'가 닿았고, '아직' '한번도' 밟지 않았다면
        if (collider.CompareTag("Player") && !hasBeenTriggered)
        {
            // 1. '한번' 밟았다고 '저장' (중복 방지)
            hasBeenTriggered = true;

            // 2. '상자' '그림' 바꾸기
            ToggleObject();

            // 3. 'GameManager'에 "나 먹혔다! 문 열어!" '신호' 보내기 (이것도 '유지'!)
            if (GameManager.instance != null)
            {
                GameManager.instance.isDungeonDoorUnlocked = true;
            }

            foreach (GameObject obj in TriggeredObjects)
            {
                // '일반 문' (Door.cs)이 '연결'되어 있다면?
                if (obj.GetComponent<Door>())
                {
                    obj.GetComponent<Door>().ToggleObject();
                }

                // '보스 문' (EnemyEncounter.cs)이 '연결'되어 있다면?
                if (obj.GetComponent<EnemyEncounter>())
                {
                    // 'EnemyEncounter' 스크립트의 'ToggleObject()' 함수를 '호출'
                    obj.GetComponent<EnemyEncounter>().ToggleObject();
                }
            }
        }
    }
}