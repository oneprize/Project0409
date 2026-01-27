using UnityEngine;
using System.Collections;

public class Pattern1_Laser : BossPattern
{
    public GameObject laserPrefab;    // 실제 발사될 레이저 프리팹
    public float trackingTime = 1.5f; // Y축 추적 시간
    public float readyTime = 0.5f;    // 위치 고정 후 발사까지 대기 시간
    public float laserDuration = 1.0f; // 레이저가 유지되는 시간

    [Header("Movement Settings")]
    public float followSpeed = 5.0f; // 손이 따라오는 속도

    public override IEnumerator Execute()
    {
        Debug.Log("패턴 1: 레이저 전개");
        // 1. 왼손 공격
        yield return StartCoroutine(HandAttackSequence(boss.leftHand));
        yield return new WaitForSeconds(3f);

        // 2. 오른손 공격
        yield return StartCoroutine(HandAttackSequence(boss.rightHand));
    }

    IEnumerator HandAttackSequence(Transform hand)
    {
        Transform firePoint = hand.Find("FirePoint");
        LineRenderer previewLine = null;

        if (firePoint != null)
        {
            previewLine = firePoint.GetComponent<LineRenderer>();
        }

        float timer = 0;
        
        // 1단계: Y축 추적 및 예고선
        if (previewLine != null) previewLine.enabled = true;

        while (timer < trackingTime)
        {         
            // 플레이어 y축 추적
            float targetY = boss.player.position.y;
            Vector3 targetPosition = new Vector3(hand.position.x, targetY, 0);
            hand.position = Vector3.Lerp(hand.position, targetPosition, Time.deltaTime * followSpeed);

            // 예고선 표시
            if (previewLine != null && firePoint != null)
            {
                previewLine.SetPosition(0, firePoint.position);

                // 레이저가 나갈 방향               
                float direction = (firePoint.position.x < boss.player.position.x) ? 1f : -1f;
                previewLine.SetPosition(1, firePoint.position + Vector3.right * direction * 20f);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // --- [2단계: 위치 고정 및 발사 대기] ---
        // 이 시점부터는 hand.position을 건드리지 않으므로 고정됩니다.       
        yield return new WaitForSeconds(readyTime);

        // --- [3단계: 레이저 발사] ---
        if (previewLine != null) previewLine.enabled = false;
        FireLaser(hand);

        yield return new WaitForSeconds(laserDuration);
    }

    void FireLaser(Transform hand)
    {
        Transform firePoint = hand.Find("FirePoint");
        Vector3 spawnPos = (firePoint != null) ? firePoint.position : hand.position;
        if (firePoint !=null)
        {
            Debug.Log("예시선 표시");
        }
        else
        {
            Debug.Log("예시선 표시 불가");
        }

            // 손의 위치에 레이저 생성
            GameObject laser = Instantiate(laserPrefab, hand.position, Quaternion.identity);

        // 보스 방향에 따른 레이저 방향 설정
        float direction = (hand.position.x < boss.player.position.x) ? 1f : -1f;
        laser.transform.localScale = new Vector3(direction, 1, 1);

        Destroy(laser, laserDuration);
    }
}