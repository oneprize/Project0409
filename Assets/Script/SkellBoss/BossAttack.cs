using System.Collections;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    private int attackDurection = 1;
    public bool isAttacking = false;

    private Collider2D bossCollider;

    private void Awake()
    {
        bossCollider=GetComponent<Collider2D>();
        bossCollider.isTrigger = false;
        bossCollider.enabled = false;
    }

    public void ExcuteAttack()
    {
        if (isAttacking) { return; }
        StartCoroutine(AttackProsses());
        
    }
    private IEnumerator AttackProsses()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackDurection);
        bossCollider.enabled=true;
    }
}
