using UnityEngine;
using Unity.Cinemachine; // Unity 6 기준 네임스페이스

[RequireComponent(typeof(CinemachineCamera))]
public class TargetCamera : MonoBehaviour
{
    private CinemachineCamera _cmCamera;

    void Awake()
    {
        _cmCamera = GetComponent<CinemachineCamera>();
    }

    void Start()
    {
        FindAndAssignPlayer();
    }

    void Update()
    {
        // 타겟이 없거나, 참조가 깨졌을 때만 검색 (성능 고려)
        if (_cmCamera.Follow == null)
        {
            FindAndAssignPlayer();
        }
    }

    private void FindAndAssignPlayer()
    {
        // "Player" 태그를 가진 오브젝트를 찾습니다.
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            _cmCamera.Follow = player.transform;

            Debug.Log($"[Cinemachine] {player.name}을(를) 새로운 타겟으로 설정했습니다.");
        }
    }
}