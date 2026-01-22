using UnityEngine;
using TMPro;
using System.Collections;

public class NPCTextEffect : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private string originText; // 인스펙터에 적힌 원본 글자를 저장할 변수

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();

        // 1. 게임 시작 시 인스펙터에 적힌 글자를 미리 기억해둡니다.
        originText = textMeshPro.text;

        // 2. 화면에는 아무것도 안 보이게 일단 지웁니다.
        textMeshPro.text = "";
    }

    // 이 함수를 호출하면 인스펙터에 있던 글자가 한 글자씩 나옵니다!
    public void PlayTypewriter()
    {
        StopAllCoroutines(); // 혹시 실행 중인 게 있다면 멈춤
        StartCoroutine(TypeText(originText));
    }

    private IEnumerator TypeText(string sentence)
    {
        textMeshPro.text = ""; // 다시 한번 비워주기

        foreach (char letter in sentence.ToCharArray())
        {
            textMeshPro.text += letter;
            yield return new WaitForSeconds(0.05f); // 속도 조절
        }
    }
}