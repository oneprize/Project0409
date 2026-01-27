using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossMain : MonoBehaviour
{
    public Transform player;
    public Transform skellBoss;
    public Transform leftHand;
    public Transform rightHand;

    [Header("Patterns")]
    public List<BossPattern> patterns;
    private int currentPatternIndex = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        // 모든 패턴 초기화
        foreach (var p in patterns) p.Init(this);
        StartCoroutine(PatternLoop());
    }

    IEnumerator PatternLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // 패턴 사이 대기 시간
            yield return StartCoroutine(patterns[currentPatternIndex].Execute());

            // 다음 패턴으로 (순환)
            currentPatternIndex = (currentPatternIndex + 1) % patterns.Count;
        }
    }
}