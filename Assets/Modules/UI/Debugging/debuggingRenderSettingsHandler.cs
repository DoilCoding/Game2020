using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class debuggingRenderSettingsHandler : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject.transform.parent.parent.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnApplicationQuit()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var self = transform.GetComponent<Text>();
        self.text = string.Empty;
        self.text += $"Scene: {scene.name} ";
        self.text += $"Resolution: {Screen.currentResolution.width} x {Screen.currentResolution.height} @ {Screen.currentResolution.refreshRate}Hz ";
        self.text += $"Screen-mode: {Screen.fullScreenMode} ";
        lastResolution = Screen.currentResolution;
        lastScene = scene;
        lastFullScreen = Screen.fullScreenMode;
    }

    private Resolution lastResolution;
    private Scene lastScene;
    private FullScreenMode lastFullScreen;

    private void FixedUpdate()
    {
        if (lastResolution.ToString() != Screen.currentResolution.ToString() || lastFullScreen != Screen.fullScreenMode)
        {
            OnSceneLoaded(lastScene, LoadSceneMode.Single);
        }
    }
}
