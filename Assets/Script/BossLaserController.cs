using UnityEngine;

public class BossLaserController : MonoBehaviour
{
    public GameObject laserObject; // 레이저 프리팹이 아니라, Hierarchy에 붙어있는 자식 오브젝트

    void Start()
    {
        if (laserObject != null)
            laserObject.SetActive(false);
    }

    // Animation Event로 호출
    public void ShowLaser()
    {
        if (laserObject != null)
            laserObject.SetActive(true);
    }

    // Animation Event로 호출
    public void HideLaser()
    {
        if (laserObject != null)
            laserObject.SetActive(false);
    }
}
