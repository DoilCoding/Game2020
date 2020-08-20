using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Console = Assets.Modules.Console.Console;

[Obsolete]
public class SettingButtonBehaviour : MonoBehaviour
{
    private void DropDownHandler<T>(T current, Action<T> handler) where T : struct
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

    private void SliderHandler(float current, Action<float> handler)
    {
        var slider = transform.Find("Slider").GetComponent<Slider>();
        var input = transform.Find("InputField").GetComponent<InputField>();
        slider.value = current;
        input.text = $"{current}";

        slider.onValueChanged.AddListener(result =>
        {
            result = Mathf.Clamp(result, slider.minValue, slider.maxValue);
            input.text = $"{result}";
            handler(result);
        });

        input.onEndEdit.AddListener(change =>
        {
            if (float.TryParse(change, out var result))
            {
                result = Mathf.Clamp(result, slider.minValue, slider.maxValue);
                slider.value = result;
                handler(result);
                input.text = $"{result}";
            }
            else
                input.text = $"{slider.value}";
        });
    }

    private void KeyBindingHandler()
    {
        var primary = transform.Find("Primary");
        var secondary = transform.Find("Secondary");

        primary.GetComponent<Button>().onClick.AddListener(() =>
            MenuOptions.singleton.ListenForInput(primary));
        secondary.GetComponent<Button>().onClick.AddListener(() =>
            MenuOptions.singleton.ListenForInput(secondary));
    }


    public void OnEnable()
    {
        switch (gameObject.name)
        {
            case "FullScreen-mode":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.fullScreenMode, result =>
                    SettingsManager.RequestedPlayerConfiguration.fullScreenMode = result);
                break;
            case "Resolution":
                var drop = transform.Find("Dropdown").GetComponent<Dropdown>();
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
                SliderHandler(SettingsManager.CurrentPlayerConfiguration.brightness, result =>
                    SettingsManager.RequestedPlayerConfiguration.brightness = result);
                break;
            case "Field of View":
                SliderHandler(SettingsManager.CurrentPlayerConfiguration.fieldOfView, result =>
                    SettingsManager.RequestedPlayerConfiguration.fieldOfView = Convert.ToInt32(result));
                break;
            case "Texture Quality":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.textureQuality, result =>
                    SettingsManager.RequestedPlayerConfiguration.textureQuality = result);
                break;
            case "Shadows":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.ShadowQuality, result =>
                    SettingsManager.RequestedPlayerConfiguration.ShadowQuality = result);
                break;
            case "Anti-Aliasing":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.antiAliasing, result =>
                    SettingsManager.RequestedPlayerConfiguration.antiAliasing = result);
                break;
            case "Anisotropic Filtering":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.anisotropicFiltering, result =>
                    SettingsManager.RequestedPlayerConfiguration.anisotropicFiltering = result);
                break;
            case "Device Mode":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.deviceMode, result =>
                    SettingsManager.RequestedPlayerConfiguration.deviceMode = result);
                break;
            case "Master Volume":
                SliderHandler(SettingsManager.CurrentPlayerConfiguration.masterVolume, result =>
                    SettingsManager.RequestedPlayerConfiguration.masterVolume = result);
                break;
            case "Effects Volume":
                SliderHandler(SettingsManager.CurrentPlayerConfiguration.effectsVolume, result =>
                    SettingsManager.RequestedPlayerConfiguration.effectsVolume = result);
                break;
            case "Music Volume":
                SliderHandler(SettingsManager.CurrentPlayerConfiguration.musicVolume, result =>
                    SettingsManager.RequestedPlayerConfiguration.musicVolume = result);
                break;
            case "Interface Volume":
                SliderHandler(SettingsManager.CurrentPlayerConfiguration.interfaceVolume, result =>
                    SettingsManager.RequestedPlayerConfiguration.interfaceVolume = result);
                break;
            case "Mouse Look Sensitivity":
                SliderHandler(SettingsManager.CurrentPlayerConfiguration.mouseLookSensitivity, result =>
                    SettingsManager.RequestedPlayerConfiguration.mouseLookSensitivity = result);
                break;
            case "Mouse Aim Sensitivity":
                SliderHandler(SettingsManager.CurrentPlayerConfiguration.mouseAimSensitivity, result =>
                    SettingsManager.RequestedPlayerConfiguration.mouseAimSensitivity = result);
                break;
            case "Invert Mouse":
                var _ = transform.Find("Toggle").GetComponent<Toggle>();
                _.isOn = SettingsManager.CurrentPlayerConfiguration.invertMouse;
                _.onValueChanged.AddListener(result =>
                    SettingsManager.RequestedPlayerConfiguration.invertMouse = result);
                break;
            case "Crouch Mode":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.crouchMode, result =>
                    SettingsManager.RequestedPlayerConfiguration.crouchMode = result);
                break;
            case "Sprint Mode":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.sprintMode, result =>
                    SettingsManager.RequestedPlayerConfiguration.sprintMode = result);
                break;
            case "Aim Mode":
                DropDownHandler(SettingsManager.CurrentPlayerConfiguration.aimMode, result =>
                    SettingsManager.RequestedPlayerConfiguration.aimMode = result);
                break;
            case "Forward":
                KeyBindingHandler();
                break;
            case "Left":
                KeyBindingHandler();
                break;
            case "Back":
                KeyBindingHandler();
                break;
            case "Right":
                KeyBindingHandler();
                break;
            case "Jump":
                KeyBindingHandler();
                break;
            case "Crouch":
                KeyBindingHandler();
                break;
            default:
                Console.Log($"<color=red>Method not implemented for '{gameObject.name}'</color>");
                break;

        }
    }
}
