using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLog
{
    public static void Log(object message)
    {
#if UNITY_EDITOR || Debug
        Debug.Log(message);
#endif
    }
    public static void LogWarning(object message)
    {
#if UNITY_EDITOR || Debug
        Debug.LogWarning(message);
#endif
    }
    public static void LogError(object message)
    {
#if UNITY_EDITOR || Debug
        Debug.LogError(message);
#endif
    }
}
