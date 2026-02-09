using UnityEngine;

public class NPCText : MonoBehaviour
{
    [SerializeField] private GameObject fKeyPanel;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject dialoguePanel2;
    [SerializeField] private GameObject YesButton;
    [SerializeField] private GameObject NoButton;
    [SerializeField] private GameObject TextBox;
    [SerializeField] private TypewriterEffect typewriter;

    [Header("대화 데이터")]
    [SerializeField] private DialogueData npcDialogueData; // 리스트 대신 데이터 에셋을 넣습니다.

    private int dialogueIndex = 0;
    private bool isPlayerInRange = false;
    private bool isDialogueActive = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (!isDialogueActive) StartDialogue();
            else NextDialogue();
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        fKeyPanel.SetActive(false);
        TextBox.SetActive(true);
        dialogueIndex = 0;

        ShowCurrentDialogue();
    }

    private void NextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex >= npcDialogueData.steps.Count)
        {
            EndDialogue();
            return;
        }

        ShowCurrentDialogue();
    }

    private void ShowCurrentDialogue()
    {
        DialogueStep currentStep = npcDialogueData.steps[dialogueIndex];

        // 핵심: 조건을 검사합니다!
        if (!GameEventManager.IsEventCompleted(currentStep.requiredFlag))
        {
            typewriter.PlayTypewriter("아직 나랑 대화할 준비가 안 된 것 같군.");
            // 여기서 강제로 대화를 끊거나, 이전 인덱스로 돌릴 수 있습니다.
            return;
        }

        // 버튼 활성화 여부를 데이터에 따라 결정합니다.
        bool shouldShowButtons = currentStep.showButtons;
        dialoguePanel2.SetActive(shouldShowButtons);
        YesButton.SetActive(shouldShowButtons);
        NoButton.SetActive(shouldShowButtons);

        typewriter.PlayTypewriter(currentStep.text);
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        fKeyPanel.SetActive(true);
        // 대화가 끝나면 이 NPC와 대화했다는 플래그를 저장할 수도 있습니다.
        // GameEventManager.CompleteEvent("TalkedTo_" + gameObject.name);
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

    public void OnClickYes()
    {
        DialogueStep currentStep = npcDialogueData.steps[dialogueIndex];

        // 1. 아이템 지급 처리
        if (currentStep.giveItem)
        {
            ItemManager.AddItem(currentStep.itemID, currentStep.itemCount);
        }

        // 2. 퀘스트 완료 플래그 등 처리
        if (!string.IsNullOrEmpty(currentStep.grantFlag))
        {
            GameEventManager.CompleteEvent(currentStep.grantFlag);
        }

        // 3. 버튼 끄고 다음 대사로
        CloseButtons();
        NextDialogue();
    }

    public void OnClickNo()
    {
        // 거절했을 때의 로직 (예: 대화 종료)
        CloseButtons();
        EndDialogue();
    }

    private void CloseButtons()
    {
        dialoguePanel2.SetActive(false);
        YesButton.SetActive(false);
        NoButton.SetActive(false);
    }
}