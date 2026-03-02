using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Bossroom : MonoBehaviour
{
    public CinemachineCamera playerCamera;
    public CinemachineCamera bossCamera;

    [Header("Boss Reference")]
    public BossMain bossMain;

    private GameObject player;
    public GameObject DungeonDoor;
    public GameObject DungeonDoor2;
    public GameObject HPUICanvas;

    private float focusTime = 3f;
    private bool isBossZoomIN = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isBossZoomIN)
        {
            isBossZoomIN = true;
            DungeonDoor.SetActive(true);
            DungeonDoor2.SetActive(true);
            StartCoroutine(BossZoom(collision.gameObject));
        }
    }
    IEnumerator BossZoom(GameObject player)
    {
        var controller = player.GetComponent<PlayerController>();
        var rb = player.GetComponent<Rigidbody2D>();
        var anim = player.GetComponent<Animator>();

        if (controller != null) controller.enabled = false;
        if (rb != null) rb.linearVelocity = Vector2.zero;
        if (anim != null) anim.SetFloat("Speed", 0f);

        bossCamera.Priority = 20;
        yield return new WaitForSeconds(focusTime);
        bossCamera.Priority = 10;
        yield return new WaitForSeconds(1.5f);
        if (controller != null) controller.enabled = true;
        if (bossMain != null)
        {
            bossMain.ActivateBoss();
        }
        else
        {
            Debug.LogError("BossMain이 Bossroom 스크립트에 할당되지 않았습니다!");
        }
        HPUICanvas.SetActive(true);
    }
}
