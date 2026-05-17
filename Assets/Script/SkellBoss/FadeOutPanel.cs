using UnityEngine;
using System.Collections;
public class FadeOutPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1.0f; // 페이드 속도

    void OnEnable()
    {
        // 오브젝트가 활성화될 때마다 페이드 아웃 실행
        StartCoroutine(FadeOut());
    }
    public IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 1;
    }
}
