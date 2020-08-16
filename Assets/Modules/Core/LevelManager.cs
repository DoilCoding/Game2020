using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Console = Assets.Modules.Console.Console;

public class LevelManager : MonoBehaviour
{
    private void Awake()
    {
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        singleton = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnApplicationQuit()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.Equals(scene.name, "splashscreen", System.StringComparison.OrdinalIgnoreCase))
        {
            //await Task.Delay(2000);
            await Task.Delay(1);
            await LoadNewScene("mainscreen");
        }
    }

    private CancellationTokenSource cts;

    private async Task LoadNewScene(string sceneName)
    {
        if (cts == null)
        {
            cts = new CancellationTokenSource();
            try
            {
                await PerformSceneLoading(sceneName, cts.Token);
            }
            catch (OperationCanceledException ex)
            {
                if (ex.CancellationToken == cts.Token)
                {
                    Console.Log("CTS token was canceled");
                }
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
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        token.ThrowIfCancellationRequested();
        if (token.IsCancellationRequested)
            return;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
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
        cts.Cancel();
        token.ThrowIfCancellationRequested();
        if (token.IsCancellationRequested)
            return;

    }

    public static LevelManager singleton { get; private set; }
}
