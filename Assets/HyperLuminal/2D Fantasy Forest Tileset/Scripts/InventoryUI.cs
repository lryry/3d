using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("UI 패널")]
    [SerializeField]
    private GameObject expansionPanel;

    [Header("인벤토리 슬롯 (UI)")]
    public Button[] slotButtons = new Button[10];

    [Header("아이템 설정 (드래그 필수!)")]
    public Sprite appleIcon; // 줄 사과
    public Sprite coinIcon;  // 받을 코인

    private bool isExpanded = false;
    private Sprite learnedEmptySprite;
    private PlayerHealth playerHealth;

    void Start()
    {
        if (expansionPanel != null)
        {
            expansionPanel.SetActive(false);
            isExpanded = false;
        }

        playerHealth = FindObjectOfType<PlayerHealth>();

        if (GameManager.instance != null)
        {
            learnedEmptySprite = GameManager.instance.emptySlotSprite;
        }

        RefreshInventoryUI(); // 시작할 때 인벤토리 그리기
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isExpanded = !isExpanded;
            if (expansionPanel != null) expansionPanel.SetActive(isExpanded);
        }

        // (참고) 매 프레임마다 그리면 성능에 안 좋지만, 확실한 업데이트를 위해 임시로 둡니다.
        RefreshInventoryUI();
    }

    // 화면 그리기 함수 (따로 뺌)
    void RefreshInventoryUI()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int slotIndex = i;
            slotButtons[i].onClick.RemoveAllListeners(); // 중복 클릭 방지
            slotButtons[i].onClick.AddListener(() => OnSlotClicked(slotIndex));

            if (GameManager.instance != null)
            {
                if (GameManager.instance.inventoryItems[i] != null)
                    slotButtons[i].GetComponent<Image>().sprite = GameManager.instance.inventoryItems[i];
                else
                    slotButtons[i].GetComponent<Image>().sprite = learnedEmptySprite;
            }
        }
    }

    void OnSlotClicked(int slotIndex)
    {
        if (GameManager.instance == null) return;
        Sprite itemInSlot = GameManager.instance.inventoryItems[slotIndex];

        // 사과 먹기 기능
        if (itemInSlot != null && appleIcon != null && itemInSlot.name == appleIcon.name)
        {
            if (playerHealth != null)
            {
                if (playerHealth.currentHP >= playerHealth.maxHP) return;
                playerHealth.HealToFull();
                RemoveItemAt(slotIndex);
            }
        }
    }

    // 아이템 추가
    public bool AddItem(Sprite newItemIcon)
    {
        if (newItemIcon == null || learnedEmptySprite == null) return false;

        for (int i = 0; i < slotButtons.Length; i++)
        {
            Image slotImage = slotButtons[i].GetComponent<Image>();
            // 이름으로 빈칸 확인
            if (slotImage.sprite.name == learnedEmptySprite.name)
            {
                slotImage.sprite = newItemIcon;
                GameManager.instance.inventoryItems[i] = newItemIcon;
                return true;
            }
        }
        Debug.Log("가방이 꽉 찼습니다!");
        return false;
    }

    // 아이템 삭제 (특정 아이템 1개 지우기)
    public bool RemoveItem(Sprite itemToRemove)
    {
        if (itemToRemove == null) return false;

        for (int i = 0; i < slotButtons.Length; i++)
        {
            Image slotImage = slotButtons[i].GetComponent<Image>();
            // 이름이 같으면 삭제 (같은 그림이면 OK)
            if (slotImage.sprite.name == itemToRemove.name)
            {
                RemoveItemAt(i);
                return true; // 하나 지웠으면 성공!
            }
        }
        return false; // 못 찾았음
    }

    private void RemoveItemAt(int slotIndex)
    {
        if (learnedEmptySprite == null) return;
        slotButtons[slotIndex].GetComponent<Image>().sprite = learnedEmptySprite;
        GameManager.instance.inventoryItems[slotIndex] = learnedEmptySprite;
    }

    // ▼▼▼ [사과 구매 버튼 기능] ▼▼▼
    public void BuyApple()
    {
        if (coinIcon == null || appleIcon == null)
        {
            Debug.LogError("InventoryUI에 코인/사과 그림이 연결되지 않았습니다!");
            return;
        }

        // 1. 가방에 '코인'이 있는지 확인하고 1개 뺏기
        if (RemoveItem(coinIcon))
        {
            // 2. 코인 뺏기 성공! 이제 사과 주기
            if (AddItem(appleIcon))
            {
                Debug.Log("구매 성공!");
            }
            else
            {
                // (고급) 가방이 꽉 차서 사과 못 받으면? 코인 다시 돌려주기
                AddItem(coinIcon);
                Debug.Log("가방이 꽉 차서 구매 실패!");
            }
        }
        else
        {
            Debug.Log("구매 실패: 가방에 '코인'이 없습니다.");
        }
    }

    // ▼▼▼ [사과 판매 버튼 기능] ▼▼▼
    public void SellApple()
    {
        if (coinIcon == null || appleIcon == null) return;

        // 1. 가방에 '사과'가 있는지 확인하고 1개 뺏기
        if (RemoveItem(appleIcon))
        {
            // 2. 사과 뺏기 성공! 코인 주기
            AddItem(coinIcon);
            Debug.Log("판매 성공!");
        }
        else
        {
            Debug.Log("판매 실패: 가방에 '사과'가 없습니다.");
        }
    }
}