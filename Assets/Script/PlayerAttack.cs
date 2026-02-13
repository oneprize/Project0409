using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    public WeaponAttack weapon;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 무기가 공격 중이 아닐 때만 실행
            if (weapon != null && !weapon.IsAttacking)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        weapon.ExecuteAttack();
    }
}