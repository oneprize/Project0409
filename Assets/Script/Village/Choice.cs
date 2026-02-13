using UnityEngine;

[System.Serializable]
public class Choice
{
    public string choiceText;    // 버튼에 표시될 텍스트 (예: "무기를 가져간다")

    [Header("보상/결과")]
    public string grantFlag;     // 선택 시 완료될 이벤트 이름
    public bool giveItem;        // 아이템 지급 여부
    public GameObject itemPrefab;
    public int itemCount=1;
}