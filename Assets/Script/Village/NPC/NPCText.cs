using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class NPCText : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject fKeyPanel;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private GameObject TextBox;
    [SerializeField] private TypewriterEffect typewriter;

    [Header("데이터")]
    [SerializeField] private DialogueData npcDialogueData;

    private int dialogueIndex = 0;
    private bool isPlayerInRange = false;
    private bool isDialogueActive = false;
    private List<GameObject> activeButtons = new List<GameObject>();


    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            // 타자기 효과가 진행 중일 때 F를 누르면 스킵하는 로직이 typewriter에 있다면 좋음
            if (!isDialogueActive)
                StartDialogue();
            else if (activeButtons.Count == 0) // 선택지가 없을 때만 F키로 다음 대사 진행
                NextDialogue();
        }
    }

    private void StartDialogue()
    {
        if (npcDialogueData == null || npcDialogueData.steps.Count == 0) return;

        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        fKeyPanel.SetActive(false);
        TextBox.SetActive(true);
        dialogueIndex = 0;
        ShowCurrentDialogue();
    }

    private void ShowCurrentDialogue()
    {
        ClearButtons();
        DialogueStep currentStep = npcDialogueData.steps[dialogueIndex];

        // 1. 조건 체크 (해당 단계의 조건을 만족하지 못하면 대화 종료 혹은 안내)
        if (!string.IsNullOrEmpty(currentStep.requiredFlag) && !GameEventManager.IsEventCompleted(currentStep.requiredFlag))
        {
            typewriter.PlayTypewriter("아직 나랑 대화할 준비가 안 된 것 같군.");
            // 보너스: 조건 안 맞으면 2초 뒤 종료하거나 다음 클릭 시 종료되게 처리 필요
            return;
        }

        typewriter.PlayTypewriter(currentStep.text);

        // 2. 선택지 생성
        if (currentStep.choices != null && currentStep.choices.Count > 0)
        {
            choicePanel.SetActive(true);
            foreach (Choice choice in currentStep.choices)
            {
                CreateButton(choice);
            }
        }
    }

    private void CreateButton(Choice choiceData)
    {
        if (choiceButtonPrefab == null) Debug.Log("프리팹없음");
        GameObject btnObj = Instantiate(choiceButtonPrefab, choicePanel.transform);
        activeButtons.Add(btnObj);

        // 텍스트 설정
        TMP_Text btnText = btnObj.GetComponentInChildren<TMP_Text>();
        if (btnText != null) btnText.text = choiceData.choiceText;

        // 버튼 클릭 이벤트 (람다식 사용 시 데이터 오염 방지를 위해 로컬 변수화)
        btnObj.GetComponent<Button>().onClick.AddListener(() => OnChoiceSelected(choiceData));
    }

    private void OnChoiceSelected(Choice choice)
    {
        Debug.Log($"[선택지 클릭] 선택한 텍스트: {choice.choiceText}, grantFlag: {choice.grantFlag}");
        // 1. 보상 처리
        if (choice.giveItem) ItemManager.AddItem(choice.itemPrefab, choice.itemCount, transform.position);

        // 2. 씬 이동 로직 (매니저에게 위임)
        if (!string.IsNullOrEmpty(choice.grantFlag) && choice.grantFlag.StartsWith("GoToNextScene"))
        {
            string targetScene = choice.grantFlag.Replace("GoToNextScene", "Dungeon0");
            Debug.Log($"[씬 전환 시도] 대상 씬 이름: {targetScene}");
            // UI를 미리 정리
            ClearButtons();
            dialoguePanel.SetActive(false);
            TextBox.SetActive(false);

            // 매니저 호출
            if (SceneTransferManager.Instance != null)
            {
                SceneTransferManager.Instance.ChangeScene(targetScene);
            }
            else
            {
                Debug.LogError("SceneTransferManager 인스턴스가 씬에 없습니다!");
            }
            return;
        }

        // 3. 일반 플래그 처리 및 다음 대화
        if (!string.IsNullOrEmpty(choice.grantFlag))
        {
            GameEventManager.CompleteEvent(choice.grantFlag);
        }

        ClearButtons();
        NextDialogue();
    }

    private void ClearButtons()
    {
        foreach (GameObject btn in activeButtons) Destroy(btn);
        activeButtons.Clear();
        if (choicePanel != null) choicePanel.SetActive(false);
    }

    private void NextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex >= npcDialogueData.steps.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentDialogue();
        }
    }

    public void EndDialogue()
    {
        if (this == null) return;

        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        if (isPlayerInRange && fKeyPanel != null) fKeyPanel.SetActive(true);
        TextBox.SetActive(false);
        ClearButtons();
        dialogueIndex = 0;
    }

    // --- 트리거 부분 ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (fKeyPanel != null && !isDialogueActive) fKeyPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (fKeyPanel != null) fKeyPanel.SetActive(false);
            if (isDialogueActive) EndDialogue(); // 멀어지면 대화 강제 종료
        }
    }
}