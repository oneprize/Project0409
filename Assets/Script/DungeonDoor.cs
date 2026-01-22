using UnityEngine;

public class DungeonDoor : MonoBehaviour
{   
    [SerializeField] private Collider2D doorCollider;
    private Animator animator;
    
    void Start()
    {
        CloseDoor();
        animator = GetComponent<Animator>();
    }


    public void OpenDoor()
    {
        animator.SetTrigger("Open");
        doorCollider.enabled = false;
    }

    public void CloseDoor()
    {
        doorCollider.enabled = true;
    }
}
