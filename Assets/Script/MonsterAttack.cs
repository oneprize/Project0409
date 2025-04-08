using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public int damage = 10;
    public float attackCooldown = 2f;

    private float lastAttackTime = -Mathf.Infinity;
    private Animator animator;
    private Transform target;

    private bool canAttack = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= 2f && Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            // 애니메이션 타이밍 동안 이동 중지
            MonsterAI ai = GetComponentInParent<MonsterAI>();
            if (ai != null)
            {
                ai.StartAttack(attackCooldown);
            }

            Debug.Log("몬스터가 공격 애니메이션 실행!");
        }
    }

    // 애니메이션 이벤트에서 호출될 데미지 적용 함수
    public void DealDamage()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance <= 1.5f)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                Debug.Log("애니메이션 이벤트로 데미지 적용");
            }
        }
    }

    
}
