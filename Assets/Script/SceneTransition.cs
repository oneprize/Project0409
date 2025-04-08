using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneTransition : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("드래그할 씬 (유니티 에디터 전용)")]
    public SceneAsset sceneAsset;
#endif

    [SerializeField] private string sceneName;
    [SerializeField] private Transform destinationTransform; // 현재 씬 안에서만 참조됨

    private static Vector3 nextPosition = Vector3.zero;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 다음 씬에서 사용할 위치를 미리 저장
            if (destinationTransform != null)
                nextPosition = destinationTransform.position;

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = nextPosition;
        }

        // 위치 초기화 (다음 이동에 영향 안 주게)
        nextPosition = Vector3.zero;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
    }
#endif
}
