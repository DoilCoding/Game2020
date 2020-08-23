using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[assembly: System.Reflection.AssemblyVersion("1.0.*")]
public class debuggingGameVersionHandler : MonoBehaviour
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
        self.text = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
    }
}
