using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public enum TransitionType { PortalID, DirectPosition }

    [Header("이동 방식 설정")]
    public TransitionType type;
    public string targetSceneName;

    [Header("ID 방식 (일반 던전)")]
    public int targetPortalID; // 도착할 포탈 번호
    public int myPortalID;     // 이 포탈의 고유 번호

    [Header("자식 오브젝트로 지정할 스폰 위치")]
    [SerializeField] private Transform spawnPoint;

    [Header("직접 좌표 (보스룸 등)")]
    public Vector3 customTargetPosition;

    [Header("무한루프 방지")]
    private static float lastTransitionTime = -1f;
    private const float transitionDelay = 1.0f; // 1초 쿨타임

    private void Awake()
    {
        Vector3 spawnPos = (spawnPoint != null) ? spawnPoint.position : transform.position;
        PortalManager.RegisterPortal(myPortalID, spawnPos);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 쿨타임 체크 (방금 이동해왔는데 다시 나가는 것 방지)
            if (Time.time < lastTransitionTime + transitionDelay) return;

            lastTransitionTime = Time.time;

            // 목적지 타입에 따라 데이터 전송
            if (type == TransitionType.PortalID)
                PortalManager.SetTargetID(targetPortalID);
            else
                PortalManager.SetTargetPosition(customTargetPosition);

            DungeonManager.MarkRoomAsCleared();
            SceneManager.LoadScene(targetSceneName);
        }
    }
}