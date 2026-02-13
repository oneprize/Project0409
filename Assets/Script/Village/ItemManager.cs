using Unity.VisualScripting;
using UnityEngine;

public static class ItemManager
{
    public static void AddItem(GameObject itemPrefab, int count,Vector3 spawnPosition)
    {
        Debug.Log($"아이템 획득: {itemPrefab} x {count}개");
        // 실제 인벤토리 리스트에 추가하는 로직이 여기 들어갑니다.
        if (itemPrefab == null)
        {
            Debug.Log("프리팹이 설정되지 않았습니다");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            // 1. 아이템 생성
            Vector3 finalPos = spawnPosition + new Vector3(0, 0.5f, 0);
            GameObject droppedItem = Object.Instantiate(itemPrefab, finalPos, Quaternion.identity);

            Debug.Log($"아이템 생성됨{itemPrefab.name}");

            Rigidbody2D rb = droppedItem.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 launchDirection = new Vector2(Random.Range(-1.5f, 1.5f), 3f);
                rb.AddForce(launchDirection, ForceMode2D.Impulse);
            }
        }
    }
}