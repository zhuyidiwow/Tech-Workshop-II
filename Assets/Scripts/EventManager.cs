using System;
using System.Collections.Generic;

public enum BVWEvent {
    StartGame,
    ResetGame,
}

public static class EventManager {
    
    private static Dictionary<BVWEvent, Action> eventDic = new Dictionary<BVWEvent, Action>();

    public static void StartListening(BVWEvent bvwEvent, Action action) {
        if (eventDic.ContainsKey(bvwEvent)) {
            eventDic[bvwEvent] += action;
        } else {
            eventDic[bvwEvent] = action;
        }
    }

    public static void StopListening(BVWEvent bvwEvent, Action action) {
        if (eventDic.ContainsKey(bvwEvent)) {
            eventDic[bvwEvent] -= action;
        }
    }

    public static void TriggerEvent(BVWEvent bvwEvent) {
        if (eventDic.ContainsKey(bvwEvent)) {
            eventDic[bvwEvent]();
        }
    }
}