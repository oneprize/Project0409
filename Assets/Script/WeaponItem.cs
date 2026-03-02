using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    private bool isEquipped = false;

    // Trigger Collider에 플레이어가 닿았을 때 호출됨
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 이미 장착되었거나 플레이어가 아니면 무시
        if (isEquipped || !other.CompareTag("Player")) return;

        if (other.CompareTag("Player"))
        {
            // 부딪힌 물체의 Transform 정보를 넘겨주며 장착 함수 실행
            Equip(other.transform);
        }
    }

    void Equip(Transform playerTransform)
    {
        isEquipped = true;

        // 1. 플레이어 자식 중에 "Weapon"라는 이름의 위치를 찾음
        Transform hand = playerTransform.Find("WeaponHolder/Weapon");

        if (hand != null)
        {
            // 2. 물리 엔진 비활성화 (더 이상 땅에 떨어지거나 구르지 않게 함)
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null) Destroy(rb); // 물리 시뮬레이션 전체를 꺼버림

            // 3. 부모를 HandPos로 설정하고 위치/회전값 초기화
            transform.SetParent(hand);
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            Transform Weapon = transform.Find("Collider");
            Collider2D col = Weapon.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            WeaponAttack attackScript = GetComponent<WeaponAttack>();
            if (attackScript != null)
            {
                attackScript.enabled = true;
                // 이때 공격용 콜라이더를 꺼서 대기 상태로 만듭니다.
                GetComponent<Collider2D>().enabled = false;
            }

            Debug.Log("무기 장착 완료!");
        }
    }
}