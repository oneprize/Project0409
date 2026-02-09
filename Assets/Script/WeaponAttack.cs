using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public int damage = 10;
    public float attackDuration = 0.2f; // 실제 공격 판정이 활성화되는 시간
    public LayerMask enemyLayer;

    private Collider2D weaponCollider;
    private List<Collider2D> hitEnemies = new List<Collider2D>();
    public bool IsAttacking { get; private set; } // 플레이어 스크립트에서 참조 가능

    void Awake()
    {
        weaponCollider = GetComponent<Collider2D>();
        weaponCollider.isTrigger = true;
        weaponCollider.enabled = false;
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
        weaponCollider.enabled = true;

        yield return new WaitForSeconds(attackDuration);

        weaponCollider.enabled = false;
        IsAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
}