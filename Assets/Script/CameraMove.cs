using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 1, -10);
    public float smoothSpeed = 5f;

    private Vector2 minBounds;
    private Vector2 maxBounds;

    private float camHalfHeight;
    private float camHalfWidth;

    [Header("Tilemap 자동 참조")]
    public Tilemap tilemap;

    void Start()
    {
        // 카메라 화면 크기 계산
        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = cam.aspect * camHalfHeight;

        //  target이 비어있다면 Player 태그로 찾아서 자동 설정
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        // 타일맵 범위 가져오기
        if (tilemap != null)
        {
            Bounds tileBounds = tilemap.localBounds;

            minBounds = tileBounds.min;
            maxBounds = tileBounds.max;
        }
        else
        {
            Debug.LogWarning("Tilemap이 연결되지 않았습니다! Camera 제한이 작동하지 않을 수 있습니다.");
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 따라갈 위치
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 카메라가 벽 밖으로 나가지 않도록 제한
        float clampedX = Mathf.Clamp(smoothedPosition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);

        transform.position = new Vector3(clampedX, clampedY, offset.z);
    }
}
