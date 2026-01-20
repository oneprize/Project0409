using UnityEngine;

public class DungeonDoor : MonoBehaviour
{
    [SerializeField] private GameObject doorVisual;
    [SerializeField] private Collider2D doorCollider;
    
    void Start()
    {
        CloseDoor();
    }


    public void OpenDoor()
    {
        doorVisual.SetActive(false);
        doorCollider.enabled = false;
    }

    public void CloseDoor()
    {
        doorVisual.SetActive(true);
        doorCollider.enabled = true;
    }
}
