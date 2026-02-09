using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    public WeaponAttack weapon; // 인스펙터에서 WeaponAttack이 붙은 오브젝트를 할당하세요.

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