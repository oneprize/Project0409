using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class BossMain : MonoBehaviour
{
    public Transform player;
    public Transform skellBoss;
    public Transform leftHand;
    public Transform rightHand;

    // 손의 원래 위치 저장을 위한 변수
    private Vector3 leftHandOrigin;
    private Vector3 rightHandOrigin;

    [Header("Patterns")]
    public List<BossPattern> patterns;
    private int lastPatternIndex = -1; // 직전 패턴 기억

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // 시작 시 손의 로컬 위치 저장 (부모가 보스 몸체라면 로컬 위치가 편함)
        leftHandOrigin = leftHand.localPosition;
        rightHandOrigin = rightHand.localPosition;

        foreach (var p in patterns) p.Init(this);
        StartCoroutine(PatternLoop());
    }

    IEnumerator PatternLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            // 1. 랜덤 인덱스 결정 (직전 패턴과 겹치지 않게)
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, patterns.Count);
            } while (randomIndex == lastPatternIndex && patterns.Count > 1);

            lastPatternIndex = randomIndex;

            // 2. 패턴 실행
            yield return StartCoroutine(patterns[randomIndex].Execute());

            // 3. 패턴 종료 후 손 위치 복귀
            yield return StartCoroutine(ReturnHands());
        }
    }

    IEnumerator ReturnHands()
    {
        float duration = 1.0f; // 복귀에 걸리는 시간
        float elapsed = 0f;
        Vector3 startL = leftHand.localPosition;
        Vector3 startR = rightHand.localPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            leftHand.localPosition = Vector3.Lerp(startL, leftHandOrigin, elapsed / duration);
            rightHand.localPosition = Vector3.Lerp(startR, rightHandOrigin, elapsed / duration);
            yield return null;
        }
    }
}