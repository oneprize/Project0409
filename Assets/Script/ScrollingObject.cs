using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    public float speed = 10f;

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
