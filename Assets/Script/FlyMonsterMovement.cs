using UnityEngine;
using System.Collections;

public class FlyMonsterMovement : MonoBehaviour
{
    private Transform player;      // 플레이어의 위치
    public float moveSpeed = 3f;  // 이동 속도
    public float detectRange = 7f; // 인식 범위
    public float attackRange = 2.5f; // 공격 범위

    public enum EnemyState { Idle, Patrol, Chasing, ReadyToDash, Dashing }
    public EnemyState currentState = EnemyState.Patrol;

    [Header("Dash Settings")]
    public float dashSpeed = 12f;      // 대시 속도
    public float dashDuration = 0.4f;   // 대시 지속 시간
    public float dashReadyTime = 0.7f;  // 대시 전 멈춤 시간 (선딜레이)
    public float dashCooldown = 2f;     // 대시 후 재사용 대기시간
    private bool canDash = true;
    private Vector2 dashDirection;

    [Header("Patrol Settings")]
    public float patrolRange = 3f;     // 정찰할 범위
    private Vector2 startPosition;      // 처음 생성된 위치 (기준점)
    private Vector2 patrolTarget;       // 현재 이동 중인 목표 지점

    [Header("Sin Wave Settings")]
    public float amplitude = 0.5f;
    public float frequency = 3f;

    private Rigidbody2D rb;
    private float sinTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        startPosition = transform.position; // 시작 위치 저장
        SetNewPatrolTarget();               // 첫 정찰 목표 설정
    }

    void FixedUpdate()
    {
        if (player == null) return;
        if (currentState == EnemyState.Dashing || currentState == EnemyState.ReadyToDash) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
       
        // 상태 전환 로직
        if (distanceToPlayer <= attackRange && canDash)
        {
            StartCoroutine(DashAttackRoutine());
        }
        else if (distanceToPlayer <= detectRange)
        {
            currentState = EnemyState.Chasing;
            MoveTowards(player.position);
        }
        else
        {
            currentState = EnemyState.Patrol;
            PatrolBehavior();
        }
    }

    IEnumerator DashAttackRoutine()
    {
        currentState = EnemyState.ReadyToDash;
        canDash = false;
        rb.linearVelocity = Vector2.zero; // 잠시 멈춤

        // 1. 공격 전 예보
        float timer = 0;
        Vector3 originalPos = transform.position;
        while (timer < dashReadyTime)
        {
            // 부들부들 떠는 연출
            transform.position = originalPos + (Vector3)Random.insideUnitCircle * 0.1f;
            timer += Time.deltaTime;
            yield return null;
        }
        if (currentState == EnemyState.ReadyToDash)
        {
            Debug.Log("상태 ReadyToDash");
        }

        // 2. 대시 방향 결정 (당시 플레이어 위치)
        currentState = EnemyState.Dashing;
        dashDirection = (player.position - transform.position).normalized;
        if (currentState==EnemyState.Dashing)
        {
            Debug.Log("상태 Dash");
        }

        // 3. 대시 실행
        rb.linearVelocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);

        // 4. 대시 종료 후 후딜레이
        rb.linearVelocity = Vector2.zero;
        currentState = EnemyState.Idle; // 잠시 멍 때리기
        yield return new WaitForSeconds(0.5f);

        // 5. 쿨타임 대기
        currentState = EnemyState.Patrol;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void PatrolBehavior()
    {
        // 정찰 목표 지점에 거의 도달했는지 확인
        if (Vector2.Distance(transform.position, patrolTarget) < 0.2f)
        {
            SetNewPatrolTarget(); // 새로운 목표 지점 설정
        }

        MoveTowards(patrolTarget);
    }

    void ChasingBehavior()
    {
        MoveTowards(player.position);
    }

    void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        Vector2 moveVelocity = direction * moveSpeed;

        // Mathf.Sin을 이용한 넘실거리는 효과
        sinTimer += Time.fixedDeltaTime * frequency;
        float sinOffSet = Mathf.Sin(sinTimer) * amplitude;
        Vector2 upVector = new Vector2(-direction.y, direction.x);

        rb.linearVelocity = moveVelocity + (upVector * sinOffSet);

        // 방향 전환
        if (direction.x > 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (direction.x < 0) transform.localScale = new Vector3(1, 1, 1);
    }

    void SetNewPatrolTarget()
    {
        // 기준점(startPosition) 중심으로 랜덤한 좌표 생성
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomY = Random.Range(-patrolRange, patrolRange);
        patrolTarget = startPosition + new Vector2(randomX, randomY);
    }

    private void OnDrawGizmosSelected()
    {
        // 인식 범위 (노란색)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        // 정찰 기준 범위 (초록색) - 에디터에서 시작 위치 기준을 확인하기 위함
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startPosition, patrolRange);
            Gizmos.DrawLine(transform.position, patrolTarget); // 현재 목표선
        }
    }
}
