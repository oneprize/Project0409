using UnityEngine;

public static class ItemManager
{
    public static void AddItem(string id, int count)
    {
        Debug.Log($"아이템 획득: {id} x {count}개");
        // 실제 인벤토리 리스트에 추가하는 로직이 여기 들어갑니다.
    }
}