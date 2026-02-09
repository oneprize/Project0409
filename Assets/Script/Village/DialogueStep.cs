using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueStep
{
    [TextArea(3, 10)]
    public string text;           // 대사 내용
    public bool showButtons;      // 이 대사에서 예/아니오 버튼을 띄울 것인가?
    public string requiredFlag;   // 이 대사가 나오기 위해 필요한 '사건 이름' (비어있으면 통과)

    [Header("보상 설정")]
    public bool giveItem;        // 아이템을 줄 것인가?
    public string itemID;        // 아이템 이름 
    public int itemCount;        // 아이템 개수
    public string grantFlag;     // 대화 후 활성화할 플래그 
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/NPC Data")]
public class DialogueData : ScriptableObject
{
    public List<DialogueStep> steps;
}