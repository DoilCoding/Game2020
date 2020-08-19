using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using Object = UnityEngine.Object;

// obsolete, replace it with Body.Configuration
//TODO: get rid of this script (cant atm..)
[Obsolete("Replacing this with Body.Configuration", false)]
public class SettingsManager : MonoBehaviour
{
    public static Config CurrentPlayerConfiguration = new Config();
    public static Config RequestedPlayerConfiguration = new Config();

    private void Awake()
    {
        CurrentPlayerConfiguration = Config.Load();
        CurrentPlayerConfiguration.Initialize();
        RequestedPlayerConfiguration = CurrentPlayerConfiguration.Clone();
    }

    public static void ResetToDefaults()
    {
        Config.Delete();
        RequestedPlayerConfiguration = Config.Load();
        RequestedPlayerConfiguration.resolution = Screen.resolutions[Screen.resolutions.Length - 1].ToString();
        RequestedPlayerConfiguration.Apply();
        foreach (var obj in (SettingButtonBehaviour[])FindObjectsOfType(typeof(SettingButtonBehaviour)))
            obj.OnEnable();
    }
}