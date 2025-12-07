using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trigger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Sprite OnSprite;
    public Sprite OffSprite;
    public List<GameObject> TriggeredObjects = new List<GameObject>();

    public enum TOGGLE { ON = 0, OFF = 1 }
    public TOGGLE Toggle;
    private bool hasBeenTriggered = false;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (Toggle == TOGGLE.OFF) spriteRenderer.sprite = OffSprite;
        else if (Toggle == TOGGLE.ON) spriteRenderer.sprite = OnSprite;
    }

    public void ToggleObject()
    {
        if (Toggle == TOGGLE.OFF) { Toggle = TOGGLE.ON; spriteRenderer.sprite = OnSprite; }
        else if (Toggle == TOGGLE.ON) { Toggle = TOGGLE.OFF; spriteRenderer.sprite = OffSprite; }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            ToggleObject(); // 상자 열기

            // 문 열렸다고 매니저한테 알림
            if (GameManager.instance != null)
            {
                GameManager.instance.isDungeonDoorUnlocked = true;
            }

            foreach (GameObject obj in TriggeredObjects)
            {
                // 1. 문(Door)이 연결되어 있으면 -> 문 열기 (ToggleObject)
                if (obj.GetComponent<Door>())
                {
                    obj.GetComponent<Door>().ToggleObject();
                }

                // ▼▼▼ [이 부분을 다시 넣어야 합니다!] ▼▼▼
                // 2. 적(EnemyEncounter)이 연결되어 있으면 -> 전투 시작하기 (StartEncounter)
                if (obj.GetComponent<EnemyEncounter>())
                {
                    // 아까 에러났던 ToggleObject() 대신, 
                    // 올바른 함수인 StartEncounter()를 부르게 고쳤습니다!
                    obj.GetComponent<EnemyEncounter>().StartEncounter();
                }
                // ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
            }
        }
    }
}