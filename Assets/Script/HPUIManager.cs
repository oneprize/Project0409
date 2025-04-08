using UnityEngine;

public class HPUIManager : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsByType<HPUIManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
