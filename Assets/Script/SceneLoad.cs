using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    // 인스펙터 창에서 이동할 씬 이름을 직접 적어주세요 (예: MainScene)
    [SerializeField] private string sceneName;

    public void MoveToScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}