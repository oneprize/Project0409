using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class BossDeadCamera : MonoBehaviour
{
    public CinemachineCamera playerCamera;
    public CinemachineCamera bossCamera;

    public BossMain bossMain;

    private GameObject player;

    private int focusTime = 3;

    private void BossDie()
    {
        // StartCoroutine(BossDead(GameObject player));
    }

    IEnumerator BossDead(GameObject player)
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
    }
}
