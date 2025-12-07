using UnityEngine;
using UnityEngine.UI;
using TMPro; // 레벨 글자(Lv.1) 띄우려면 필요

public class ExpBar : MonoBehaviour
{
    public Slider slider;             // 게이지 바 (인스펙터 연결)
    public TextMeshProUGUI levelText; // "Lv.1" 글자 (인스펙터 연결, 없으면 비워도 됨)

    void Update()
    {
        if (GameManager.instance != null)
        {
            // 1. 슬라이더 채우기 (현재 경험치 / 최대 경험치)
            if (slider != null)
            {
                // 소수점(0.0 ~ 1.0)으로 바꿔서 슬라이더에 넣기
                if (GameManager.instance.maxExp > 0)
                    slider.value = (float)GameManager.instance.currentExp / GameManager.instance.maxExp;
            }

            // 2. 레벨 글자 업데이트
            if (levelText != null)
            {
                levelText.text = "Lv." + GameManager.instance.level;
            }
        }
    }
}