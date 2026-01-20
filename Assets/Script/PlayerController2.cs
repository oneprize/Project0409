using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController2 : MonoBehaviour, IDamageable
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public int maxJumps = 2;
    public int maxHP = 100;

    private int currentHP;
    private int jumpCount;
    private bool isGrounded;
    private bool wasGrounded;
    private bool isDead = false;
    
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public Slider hpBar;

    //  Game Over UI 연결용
    public GameObject gameOverUI;

    private Camera mainCamera;
    public Transform weaponPivot;

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
        mainCamera = Camera.main;
        jumpCount = maxJumps;
        currentHP = maxHP;

        if (hpBar != null)
        {
            hpBar.maxValue = maxHP;
            hpBar.value = currentHP;
        }

        // 게임 오버 UI 꺼두기
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (isDead)
        {
            // R 키로 씬 재시작
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(moveX));

        HandleSpriteFlip();

        HandleWeaponAiming();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            jumpCount = maxJumps;
        }

        wasGrounded = isGrounded;

        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount--;
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private void HandleSpriteFlip()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void HandleWeaponAiming()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetDir = mousePos - weaponPivot.position;

        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

        if (transform.localScale.x < 0)
        {
            weaponPivot.rotation = Quaternion.Euler(0, 0, angle + 180f);
        }
        else
        {
            weaponPivot.rotation = Quaternion.Euler(0, 0, angle);
        }
    }



    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);

        if (hpBar != null)
        {
            hpBar.value = currentHP;
        }

        Debug.Log("플레이어 피격! 남은 HP: " + currentHP);

        if (currentHP <= 0)
        {
            Debug.Log("플레이어 사망");

            isDead = true;

            if (animator != null)
            {
                animator.SetTrigger("Die");
            }

            rb.linearVelocity = Vector2.zero;

            if (gameOverUI != null)
            {
                gameOverUI.SetActive(true); //  게임 오버 UI 표시
            }
        }
    }
}
