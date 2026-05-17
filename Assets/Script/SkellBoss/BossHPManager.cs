using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHPManager : MonoBehaviour, IDamageable
{
    public Slider slider;
    private int maxHP = 100;
    private int currentHP;
    private Animator animator;

    public GameObject bossLaftHand;
    public GameObject bossRightHand;
    public GameObject bossdead;
    public GameObject victoryCanvas;
    public GameObject victoryPanel;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHP = maxHP;
        slider.maxValue = maxHP;
        slider.value = currentHP;
    }

    public void TakeDamage(int Damage)
    {
        if (currentHP < 0) return;

        currentHP -= Damage;
        slider.value = currentHP;

        Debug.Log("보스 피격! 남은 HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        StartCoroutine(DieSequence());
    }

    IEnumerator DieSequence()
    {
        animator.SetTrigger("Die");
        Instantiate(bossdead, transform.position, transform.rotation);
        victoryCanvas.SetActive(true);
        yield return new WaitForSeconds(2f);
        victoryPanel.SetActive(true);

        yield return new WaitForSeconds(3f);

        GameObject[] lasers = GameObject.FindGameObjectsWithTag("Laser");
        foreach (GameObject a in lasers) Destroy(a);
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject b in bullets) Destroy(b);
        GameObject[] swords = GameObject.FindGameObjectsWithTag("Sword");
        foreach (GameObject c in swords) Destroy(c);
        // 5. 보스 본체 파괴
        Destroy(gameObject);
    }
}
