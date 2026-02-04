using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pattern3_FallingSwords : BossPattern
{
    public GameObject swordPrefab;    // 검 프리팹
    public int swordCount = 5;        // 떨어뜨릴 검의 개수    
    public float interval = 0.4f;     // 검 생성 간격
    public float launchInterval = 0.2f;   // 발사 사이의 간격
    public float fallSpeed = 20f;     // 낙하 속도
    public LayerMask Ground;

    public override IEnumerator Execute()
    {
        Debug.Log("패턴 3: 낙하하는 검");

        List<GameObject> spawnSword = new List<GameObject>();
        float space = 2f;
        float startOffset = (swordCount - 1) * space / 2f;

        for (int i = 0; i < swordCount; i++)
        {
            // 보스 머리 위 X좌표 설정
            float offsetX = (i * space) - startOffset;
            Vector3 spawnPos = new Vector3(boss.skellBoss.position.x + offsetX, boss.skellBoss.position.y, 0);

            // 검 생성
            GameObject sword = Instantiate(swordPrefab, spawnPos, Quaternion.Euler(0,0,180)); 
            spawnSword.Add(sword);

            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(1.5f); // 패턴 완전 종료 대기

        foreach (GameObject sword in spawnSword)
        {
            if (sword !=null)
            {
                StartCoroutine(DropSword(sword));
                yield return new WaitForSeconds(launchInterval);
            }
        }
        yield return new WaitForSeconds(2f);
    }

    IEnumerator DropSword(GameObject sword)
    {
        // 1. 예고 단계 (깜빡이거나 빨간 선 표시)
        SpriteRenderer sr = sword.GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        float aimTime = 0.4f;
        Vector3 finalDirection = Vector3.down;

        while (aimTime > 0)
        {
            if (sword == null) yield break;

            Vector3 targetPos = boss.player.position;
            finalDirection = (targetPos - sword.transform.position).normalized;

            float angle = Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg;
            sword.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            //sr.color = Color.red;
            //yield return new WaitForSeconds(0.1f);
            //sr.color = originalColor;
            //yield return new WaitForSeconds(0.1f);
            aimTime -= Time.deltaTime;
            yield return null;
        }

        // 2. 낙하 단계 (벽 충돌 로직 포함)
        float fallSpeed = 30f;
        float lifeTime = 3f;
        float timer = 0f;
        bool isStuck = false;

        while (timer < lifeTime && !isStuck)
        {
            if (sword == null) yield break;

            float moveDistance = fallSpeed * Time.deltaTime;
            // 레이캐스트로 벽 체크
            RaycastHit2D hit = Physics2D.Raycast(sword.transform.position, finalDirection, moveDistance, Ground);

            if (hit.collider != null)
            {
                sword.transform.position = hit.point;
                isStuck = true;
            }
            else
            {
                sword.transform.position += finalDirection * moveDistance;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // 3. 벽에 박혔을 때 처리
        if (isStuck)
        {
            // 공격 판정 제거 (박힌 칼에 데미지 입지 않도록)
            if (sword.TryGetComponent<Collider2D>(out var col)) col.enabled = false;

            yield return new WaitForSeconds(1.5f); // 1.5초간 박혀있다가 삭제
        }

        Destroy(sword);
    }
}