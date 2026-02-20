using UnityEngine;
using UnityEngine.UI;

public class BossHPManager : MonoBehaviour, IDamageable
{
    public Slider slider;
    private int maxHP = 100;
    private int currentHP;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHP = maxHP;
        slider.maxValue = maxHP;
        slider.value = currentHP;
    }

    public void TakeDamage(int Damage)
    {
        if (currentHP < 0) return;

        currentHP -= Damage;
        slider.value = currentHP;

        Debug.Log("보스 피격! 남은 HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        animator.SetTrigger("Die");

        // Destroy();
    }
}
