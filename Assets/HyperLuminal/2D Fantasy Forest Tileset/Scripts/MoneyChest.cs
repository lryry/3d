using UnityEngine;

public class MoneyChest : MonoBehaviour
{
    [Header("»óÀÚ ¼³Á¤")]
    public Sprite itemToGive;
    public Sprite openSprite;

    private SpriteRenderer spriteRenderer;
    private bool isOpened = false;
    private InventoryUI inventory;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventory = FindObjectOfType<InventoryUI>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !isOpened)
        {
            isOpened = true;

            if (openSprite != null)
            {
                spriteRenderer.sprite = openSprite;
            }

            if (inventory != null && itemToGive != null)
            {
                inventory.AddItem(itemToGive);
                Debug.Log("ÄÚÀÎ È¹µæ!");
            }

            if (GameManager.instance != null)
            {
                GameManager.instance.isDungeonDoorUnlocked = true;
            }
        }
    }
}