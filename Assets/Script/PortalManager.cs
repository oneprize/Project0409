using System.Collections.Generic;
using UnityEngine;

public static class PortalManager
{
    private static Dictionary<int, Vector3> portalMap = new Dictionary<int, Vector3>();
    private static int pendingTargetID = -1;
    private static Vector3? pendingCustomPosition = null;

    // 1. 다음 목적지가 ID 기반인지 설정
    public static void SetTargetID(int id)
    {
        pendingTargetID = id;
        pendingCustomPosition = null;
        portalMap.Clear(); // 씬 이동 시 이전 씬 포탈 데이터는 비워줌
    }

    // 2. 다음 목적지가 특정 좌표 기반인지 설정 (보스룸 등)
    public static void SetTargetPosition(Vector3 pos)
    {
        pendingCustomPosition = pos;
        pendingTargetID = -1;
        portalMap.Clear();
    }

    // 3. 씬이 로드될 때 각 포탈들이 호출하여 자신의 위치를 알림
    public static void RegisterPortal(int id, Vector3 position)
    {
        portalMap[id] = position;

        // 대기 중인 이동 명령이 있는지 확인
        if (id == pendingTargetID)
        {
            MovePlayer(position);
            pendingTargetID = -1;
        }
        else if (pendingCustomPosition.HasValue)
        {
            MovePlayer(pendingCustomPosition.Value);
            pendingCustomPosition = null;
        }
    }

    private static void MovePlayer(Vector3 pos)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = pos;
        }
    }
}