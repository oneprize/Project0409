using UnityEngine;

public class HPUIManager : MonoBehaviour
{
    private PlayerController playerController;
    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        if (FindObjectsByType<HPUIManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (playerController.isDead==true)
        {
            Destroy(gameObject);
        }
    }
}
