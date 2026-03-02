using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public int damage = 10;
    public bool destroyOnHit = true; // 플레이어와 충돌 시 투사체가 사라질지 여부

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 플레이어인지 확인
        if (collision.CompareTag("Player"))
        {
            // 2. 플레이어의 TakeDamage 호출
            if (collision.TryGetComponent<PlayerController>(out var player))
            {
                player.TakeDamage(damage);
            }

            // 3. 충돌 시 투사체 삭제 (레이저처럼 관통해야 하면 false로 설정)
            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }
}