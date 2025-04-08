using UnityEngine;

public class MonsterController : MonoBehaviour, IDamageable
{
    public int maxHP = 30;
    private int currentHP;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;
        Debug.Log("몬스터 피격! 남은 HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die"); // "Die" 트리거 애니메이션 실행
        }

        // 일정 시간 후 오브젝트 제거
        Destroy(gameObject, 1.5f); // 애니메이션 길이에 따라 시간 조절
    }
}
