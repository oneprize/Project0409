using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern3_FallingSwords : BossPattern
{
    public GameObject swordPrefab;    // 검 프리팹 (박스 콜라이더 포함)
    public int swordCount = 5;        // 떨어뜨릴 검의 개수
    public float spawnHeight = 8f;    // 플레이어 머리 위 소환 높이
    public float interval = 0.4f;     // 검 생성 간격
    public float fallSpeed = 20f;     // 낙하 속도

    public override IEnumerator Execute()
    {
        Debug.Log("패턴 3: 낙하하는 검");

        for (int i = 0; i < swordCount; i++)
        {
            // 플레이어의 현재 위치를 추적하여 머리 위 X좌표 설정
            Vector3 spawnPos = new Vector3(boss.skellBoss.position.x + i, boss.skellBoss.position.y, 0);

            // 검 생성 및 예고선 로직 (검 프리팹 내부나 여기서 처리)
            GameObject sword = Instantiate(swordPrefab, spawnPos, Quaternion.Euler(0,0,180)); 

            // 검이 바로 떨어지지 않도록 별도 코루틴 실행
            StartCoroutine(DropSword(sword));

            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(1.5f); // 패턴 완전 종료 대기
    }

    IEnumerator DropSword(GameObject sword)
    {
        // 1. 예고 단계 (깜빡이거나 빨간 선 표시)
        SpriteRenderer sr = sword.GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        float waitTimer = 0.7f;
        while (waitTimer > 0)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = originalColor;
            yield return new WaitForSeconds(0.1f);
            waitTimer -= 0.2f;
        }

        // 2. 낙하 단계
        Vector3 targetPos = boss.player.position;
        Vector3 fallDirection = (targetPos - sword.transform.position).normalized;
        float startTime = Time.time;
        float fallSpeed = 25f;

        while (sword != null)
        {
            sword.transform.position += fallDirection * fallSpeed * Time.deltaTime;

            float angle = Mathf.Atan2(fallDirection.y, fallDirection.x) * Mathf.Rad2Deg;
            sword.transform.rotation = Quaternion.Euler(0,0, angle);
            yield return null;

            // 바닥에 닿거나 일정 시간 지나면 삭제 (단순화)
            if (Time.time - startTime > 2f) break;
        }

        Destroy(sword);
    }
}