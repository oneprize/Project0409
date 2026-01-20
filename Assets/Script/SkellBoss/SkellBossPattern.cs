using UnityEngine;
using System.Collections;

public abstract class BossPattern : MonoBehaviour
{
    protected BossMain boss; // 보스 메인 참조
    public float patternDuration; // 패턴 지속 시간

    public virtual void Init(BossMain main)
    {
        boss = main;
    }

    // 패턴 시작, 실행, 종료 (추상 메서드)
    public abstract IEnumerator Execute();
}