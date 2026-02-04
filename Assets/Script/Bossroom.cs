using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Bossroom : MonoBehaviour
{
    public CinemachineCamera playerCamera;
    public CinemachineCamera bossCamera;

    private GameObject player;

    private float focusTime = 3f;
    private bool isBossZoomIN = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 변수 할당 여부와 상관없이 충돌한 대상의 태그만 확인하면 됩니다.
        if (collision.CompareTag("Player") && !isBossZoomIN)
        {
            isBossZoomIN = true; // 실행됨을 표시
            StartCoroutine(BossZoom(collision.gameObject));
        }
    }
    IEnumerator BossZoom(GameObject player)
    {
        Debug.Log("코루틴 실행");
        var controller = player.GetComponent<PlayerController2>();
        if (controller != null) controller.enabled = false;

        bossCamera.Priority = 20;
        yield return new WaitForSeconds(focusTime);
        bossCamera.Priority = 10;
        yield return new WaitForSeconds(1.5f);
        if (controller != null) controller.enabled = true;
    }
}
