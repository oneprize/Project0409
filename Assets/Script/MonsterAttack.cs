using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public int damage = 10;
    public float attackStart = 0.5f;
    public float attackDuration = 0.3f;
    public float attackCooldown = 2f;
    public LayerMask enemyLayer;

    private Collider2D weaponCollider;
    private List<Collider2D> hitEnemies = new List<Collider2D>();
    public bool IsAttacking { get; private set; }

    private float lastAttackTime = -Mathf.Infinity;
    private Animator animator;
    private Transform target;
    private MonsterAI MonsterAI;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        MonsterAI = GetComponentInParent<MonsterAI>();
        weaponCollider = GetComponent<Collider2D>();

        weaponCollider.enabled = false;
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= 2f && Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            if (animator != null) animator.SetTrigger("Attack");

            if (MonsterAI != null) MonsterAI.StartAttack(1f);
            
            ExecuteAttack();

            Debug.Log("몬스터가 공격 애니메이션 실행 및 AI 정지!");
        }
    }

    public void ExecuteAttack()
    {
        if (IsAttacking) return; // 이미 공격 중이면 무시
        StartCoroutine(AttackProcess());
    }

    private IEnumerator AttackProcess()
    {
        IsAttacking = true;
        hitEnemies.Clear();

        yield return new WaitForSeconds(attackStart);

        weaponCollider.enabled = true;

        yield return new WaitForSeconds(attackDuration);

        weaponCollider.enabled = false;
        IsAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{collision.name}과(와) 충돌 발생!");

        if (IsAttacking && !hitEnemies.Contains(collision))
        {
            if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
            {
                IDamageable target = collision.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                    hitEnemies.Add(collision);
                }
            }
        }
    }

    // 애니메이션 이벤트에서 호출될 데미지 적용 함수
    //public void DealDamage()
    //{
    //    if (target == null) return;

    //    float distance = Vector2.Distance(transform.position, target.position);
    //    if (distance <= 1.5f)
    //    {
    //        IDamageable damageable = target.GetComponent<IDamageable>();
    //        if (damageable != null)
    //        {
    //            damageable.TakeDamage(damage);
    //        }
    //    }
    //}


}
