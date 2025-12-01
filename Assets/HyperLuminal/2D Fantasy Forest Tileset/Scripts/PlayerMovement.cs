using UnityEngine;
using TMPro; // (이제 TMPro는 이 스크립트에서 필요 없습니다)

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float movementSpeed = 5.0f;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Vector3 CheckPointPosition;
    private bool isDead = false;

    private GameObject currentNpc = null;

    [Header("UI")]
    public GameObject shopWindow; // ◀ 상점 창 열기 기능은 남겨둡니다.

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        CheckPointPosition = transform.position;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (shopWindow != null)
        {
            shopWindow.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            if (animator != null) animator.speed = 0f;
            return;
        }

        // (이동 코드)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 move = new Vector2(horizontal, vertical).normalized * movementSpeed;
        rb.linearVelocity = move;
        if (horizontal > 0) sr.flipX = false;
        else if (horizontal < 0) sr.flipX = true;
        if (animator != null)
        {
            animator.SetFloat("MoveX", horizontal);
            animator.SetFloat("MoveY", vertical);
            animator.SetFloat("Speed", move.sqrMagnitude);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("DangerousTile"))
        {
            GameObject.Find("FadePanel").GetComponent<FadeScript>().RespawnFade();
            isDead = true;
        }
        else if (collider.CompareTag("LevelChanger"))
        {
            GameObject.Find("FadePanel").GetComponent<FadeScript>().FadeOut();
            isDead = true;
        }
        else if (collider.CompareTag("NPC"))
        {
            currentNpc = collider.gameObject;
            if (shopWindow != null)
            {
                shopWindow.SetActive(true);
                isDead = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("NPC") && collider.gameObject == currentNpc)
        {
            currentNpc = null;
            CloseShopWindow();
        }
    }

    public void RespawnPlayerAtCheckpoint()
    {
        transform.position = CheckPointPosition;
        isDead = false;
    }

    public void CloseShopWindow()
    {
        if (shopWindow != null && shopWindow.activeSelf == true)
        {
            shopWindow.SetActive(false);
            isDead = false;
        }
    }
}