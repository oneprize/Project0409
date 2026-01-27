using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern2_Barrage : BossPattern
{
    public GameObject bulletPrefab;
    public int bulletCount = 4;
    public float spawnInterval = 2.0f; // 탄막 생성 간격 (1초)
    public float startRadius = 0.5f;
    public float expandSpeed = 2.0f;
    public float rotateSpeed = 150f;
    public float individualDuration = 15.0f; // 생성된 탄막 묶음이 유지되는 시간

    public override IEnumerator Execute()
    {
        Debug.Log("패턴 2: 탄막 피하기");
        float timer = 0f;
        float lastSpawnTime = -spawnInterval; // 시작하자마자 첫 번째 탄막을 만들기 위함

        while (timer < patternDuration)
        {
            // 1. 일정 시간(spawnInterval)마다 새로운 탄막 묶음 생성
            if (timer - lastSpawnTime >= spawnInterval)
            {
                StartCoroutine(SpawnBarrageWave()); // 서브 코루틴 실행
                lastSpawnTime = timer;
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    // 새로운 탄막 한 묶음을 생성하고 관리하는 서브 코루틴
    IEnumerator SpawnBarrageWave()
    {
        GameObject container = new GameObject("BarrageWave");
        container.transform.position = boss.transform.position;
        List<GameObject> bullets = new List<GameObject>();

        // 1. 탄알 생성 및 초기화
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, container.transform);
            float angle = i * Mathf.PI * 2f / bulletCount;
            bullet.transform.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * startRadius;
            bullets.Add(bullet);
        }

        // 2. 이 묶음만 별도로 회전 및 팽창
        float waveTimer = 0f;
        float currentRadius = startRadius;

        while (waveTimer < individualDuration)
        {
            if (container == null) break;

            container.transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
            currentRadius += expandSpeed * Time.deltaTime;

            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i] == null) continue;
                float angle = i * Mathf.PI * 2f / bulletCount;
                bullets[i].transform.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * currentRadius;
            }

            waveTimer += Time.deltaTime;
            yield return null;
        }

        // 3. 시간이 다 되면 이 묶음 삭제
        Destroy(container);
    }
}