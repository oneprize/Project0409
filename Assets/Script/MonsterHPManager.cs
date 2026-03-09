using UnityEngine;
using System.Collections;

public class MonsterHPManager : MonoBehaviour, IDamageable
{
    public float flashDuration = 0.1f; // 깜박이는 시간 (예: 0.1초 빨강, 0.1초 원본)
    public int flashCount = 2;          // 깜박이는 횟수
    public Color hitColor = Color.red;   // 피격 시 바꿀 색상

    public MonsterAI monsterAI;
    public FlyMonsterMovement flyMonster;
    public MonsterAttack monsterAttack;
    public int maxHP = 30;
    private int currentHP;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isDead = false;
    private GameObject monster;
    private Rigidbody2D rd;

    void Start()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        monsterAI = GetComponent<MonsterAI>();
        flyMonster = GetComponent<FlyMonsterMovement>();
        monsterAttack = GetComponentInChildren<MonsterAttack>();
        originalColor = spriteRenderer.color;
        GameObject monster = GameObject.FindWithTag("Enemy");
        rd = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;
        Debug.Log("몬스터 피격! 남은 HP: " + currentHP);

        StartCoroutine(HitEffect());

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitEffect()
    {
        if (spriteRenderer == null) yield break;

        for (int i = 0; i < flashCount; i++)
        {
            // 1. 빨간색으로 변경
            spriteRenderer.color = hitColor;
            // 2. flashDuration 만큼 대기
            yield return new WaitForSeconds(flashDuration);

            // 3. 원래 색상으로 복구
            spriteRenderer.color = originalColor;
            // 4. 다시 flashDuration 만큼 대기 (다음 깜박임을 위해)
            yield return new WaitForSeconds(flashDuration);
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        if (monsterAI != null) monsterAI.StopAllCoroutines();
        if (monsterAI != null) monsterAI.enabled = false;
        if (monsterAttack != null) monsterAttack.StopAllCoroutines();
        if (monsterAttack != null)monsterAttack.enabled = false;
        if(flyMonster != null)flyMonster.isdead = true;
        spriteRenderer.color = originalColor;
        animator.SetTrigger("Die");
        rd.linearVelocity = Vector2.zero;

        Destroy(gameObject, 1f);
    }
}
