using UnityEngine;
using UnityEngine.SceneManagement; // 최신 Unity용 SceneManager

public class FadeScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float AlphaValue;

    public enum FADETYPE
    {
        IN,
        OUT,
        NONE,
        RESPAWN
    }

    public FADETYPE FadeType;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        FadeType = FADETYPE.IN;
        AlphaValue = 1.0f;
    }

    void Update()
    {
        if (FadeType == FADETYPE.IN)
        {
            AlphaValue -= 0.25f * Time.deltaTime;
            if (AlphaValue < 0f)
            {
                AlphaValue = 0f;
                FadeType = FADETYPE.NONE;
            }
        }
        else if (FadeType == FADETYPE.OUT)
        {
            AlphaValue += Time.deltaTime;
            if (AlphaValue > 1f)
            {
                AlphaValue = 1f;
                FadeType = FADETYPE.NONE;
                ChangeLevel();
            }
        }
        else if (FadeType == FADETYPE.RESPAWN)
        {
            AlphaValue += 2.0f * Time.deltaTime;
            if (AlphaValue > 1f)
            {
                AlphaValue = 1f;
                FadeType = FADETYPE.IN;

                //플레이어 리스폰 호출
                GameObject.Find("Player").GetComponent<PlayerMovement>().RespawnPlayerAtCheckpoint();
            }
        }

        // 페이드 색상 적용
        spriteRenderer.color = new Color(1f, 1f, 1f, AlphaValue);
    }

    public void FadeOut()
    {
        FadeType = FADETYPE.OUT;
    }

    public void RespawnFade()
    {
        FadeType = FADETYPE.RESPAWN;
    }

    private void ChangeLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
            nextIndex = 0;

        SceneManager.LoadScene(nextIndex);
    }
}
