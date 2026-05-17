using UnityEngine;
using UnityEngine.UI;

public class HPUIManager : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerDash playerDash;

    [SerializeField] private Slider HPBar;
    [SerializeField] private Slider DashBar;
    private GameObject gameOverUI;
    void Awake()
    {
        if (FindObjectsByType<HPUIManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        HPBar = transform.Find("HP Bar").GetComponent<Slider>();
        DashBar = transform.Find("Dash Bar").GetComponent<Slider>();
    
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerDash = player.GetComponent<PlayerDash>();

        HPBar.maxValue = playerController.maxHP;
        DashBar.maxValue = playerDash.maxDashCount;

        
    }
    

    void Update()
    {
        HPBar.value = playerController.currentHP;
        DashBar.value = playerDash.currentDashCount;
        if (playerController.isDead==true)
        {
            Destroy(gameObject);
        }
    }
}
