using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow2D : MonoBehaviour
{
    [Header("따라갈 플레이어")]
    public Transform player;  // 따라갈 대상

    [Header("카메라 설정")]
    public float smoothSpeed = 0.15f; // 부드럽게 따라오는 속도
    public Vector3 offset = new Vector3(0, 0, -10); // Z축은 항상 -10 고정

    [Header("맵 경계 설정")]
    public Vector2 minBounds = new Vector2(-10, -10); // 맵 왼쪽/아래 끝
    public Vector2 maxBounds = new Vector2(100, 100); // 맵 오른쪽/위 끝

    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private float camHalfHeight;
    private float camHalfWidth;

    void Start()
    {
        cam = GetComponent<Camera>();
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;

        // 초기 위치 설정
        if (player != null)
        {
            Vector3 startPos = player.position + offset;
            startPos.x = Mathf.Clamp(startPos.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
            startPos.y = Mathf.Clamp(startPos.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);
            transform.position = startPos;
        }
        else
        {
            Debug.LogWarning(" CameraFollow2D: Player가 할당되지 않았습니다!");
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;

        Vector3 targetPos = player.position + offset;

        // 맵 경계 제한
        float clampX = Mathf.Clamp(targetPos.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
        float clampY = Mathf.Clamp(targetPos.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);

        Vector3 desiredPos = new Vector3(clampX, clampY, offset.z);

        // 부드럽게 이동
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothSpeed);
    }

#if UNITY_EDITOR
    // 에디터에서 경계 보이기
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y), new Vector3(maxBounds.x, minBounds.y));
        Gizmos.DrawLine(new Vector3(minBounds.x, maxBounds.y), new Vector3(maxBounds.x, maxBounds.y));
        Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y), new Vector3(minBounds.x, maxBounds.y));
        Gizmos.DrawLine(new Vector3(maxBounds.x, minBounds.y), new Vector3(maxBounds.x, maxBounds.y));
    }
#endif
}
