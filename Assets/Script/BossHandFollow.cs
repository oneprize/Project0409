using UnityEngine;

public class BossHandFollow : MonoBehaviour
{
    public Transform target; // 따라갈 대상 (플레이어)
    public float followSpeed = 3f;
    public bool useSmoothFollow = true;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }
    }

    void Update()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            return; // 아직 못 찾았으면 아래 실행 안 함
        }

        // y축 따라가기
        Vector3 newPos = transform.position;
        newPos.y = Mathf.Lerp(transform.position.y, target.position.y, followSpeed * Time.deltaTime);
        transform.position = newPos;
    }
}
