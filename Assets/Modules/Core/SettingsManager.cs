using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using Object = UnityEngine.Object;

public class SettingsManager : MonoBehaviour
{
    public static Config CurrentPlayerConfiguration = new Config();
    public static Config RequestedPlayerConfiguration = new Config();
    public static SettingsManager singleton { get; private set; }

    private void Awake()
    {
        singleton = this;
        CurrentPlayerConfiguration = Config.Load();
        CurrentPlayerConfiguration.Initialize();
        RequestedPlayerConfiguration = CurrentPlayerConfiguration.Clone();
    }

    public void ApplySettings() => RequestedPlayerConfiguration.Apply();

    public void ResetToDefaults()
    {
        Config.Delete();
        RequestedPlayerConfiguration = Config.Load();
        RequestedPlayerConfiguration.resolution = Screen.resolutions[Screen.resolutions.Length - 1].ToString();
        RequestedPlayerConfiguration.Apply();
        foreach (var obj in (SettingButtonBehaviour[])FindObjectsOfType(typeof(SettingButtonBehaviour)))
            obj.OnEnable();
    }
}