using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public List<DungeonDoor> doors;
    public List<GameObject> monsters;

    private bool isBattleActive = false;

    private void Awake()
    {
        // 1. 이미 클리어된 방인지 확인
        if (DungeonManager.IsRoomCleared())
        {
            // 이미 클리어된 방이라면 모든 몬스터를 즉시 파괴
            foreach (GameObject monster in monsters)
            {
                if (monster != null) Destroy(monster);
            }
            Debug.Log("클리어된 방이므로 몬스터를 소환하지 않습니다.");
        }
    }

    void Start()
    {
        StartBattle();
    }

    void StartBattle()
    {
        isBattleActive = true;
        foreach (var door in doors)
        {
            door.CloseDoor();
            Debug.Log("StartBattle");
        }
    }

    void Update()
    {
        if (isBattleActive)
        {
            monsters.RemoveAll(monster => monster == null);

            if (monsters.Count == 0)
            {
                EndBattle();
                Debug.Log("EndBattle Open");
            }
        }

        // 2. 실시간으로 몬스터가 다 죽었는지 체크
        if (!DungeonManager.IsRoomCleared() && monsters.Count > 0)
        {
            // 리스트에서 이미 파괴된(null) 몬스터 제거
            monsters.RemoveAll(m => m == null);

            // 모든 몬스터가 제거되었다면 클리어 기록
            if (monsters.Count == 0)
            {
                DungeonManager.MarkRoomAsCleared();
                Debug.Log("방 클리어! 다음 방문 시 몬스터가 나오지 않습니다.");
            }
        }
    }

    void EndBattle()
    {
        isBattleActive = false;
        foreach (var door in doors)
        {
            door.OpenDoor();
        }
        Debug.Log("방 클리어");
    }
}
