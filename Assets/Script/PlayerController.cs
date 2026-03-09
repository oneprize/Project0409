using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour, IDamageable
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public int maxJumps = 2;
    public int maxHP = 100;

    public int currentHP;
    private int jumpCount;
    private bool isGrounded;
    private bool wasGrounded;
    public bool isDead = false;
    
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public Slider hpBar;

    //  Game Over UI 연결용
    public GameObject gameOverUI;
    private GameObject player;

    private Camera mainCamera;
    private PlayerDash playerDash;
    private PlayerAttack playerAttack;
    public Transform weaponPivot;

    [SerializeField] Transform groundCheck2;
    [SerializeField] float groundCheckRadius = 0.1f;
    [SerializeField] LayerMask platformLayer;

    bool IsOnPlatform()
    {
        return Physics2D.OverlapCircle(groundCheck2.position, groundCheckRadius, platformLayer);
    }

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
        playerDash = GetComponent<PlayerDash>();
        mainCamera = Camera.main;
        jumpCount = maxJumps;
        currentHP = maxHP;

        //if (hpBar != null)
        //{
        //    hpBar.maxValue = maxHP;
        //    hpBar.value = currentHP;
        //}

        // 게임 오버 UI 꺼두기
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (isDead)
        {
            Die();
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

        if (Input.GetButtonDown("Jump") && jumpCount > 0 && !(Input.GetAxisRaw("Vertical")<0))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount--;
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (IsOnPlatform() && Input.GetAxisRaw("Vertical") < 0 && Input.GetButtonDown("Jump"))
        {            
            StartCoroutine(DownJump());
        }
    }

    IEnumerator DownJump()
    {
        
        Collider2D[] platforms = Physics2D.OverlapCircleAll(groundCheck2.position, groundCheckRadius, platformLayer);

        if (platforms.Length > 0)
        {
            foreach (var platform in platforms)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), platform, true);
            }

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -jumpForce * 0.5f);

            yield return new WaitForSeconds(0.4f);

            foreach (var platform in platforms)
            {
                if (platform != null)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), platform, false);
                }
            }
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
        if (playerDash.IsInvincible()) return;

        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);

        //if (hpBar != null)
        //{
        //    hpBar.value = currentHP;
        //}

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
    private void PlayerAttackFounder()
    {
        player = GameObject.FindWithTag("Player");
        if (playerAttack == null)
            playerAttack = player.GetComponentInChildren<PlayerAttack>();
    }
    public void Die()
    {
        if (isDead)
        {
            rb.linearVelocity= Vector2.zero;
            animator.SetFloat("Speed", 0);
            if(playerAttack != null) playerAttack.enabled = false;
            // R 키로 씬 재시작
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetGame();
            }
            return;
        }
    }

    private void ResetGame()
    {
        isDead = false;
        currentHP = maxHP;
        jumpCount = maxJumps;

        // 2. UI 및 컴포넌트 복구
        if (hpBar != null) hpBar.value = maxHP;
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (playerAttack != null) playerAttack.enabled = true;

        if (animator != null)
        {
            animator.Rebind(); // 애니메이터의 모든 파라미터와 상태를 초기 상태로 리셋
            animator.Update(0f);
        }

        SceneManager.LoadScene("Village");
    }
}
