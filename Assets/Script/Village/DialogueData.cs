using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueStep
{
    [TextArea(3, 10)]
    public string text;           // NPC 대사
    public string requiredFlag;   // 이 대사가 나오기 위해 필요한 조건
    public string rejectMessage;  // 조건 실패했을 때 나오는 대사

    public List<Choice> choices;  // 여기에 데이터를 넣으면 버튼이 생성됨
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public List<DialogueStep> steps;
}