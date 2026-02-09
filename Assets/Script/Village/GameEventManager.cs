using System.Collections.Generic;

public static class GameEventManager
{
    // 어떤 사건이 일어났는지 저장하는 바구니
    private static HashSet<string> completedEvents = new HashSet<string>();

    public static void CompleteEvent(string eventName) => completedEvents.Add(eventName);
    public static bool IsEventCompleted(string eventName) => string.IsNullOrEmpty(eventName) || completedEvents.Contains(eventName);
}