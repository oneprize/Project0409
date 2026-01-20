using UnityEngine;
using UnityEngine.InputSystem;

public class NPCText : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject textUI;

    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            ToggleTextUI();
        }
    }

    private void ToggleTextUI()
    {
        if (textUI!=null)
        {
            bool isActive = textUI.activeSelf;
            textUI.SetActive(!isActive);

            if (uiPanel != null)
            {
                uiPanel.SetActive(isActive);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (uiPanel != null)
                uiPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (uiPanel != null)
                uiPanel.SetActive(false);
        }
    }
}
