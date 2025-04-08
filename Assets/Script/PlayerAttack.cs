using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetBool("isAttacking", true);
        }

        // 애니메이션 끝나면 다시 false로
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    //  이 함수는 애니메이션 이벤트에서 호출됨
    public void ApplyDamage()
    {
        Debug.Log("ApplyDamage 애니메이션 이벤트 호출됨!");
        // 몬스터 데미지 처리 등은 이 안에서 구현
    }
}
