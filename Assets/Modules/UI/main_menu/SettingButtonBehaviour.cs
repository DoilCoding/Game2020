using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Console = Assets.Modules.Console.Console;
using Object = System.Object;


public class SettingButtonBehaviour : MonoBehaviour
{
    private void Awake()
    {
        Set();
    }

    private void DropDownHandler<T>(T current, [NotNull] Action<T> handler) where T : struct
    {
        var drop = transform.Find("Dropdown").GetComponent<Dropdown>();
        drop.options = Enum.GetNames(typeof(T)).ToList().Select(x => new Dropdown.OptionData(x)).ToList();
        drop.value = drop.options.IndexOf(drop.options.FirstOrDefault(x => x.text == current.ToString()));
        drop.onValueChanged.AddListener(changed =>
        {
            Enum.TryParse(Enum.GetName(typeof(T), changed), true, out T result);
            handler(result);
        });
    }

    public void Set()
    {
        Dropdown drop = null;
        Slider slider = null;
        InputField input = null;
        switch (gameObject.name)
        {
            #region Graphics
            case "FullScreen-mode":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.fullScreenMode, result =>
                    SettingsManager.RequestedPlayerConfiguration.fullScreenMode = result);
                break;
            case "Resolution":
                drop = transform.Find("Dropdown").GetComponent<Dropdown>();
                var refreshRate = Screen.currentResolution.refreshRate;
                drop.options = Screen.resolutions.Where(x => x.refreshRate == refreshRate).Select(x => new Dropdown.OptionData($"{x.width} x {x.height}")).ToList();
                drop.value = drop.options.IndexOf(drop.options.FirstOrDefault(x => $"{x.text} @ {refreshRate}Hz" == SettingsManager.CurrentPlayerConfiguration.resolution));

                drop.onValueChanged.AddListener(changed =>
                {
                    SettingsManager.RequestedPlayerConfiguration.resolution =
                        Screen.resolutions.FirstOrDefault(x => $"{drop.options[drop.value].text} @ {refreshRate}Hz" == $"{x.width} x {x.height} @ {x.refreshRate}Hz").ToString();
                });
                break;
            case "Brightness":
                slider = transform.Find("Slider").GetComponent<Slider>();
                input = transform.Find("InputField").GetComponent<InputField>();
                slider.value = SettingsManager.CurrentPlayerConfiguration.brightness;
                input.text = $"{SettingsManager.CurrentPlayerConfiguration.brightness}";

                slider.onValueChanged.AddListener(change =>
                {
                    change = Mathf.Clamp(change, slider.minValue, slider.maxValue);
                    input.text = $"{change}";
                    SettingsManager.RequestedPlayerConfiguration.brightness = change;
                });

                input.onEndEdit.AddListener(change =>
                {
                    if (float.TryParse(change, out var result))
                    {
                        result = Mathf.Clamp(result, slider.minValue, slider.maxValue);
                        slider.value = result;
                        SettingsManager.RequestedPlayerConfiguration.brightness = result;
                        input.text = $"{result}";
                    }
                    else
                        input.text = $"{slider.value}";
                });
                break;
            case "Field of View":
                slider = transform.Find("Slider").GetComponent<Slider>();
                input = transform.Find("InputField").GetComponent<InputField>();
                slider.value = SettingsManager.CurrentPlayerConfiguration.fieldOfView;
                input.text = SettingsManager.CurrentPlayerConfiguration.fieldOfView.ToString();

                slider.onValueChanged.AddListener(change =>
                {
                    change = Mathf.Clamp(change, slider.minValue, slider.maxValue);
                    input.text = $"{change}";
                    SettingsManager.RequestedPlayerConfiguration.fieldOfView = Convert.ToInt32(change);
                });

                input.onEndEdit.AddListener(change =>
                {
                    if (float.TryParse(change, out var result))
                    {
                        result = Mathf.Clamp(result, slider.minValue, slider.maxValue);
                        slider.value = result;
                        SettingsManager.RequestedPlayerConfiguration.fieldOfView = Convert.ToInt32(result);
                        input.text = $"{result}";
                    }
                    else
                        input.text = $"{slider.value}";
                });
                break;
            case "Texture Quality":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.textureQuality, result =>
                    SettingsManager.RequestedPlayerConfiguration.textureQuality = result);
                break;
            case "Shadows":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.shadowquality, result =>
                    SettingsManager.RequestedPlayerConfiguration.shadowquality = result);
                break;
            case "Anti-Aliasing":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.antiAliasing, result =>
                    SettingsManager.RequestedPlayerConfiguration.antiAliasing = result);
                break;
            case "Anisotropic Filtering":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.anisotropicFiltering, result =>
                    SettingsManager.RequestedPlayerConfiguration.anisotropicFiltering = result);
                break;
            #endregion

            #region Audio
            case "Device Mode":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.deviceMode, result =>
                    SettingsManager.RequestedPlayerConfiguration.deviceMode = result);
                break;
            case "Master Volume":
                slider = transform.Find("Slider").GetComponent<Slider>();
                input = transform.Find("InputField").GetComponent<InputField>();
                slider.value = SettingsManager.CurrentPlayerConfiguration.masterVolume;
                input.text = SettingsManager.CurrentPlayerConfiguration.masterVolume.ToString();

                slider.onValueChanged.AddListener(change =>
                {
                    change = Mathf.Clamp(change, slider.minValue, slider.maxValue);
                    input.text = $"{change}";
                    SettingsManager.RequestedPlayerConfiguration.masterVolume = Convert.ToInt32(change);
                });

                input.onEndEdit.AddListener(change =>
                {
                    if (float.TryParse(change, out var result))
                    {
                        result = Mathf.Clamp(result, slider.minValue, slider.maxValue);
                        slider.value = result;
                        SettingsManager.RequestedPlayerConfiguration.masterVolume = Convert.ToInt32(result);
                        input.text = $"{result}";
                    }
                    else
                        input.text = $"{slider.value}";
                });
                break;
            case "Effects Volume":
                slider = transform.Find("Slider").GetComponent<Slider>();
                input = transform.Find("InputField").GetComponent<InputField>();
                slider.value = SettingsManager.CurrentPlayerConfiguration.effectsVolume;
                input.text = SettingsManager.CurrentPlayerConfiguration.effectsVolume.ToString();

                slider.onValueChanged.AddListener(change =>
                {
                    change = Mathf.Clamp(change, slider.minValue, slider.maxValue);
                    input.text = $"{change}";
                    SettingsManager.RequestedPlayerConfiguration.effectsVolume = Convert.ToInt32(change);
                });

                input.onEndEdit.AddListener(change =>
                {
                    if (float.TryParse(change, out var result))
                    {
                        result = Mathf.Clamp(result, slider.minValue, slider.maxValue);
                        slider.value = result;
                        SettingsManager.RequestedPlayerConfiguration.effectsVolume = Convert.ToInt32(result);
                        input.text = $"{result}";
                    }
                    else
                        input.text = $"{slider.value}";
                });
                break;
            case "Music Volume":
                slider = transform.Find("Slider").GetComponent<Slider>();
                input = transform.Find("InputField").GetComponent<InputField>();
                slider.value = SettingsManager.CurrentPlayerConfiguration.musicVolume;
                input.text = SettingsManager.CurrentPlayerConfiguration.musicVolume.ToString();

                slider.onValueChanged.AddListener(change =>
                {
                    change = Mathf.Clamp(change, slider.minValue, slider.maxValue);
                    input.text = $"{change}";
                    SettingsManager.RequestedPlayerConfiguration.musicVolume = Convert.ToInt32(change);
                });

                input.onEndEdit.AddListener(change =>
                {
                    if (float.TryParse(change, out var result))
                    {
                        result = Mathf.Clamp(result, slider.minValue, slider.maxValue);
                        slider.value = result;
                        SettingsManager.RequestedPlayerConfiguration.musicVolume = Convert.ToInt32(result);
                        input.text = $"{result}";
                    }
                    else
                        input.text = $"{slider.value}";
                });
                break;
            case "Interface Volume":
                slider = transform.Find("Slider").GetComponent<Slider>();
                input = transform.Find("InputField").GetComponent<InputField>();
                slider.value = SettingsManager.CurrentPlayerConfiguration.interfaceVolume;
                input.text = SettingsManager.CurrentPlayerConfiguration.interfaceVolume.ToString();

                slider.onValueChanged.AddListener(change =>
                {
                    change = Mathf.Clamp(change, slider.minValue, slider.maxValue);
                    input.text = $"{change}";
                    SettingsManager.RequestedPlayerConfiguration.interfaceVolume = Convert.ToInt32(change);
                });

                input.onEndEdit.AddListener(change =>
                {
                    if (float.TryParse(change, out var result))
                    {
                        result = Mathf.Clamp(result, slider.minValue, slider.maxValue);
                        slider.value = result;
                        SettingsManager.RequestedPlayerConfiguration.interfaceVolume = Convert.ToInt32(result);
                        input.text = $"{result}";
                    }
                    else
                        input.text = $"{slider.value}";
                });
                break;
            case "Mouse Look Sensitivity":

                break;
            case "Mouse Aim Sensitivity":

                break;
            case "Invert Mouse":

                break;
            case "Crouch Mode":

                break;
            case "Sprint Mode":

                break;
            case "Aim Mode":

                break;
            #endregion
            default:
                Console.Log($"<color=red>Method not implemented for '{gameObject.name}'</color>");
                break;

        }
    }
}
