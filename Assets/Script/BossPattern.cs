using System.Collections;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    public GameObject bossPatternHand1;             // 레이저 시각 효과 포함된 손
    public BossHandFollow bossHandFollow;           // 움직임 제어용
    public Animator bossAnimator;                   // 팔 애니메이션
    public float patternCooldown = 5f;              // 패턴 쿨타임

    private bool isOnCooldown = false;

    public void BossPatternOn()
    {
        if (isOnCooldown) return;

        StartCoroutine(ExecutePattern());
    }

    IEnumerator ExecutePattern()
    {
        isOnCooldown = true;

        // 보스 움직임 멈춤
        if (bossHandFollow != null)
            bossHandFollow.enabled = false;

        // 손 오브젝트 활성화
        bossPatternHand1.SetActive(true);

        // 애니메이션 트리거 (레이저 애니메이션 실행)
        if (bossAnimator != null)
            bossAnimator.SetTrigger("Pattern1");

        // 0.5초 대기 (이후 HideLaser 또는 Stop 애니메이션 이벤트 호출되도록)
        yield return new WaitForSeconds(0.5f);

        // 손 오브젝트 비활성화
        bossPatternHand1.SetActive(false);

        // 움직임 다시 활성화
        if (bossHandFollow != null)
            bossHandFollow.enabled = true;

        // 쿨타임 대기
        yield return new WaitForSeconds(patternCooldown);
        isOnCooldown = false;
    }
}
