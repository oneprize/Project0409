using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public List<DungeonDoor> doors;
    public List<GameObject> monsters;

    private bool isBattleActive = false;

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
