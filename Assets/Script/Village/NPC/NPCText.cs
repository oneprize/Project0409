using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;

public class NPCText : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject fKeyPanel;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private GameObject TextBox;
    [SerializeField] private GameObject player;
    [SerializeField] private TypewriterEffect typewriter;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAttack playerAttack;

    [Header("데이터")]
    [SerializeField] private DialogueData dialogueData;

    private int dialogueIndex = 0;
    private bool isPlayerInRange = false;
    private bool isDialogueActive = false;
    private List<GameObject> activeButtons = new List<GameObject>();

    private void RefreshPlayerReference()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (player != null)
        {
            if (playerController == null)
                playerController = player.GetComponent<PlayerController>();

            // 무기는 자식에 있으므로 필요할 때마다 갱신
            if (playerAttack == null)
                playerAttack = player.GetComponentInChildren<PlayerAttack>();
        }
    }
    
    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            RefreshPlayerReference();

            if (!isDialogueActive)
                StartDialogue();
            else if (activeButtons.Count == 0) 
                NextDialogue();
        }
    }

    private void StartDialogue()
    {
        if (dialogueData == null || dialogueData.steps.Count == 0) return;

        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        fKeyPanel.SetActive(false);
        TextBox.SetActive(true);
        dialogueIndex = 0;
        ShowCurrentDialogue();
        if(playerController != null)playerController.enabled = false;
        if(playerAttack != null) playerAttack.enabled = false;
    }

    private void ShowCurrentDialogue()
    {
        ClearButtons();
        DialogueStep currentStep = dialogueData.steps[dialogueIndex];

        // 1. 조건 체크 (해당 단계의 조건을 만족하지 못하면 대화 종료 혹은 안내)
        if (!string.IsNullOrEmpty(currentStep.requiredFlag) && !GameEventManager.IsEventCompleted(currentStep.requiredFlag))
        {
            // 2. 거절 대사 출력
            typewriter.PlayTypewriter(currentStep.rejectMessage);

            // 3. 핵심: dialogueIndex를 끝으로 밀어버려서 F키를 눌러도 다음 대사가 안 나오게 함
            dialogueIndex = dialogueData.steps.Count;

            // 4. 선택지를 생성하지 않고 여기서 리턴하여 대화 흐름을 끊음
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
        if (!string.IsNullOrEmpty(choice.checkFlag))
        {
            // 만약 조건을 만족하지 못했다면?
            if (!GameEventManager.IsEventCompleted(choice.checkFlag))
            {
                // 실패 메시지 출력 (실패 시 대화 종료 처리)
                typewriter.PlayTypewriter(choice.failMessage);

                // 더 이상 진행하지 못하게 버튼만 지우고 대화 인덱스 끝으로 이동
                ClearButtons();
                dialogueIndex = dialogueData.steps.Count;
                return; // 여기서 함수 종료 (NextDialogue 호출 안 함)
            }
        }

        Debug.Log($"[선택지 클릭] 선택한 텍스트: {choice.choiceText}, grantFlag: {choice.grantFlag}");
        // 1. 보상 처리
        if (choice.giveItem)
        {
            ItemManager.AddItem(choice.itemPrefab, choice.itemCount, transform.position);

            Invoke(nameof(RefreshPlayerReference), 0.1f);
        }

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
            EndDialogue();
            return;
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

        if (dialogueIndex >= dialogueData.steps.Count)
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

        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (isPlayerInRange && fKeyPanel != null) fKeyPanel.SetActive(true);
        if (TextBox != null) TextBox.SetActive(false);
        ClearButtons();
        dialogueIndex = 0;
        if(playerController != null) playerController.enabled = true;
        if(playerAttack != null) playerAttack.enabled = true;
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