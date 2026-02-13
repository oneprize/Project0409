using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransferManager : MonoBehaviour
{
    public static SceneTransferManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않음
        }
        else { Destroy(gameObject); }
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(SceneRoutine(sceneName));
    }

    private IEnumerator SceneRoutine(string sceneName)
    {
        // 1. 페이드 아웃 시작 (화면이 검어짐)
        if (FadeManager.Instance != null)
        {
            yield return StartCoroutine(FadeManager.Instance.FadeOut());
        }

        // 2. 실제 씬 로드 (비동기)
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
        {
            yield return null;
        }

        // 3. 페이드 인 시작 (새 씬에서 화면이 밝아짐)
        if (FadeManager.Instance != null)
        {
            yield return StartCoroutine(FadeManager.Instance.FadeIn());
        }
    }
}