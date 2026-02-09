using UnityEngine;
using System.Collections;

public class Pattern1_Laser : BossPattern
{
    public GameObject laserPrefab;
    public float trackingTime = 1.5f; // Y축 추적 시간
    public float readyTime = 0.5f;    // 추적 후 발사 전 대기 (애니메이션 시작 전 여유)
    public float laserDuration = 1.0f;

    [Header("Movement Settings")]
    public float followSpeed = 5.0f;
    public float minY = -4.0f;
    public float maxY = 4.0f;

    [Header("Animation Sync")]
    [Range(0f, 1f)]
    public float fireTiming = 0.7f; // 애니메이션이 몇 % 진행됐을 때 레이저를 쏠지 (0.7 = 70%)
    public string flashAnimName = "Hand_Flash"; // 손이 번쩍이는 애니메이션 상태 이름

    public override IEnumerator Execute()
    {
        Debug.Log("패턴 1: 레이저 전개");

        // 1. 왼손 공격 시퀀스 시작
        yield return StartCoroutine(HandAttackSequence(boss.leftHand));
        yield return new WaitForSeconds(1.0f); // 양손 공격 사이 간격

        // 2. 오른손 공격 시퀀스 시작
        yield return StartCoroutine(HandAttackSequence(boss.rightHand));
    }

    IEnumerator HandAttackSequence(Transform hand)
    {
        Animator handAnim = hand.GetComponent<Animator>();
        Transform firePoint = hand.Find("FirePoint");
        LineRenderer previewLine = (firePoint != null) ? firePoint.GetComponent<LineRenderer>() : null;

        float timer = 0;
        if (previewLine != null) previewLine.enabled = true;

        // --- [1단계: Y축 추적 및 예고선] ---
        while (timer < trackingTime)
        {
            float targetY = Mathf.Clamp(boss.player.position.y, minY, maxY);
            Vector3 targetPosition = new Vector3(hand.position.x, targetY, 0);
            hand.position = Vector3.Lerp(hand.position, targetPosition, Time.deltaTime * followSpeed);

            //if (previewLine != null && firePoint != null)
            //{
            //    previewLine.SetPosition(0, firePoint.position);
            //    float direction = (firePoint.position.x < boss.player.position.x) ? 1f : -1f;
            //    previewLine.SetPosition(1, firePoint.position + Vector3.right * direction * 20f);
            //}

            timer += Time.deltaTime;
            yield return null;
        }

        // --- [2단계: 위치 고정 및 애니메이션 동기화 대기] ---
        yield return new WaitForSeconds(readyTime);

        if (handAnim != null)
        {
            // 1. 트리거 실행
            handAnim.SetTrigger("Flash");

            // 2. 중요: 현재 상태가 FlashAnimName으로 바뀔 때까지 대기
            // (전환(Transition) 중에 이전 상태의 시간을 읽는 것을 방지)
            yield return new WaitUntil(() => handAnim.GetCurrentAnimatorStateInfo(0).IsName(flashAnimName));

            // 3. 이제 정확히 FlashAnimName 상태이므로, 진행도가 fireTiming(0.7f)이 될 때까지 대기
            while (handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < fireTiming)
            {
                yield return null;
            }
        }

        // --- [3단계: 레이저 발사] ---
        // if (previewLine != null) previewLine.enabled = false;
        FireLaser(hand);

        yield return new WaitForSeconds(laserDuration);
    }

    void FireLaser(Transform hand)
    {
        Transform firePoint = hand.Find("FirePoint");
        Vector3 spawnPos = (firePoint != null) ? firePoint.position : hand.position;

        GameObject laser = Instantiate(laserPrefab, spawnPos, Quaternion.identity);

        float direction = (hand.position.x < boss.player.position.x) ? 1f : -1f;
        laser.transform.localScale = new Vector3(direction, 1, 1);

        Destroy(laser, laserDuration);
    }
}