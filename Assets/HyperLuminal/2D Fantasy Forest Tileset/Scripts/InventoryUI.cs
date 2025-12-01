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

    [Header("상점 교환 아이템")]
    public Sprite appleIcon;
    public Sprite coinIcon;

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
        if (playerHealth == null)
        {
            Debug.LogError("InventoryUI: '씬'에서 'PlayerHealth' 스크립트를 찾을 수 없습니다!");
        }

        learnedEmptySprite = GameManager.instance.emptySlotSprite;
        if (learnedEmptySprite == null)
        {
            Debug.LogError("InventoryUI: 'GameManager'에 'Empty Slot Sprite'가 연결되지 않았습니다!");
            return;
        }

        for (int i = 0; i < slotButtons.Length; i++)
        {
            int slotIndex = i;

            slotButtons[i].onClick.AddListener(() => OnSlotClicked(slotIndex));

            if (GameManager.instance.inventoryItems[i] != null)
            {
                slotButtons[i].GetComponent<Image>().sprite = GameManager.instance.inventoryItems[i];
            }
            else
            {
                slotButtons[i].GetComponent<Image>().sprite = learnedEmptySprite;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isExpanded = !isExpanded;
            if (expansionPanel != null)
            {
                expansionPanel.SetActive(isExpanded);
            }
        }
    }

    void OnSlotClicked(int slotIndex)
    {
        if (playerHealth == null)
        {
            Debug.LogError("InventoryUI: 'PlayerHealth'가 '연결'되지 않아 아이템을 '사용'할 수 없습니다!");
            return;
        }

        Sprite itemInSlot = GameManager.instance.inventoryItems[slotIndex];

        if (itemInSlot != null && itemInSlot != learnedEmptySprite)
        {
            if (itemInSlot == appleIcon)
            {
                if (playerHealth.currentHP == playerHealth.maxHP)
                {
                    Debug.Log("이미 체력이 꽉 찼습니다!");
                    return;
                }

                playerHealth.HealToFull();
                Debug.Log("사과를 먹었다! 풀피로 회복!");

                RemoveItemAt(slotIndex);
            }
            else
            {
                Debug.Log(itemInSlot.name + " 아이템은 '사용'할 수 없습니다.");
            }
        }
        else
        {
            Debug.Log(slotIndex + "번 '빈 슬롯' 클릭됨.");
        }
    }


    public bool AddItem(Sprite newItemIcon)
    {
        if (newItemIcon == null || learnedEmptySprite == null) return false;

        for (int i = 0; i < slotButtons.Length; i++)
        {
            Image slotImage = slotButtons[i].GetComponent<Image>();

            if (slotImage != null && slotImage.sprite != null && slotImage.sprite.name.Contains(learnedEmptySprite.name))
            {
                slotImage.sprite = newItemIcon;

                GameManager.instance.inventoryItems[i] = newItemIcon;
                return true;
            }
        }
        Debug.LogWarning("인벤토리가 꽉 찼습니다!");
        return false;
    }

    public bool RemoveItem(Sprite itemToRemove)
    {
        if (itemToRemove == null || learnedEmptySprite == null) return false;
        string itemToFindName = itemToRemove.name;

        for (int i = 0; i < slotButtons.Length; i++)
        {
            Image slotImage = slotButtons[i].GetComponent<Image>();

            if (slotImage == null || slotImage.sprite == null) continue;
            string slotItemName = slotImage.sprite.name;

            if (slotItemName.Contains(itemToFindName))
            {
                slotImage.sprite = learnedEmptySprite;
                GameManager.instance.inventoryItems[i] = learnedEmptySprite;
                return true;
            }
        }
        return false;
    }

    private void RemoveItemAt(int slotIndex)
    {
        if (learnedEmptySprite == null) return;

        slotButtons[slotIndex].GetComponent<Image>().sprite = learnedEmptySprite;
        GameManager.instance.inventoryItems[slotIndex] = learnedEmptySprite;
    }

    public void BuyApple()
    {
        if (coinIcon == null || appleIcon == null) return;
        if (RemoveItem(coinIcon))
        {
            if (AddItem(appleIcon))
            {
                Debug.Log("사과 구매");
            }
            else
            {
                Debug.LogError("사과를 구매했으나 인벤토리가 꽉 찼습니다! (코인만 잃음)");
            }
        }
        else
        {
            Debug.Log("구매 실패: 인벤토리에 '코인' 아이템이 없습니다.");
        }
    }

    public void SellApple()
    {
        if (coinIcon == null || appleIcon == null) return;
        if (RemoveItem(appleIcon))
        {
            if (AddItem(coinIcon))
            {
                Debug.Log("사과 판매");
            }
            else
            {
                Debug.LogError("사과를 판매했으나 인벤토리가 꽉 찼습니다! (사과만 잃음)");
            }
        }
        else
        {
            Debug.Log("판매 실패: 인벤토리에 '사과' 아이템이 없습니다.");
        }
    }
}