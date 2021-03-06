﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Console = Assets.Modules.Console;

public class LevelManager : MonoBehaviour
{
    private void Awake()
    {
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        singleton = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnApplicationQuit()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        await Task.Delay(0);
    }

    private CancellationTokenSource cts;

    public async Task LoadNewScene(string sceneName)
    {
        if (cts == null)
        {
            cts = new CancellationTokenSource();
            try
            {
                await PerformSceneLoading(sceneName, cts.Token);
            }
            finally
            {
                cts.Cancel();
                cts = null;
            }
        }
        else
        {
            cts.Cancel();
            cts = null;
        }
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    private async Task PerformSceneLoading(string sceneName, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        if (token.IsCancellationRequested)
            return;

        var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while (true)
        {
            token.ThrowIfCancellationRequested();
            if (token.IsCancellationRequested)
                return;
            if (asyncOperation.progress >= 0.9f)
                break;
        }
        asyncOperation.allowSceneActivation = true;
        Console.Log($"{sceneName} has been loaded.");
        cts.Cancel();
        //token.ThrowIfCancellationRequested();
    }

    public static LevelManager singleton { get; private set; }
}
