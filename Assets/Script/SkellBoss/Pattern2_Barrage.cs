using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Pattern2_Barrage : BossPattern
{
    public GameObject bulletPrefab;
    public int bulletCount = 4;
    public float spawnInterval = 2.0f;
    public float startRadius = 0.5f;
    public float expandSpeed = 2.0f;
    public float rotateSpeed = 150f;
    public float individualDuration = 15.0f;
    public Vector3 barragePoint = new Vector3(0.5f,-1.6f,0f);


    public override IEnumerator Execute()
    {
        Animator bossAnim = boss.GetComponent<Animator>();
        bossAnim.SetTrigger("BulletAttack");
        float timer = 0f;
        float lastSpawnTime = -spawnInterval;
        float totalEffectTime = patternDuration + individualDuration;

        GameObject totalRotator = new GameObject("TotalRotator");
        totalRotator.transform.position = boss.transform.position + barragePoint;

        while (timer < totalEffectTime)
        {
            totalRotator.transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

            if (timer < patternDuration && timer - lastSpawnTime >= spawnInterval)
            {
                StartCoroutine(SpawnBarrageWaveWithRotator(totalRotator.transform));
                lastSpawnTime = timer;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(totalRotator);
    }

    IEnumerator SpawnBarrageWaveWithRotator(Transform parentRotator)
    {
        GameObject waveContainer = new GameObject("BarrageWave");
        waveContainer.transform.SetParent(parentRotator);
        waveContainer.transform.localPosition = Vector3.zero;
        waveContainer.transform.localRotation = Quaternion.identity;

        List<GameObject> bullets = new List<GameObject>();

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, waveContainer.transform);
            float angle = i * Mathf.PI * 2f / bulletCount;
            bullet.transform.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * startRadius;
            bullets.Add(bullet);
        }

        float waveTimer = 0f;
        float currentRadius = startRadius;

        while (waveTimer < individualDuration)
        {
            if (waveContainer == null) break;

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

        Destroy(waveContainer);
    }
}