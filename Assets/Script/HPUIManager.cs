using UnityEngine;
using UnityEngine.UI;

public class HPUIManager : MonoBehaviour
{
    private PlayerController playerController;

    private Slider slider;
    private GameObject gameOverUI;
    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        slider = gameObject.GetComponentInChildren<Slider>();
        playerController = player.GetComponent<PlayerController>();

        slider.maxValue = playerController.maxHP;

        if (FindObjectsByType<HPUIManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    

    void Update()
    {
        slider.value = playerController.currentHP;
        if (playerController.isDead==true)
        {
            Destroy(gameObject);
        }
    }
}
