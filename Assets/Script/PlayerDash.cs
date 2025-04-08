using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashDistance = 3f;
    public float dashDuration = 0.2f;
    public int maxDashCount = 2;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private int currentDashCount;
    private bool isDashing = false;
    private bool isInvincible = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDashCount = maxDashCount;
    }

    void Update()
    {
        if (isDashing) return;

        // 지면 체크로 대쉬 횟수 초기화
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded && currentDashCount < maxDashCount)
        {
            currentDashCount = maxDashCount;
        }

        // 대쉬 입력 (예: X키)
        if (Input.GetKeyDown(KeyCode.X) && currentDashCount > 0)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
            StartCoroutine(DoDash(direction));
        }
    }

    private System.Collections.IEnumerator DoDash(Vector2 direction)
    {
        isDashing = true;
        isInvincible = true;
        currentDashCount--;

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
