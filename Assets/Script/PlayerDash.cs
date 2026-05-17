using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerDash : MonoBehaviour
{
    public float dashDistance = 3f;
    public float dashDuration = 0.2f;
    public int maxDashCount = 2;
    public float dashCooldown = 1.5f;

    public float currentDashCount;
    private bool isDashing = false;
    private bool isInvincible = false;

    private Rigidbody2D rb;
    public Slider dashBar;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDashCount = maxDashCount;
        // dashBar.maxValue = maxDashCount;
        // dashBar.value = currentDashCount;
    }

    void Update()
    {
        if (isDashing) return;

        // 시간이 지남에 따라 대쉬 횟수 차징
        if(currentDashCount < maxDashCount)
        {
            currentDashCount += Time.deltaTime / dashCooldown;
            // dashBar.value = currentDashCount;
        }

        // 대쉬 입력
        if (Input.GetMouseButtonDown(1) && currentDashCount > 0)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
            StartCoroutine(DoDash(direction));
        }
    }

    private IEnumerator DoDash(Vector2 direction)
    {
        isDashing = true;
        isInvincible = true;
        currentDashCount--;
        // dashBar.value = currentDashCount;

        float dashSpeed = dashDistance / dashDuration;
        float timer = 0f;

        while (timer < dashDuration)
        {
            rb.linearVelocity = direction * dashSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        isInvincible = false;
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
