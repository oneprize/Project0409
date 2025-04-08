using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public int maxJumps = 2;
    public int maxHP = 100;

    private int currentHP;
    private int jumpCount;
    private bool isGrounded;
    private bool wasGrounded;

    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Awake()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpCount = maxJumps;
        currentHP = maxHP;
    }

    void Update()
    {
        // 이동 입력
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // 애니메이션
        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(moveX));

        // 좌우 반전
        if (moveX != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1);

        // 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            jumpCount = maxJumps;
        }

        wasGrounded = isGrounded;

        // 점프
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount--;
        }

        // 점프 버튼 짧게 누르면 낮게 뜀
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    // === 공격 받는 기능 ===
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("플레이어 피격! 남은 HP: " + currentHP);

        if (currentHP <= 0)
        {
            Debug.Log("플레이어 사망");
            // TODO: 게임 오버 처리
        }
    }
}
