using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class DungeonManager
{
    // 클리어된 씬(방)의 이름을 저장하는 리스트
    public static HashSet<string> clearedRooms = new HashSet<string>();

    public static void MarkRoomAsCleared()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (!clearedRooms.Contains(currentScene))
        {
            clearedRooms.Add(currentScene);
        }
    }

    public static bool IsRoomCleared()
    {
        return clearedRooms.Contains(SceneManager.GetActiveScene().name);
    }
}