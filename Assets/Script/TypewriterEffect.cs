using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private float typingSpeed = 0.05f;
    private Coroutine typingCuroutine;

    public void PlayTypewriter(string message)
    {
        if (typingCuroutine != null)
        {
            StopCoroutine (typingCuroutine);
        }
        typingCuroutine = StartCoroutine(TypeText(message));
    }

    public IEnumerator TypeText(string sentence)
    {
        textMeshPro.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            textMeshPro.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
