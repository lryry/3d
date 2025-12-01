using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("체력 설정")]
    public int maxHP = 10;
    public int currentHP;

    [Header("UI 연결")]
    public Slider hpSlider;

    void Start()
    {
        if (GameManager.instance != null)
        {
            currentHP = GameManager.instance.playerCurrentHP;
        }
        else
        {
            currentHP = maxHP;
        }

        UpdateSlider();
    }

    void UpdateSlider()
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0)
        {
            currentHP = 0;
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.playerCurrentHP = currentHP;
        }

        UpdateSlider();
        Debug.Log("HP가 " + currentHP + " 남았습니다.");
    }

    public void HealToFull()
    {
        currentHP = maxHP;

        if (GameManager.instance != null)
        {
            GameManager.instance.playerCurrentHP = currentHP;
        }

        UpdateSlider();
        Debug.Log("풀피로 회복되었습니다!");
    }
}