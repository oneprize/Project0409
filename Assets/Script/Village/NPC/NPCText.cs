using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class NPCText : MonoBehaviour
{
    [SerializeField] private GameObject fKeyPanel;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject dialoguePanel2;
    [SerializeField] private GameObject Button1;
    [SerializeField] private GameObject Button2;
    [SerializeField] private TypewriterEffect typewriter;

    [Header("대사 목록")]
    [TextArea(3,10)]
    [SerializeField] private List<string> dialogues = new List<string> ();

    private int dialogueIndex = 0;
    private bool isPlayerInRange = false;
    private bool isDialogueActive = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if(!isDialogueActive)
            {
                StartDialogue();
            }
            else
            {
                NextDialogue();
            }
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        fKeyPanel.SetActive(false);
        dialogueIndex = 0;

        typewriter.PlayTypewriter(dialogues[dialogueIndex]);
    }

    private void NextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex == 2)
        {
            dialoguePanel2.SetActive(true);
            Button1.SetActive(true);
            Button2.SetActive(true);
        }

        if (dialogueIndex >= dialogues.Count)
        {
            EndDialogue();
            return;
        }

        typewriter.PlayTypewriter(dialogues[dialogueIndex]);
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        fKeyPanel.SetActive(true);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (fKeyPanel != null)
                fKeyPanel.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (fKeyPanel != null)
                fKeyPanel.SetActive(false);
        }
    }
}
