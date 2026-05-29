using System;
using System.Collections.Generic;

// Bỏ kế thừa MonoBehaviour vì đây là class tiện ích dạng Static
public static class EventManager
{
    private static Dictionary<string, Action> eventRegister = new Dictionary<string, Action>();

    // 1. Đăng ký sự kiện
    public static void Register(string eventName, Action callback)
    {
        if (eventRegister.ContainsKey(eventName))
        {
            eventRegister[eventName] += callback;
        }
        else
        {
            eventRegister.Add(eventName, callback);
        }
    }

    // 2. Hủy đăng ký
    public static void Unregister(string eventName, Action callback)
    {
        if (eventRegister.ContainsKey(eventName))
        {
            eventRegister[eventName] -= callback;

            if (eventRegister[eventName] == null)
            {
                eventRegister.Remove(eventName);
            }
        }
    }

    // 3. Phát sự kiện
    public static void Notify(string eventName)
    {
        if (eventRegister.TryGetValue(eventName, out Action callback))
        {
            callback?.Invoke();
        }
    }
}