using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger 
{
    public static void DebugLog(string Message)
    {
        #if UNITY_EDITOR
            Debug.Log(Message);
        #endif
    }
}
