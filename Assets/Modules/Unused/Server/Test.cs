using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        Server.OnServerShutDownTimer += OnServerShutDownTimerHandler;
    }

    private void OnServerShutDownTimerHandler(int minutes, int seconds)
    {
        Debug.Log($"Log: Server shutdown in {minutes} minutes & {seconds} seconds.");
    }

    private void OnApplicationQuit()
    {
        Server.OnServerShutDownTimer -= OnServerShutDownTimerHandler;
    }
}
