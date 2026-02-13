using UnityEngine;
using UnityEngine.UI;

public class BossHPManager : MonoBehaviour
{
    public Slider slider;
    public int maxHP = 100;
    public int currentHP;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        currentHP = maxHP;
        slider.maxValue = maxHP;
        slider.value = currentHP;
    }

    private void TakeDamage(int Damage)
    {

    }
}
