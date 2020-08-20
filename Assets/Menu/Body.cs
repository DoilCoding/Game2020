using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Console = Assets.Modules.Console.Console;

namespace Assets.Menu
{
    public class Body : MonoBehaviour
    {
        private static Body singleton;
        //private void Awake() => singleton = this;
        private void Awake()
        {
            singleton = this;

            //TODO IMPORTANT: TEMPORARY MOVE THIS TO GAME MANAGER
            Configuration.RequestedPlayerSettings = Config.Load();
            Configuration.RequestedPlayerSettings.resolution = Screen.resolutions[Screen.resolutions.Length - 1].ToString();
            Configuration.RequestedPlayerSettings.Apply();
        }
        private void OnEnable() => Populate();
        private void OnDisable() =>
            Configuration.RequestedPlayerSettings = Configuration.CurrentPlayerSettings.Clone();

        //TODO: add the other variables
        private static void SetSetting(Transform target)
        {
            for (var i = 0; i < target.childCount; i++)
            {
                var self = target.GetChild(i);
                switch (self.name)
                {
                    case "Mouse Look Sensitivity":
                        SliderHandler(self, Configuration.CurrentPlayerSettings.mouseLookSensitivity, result =>
                            Configuration.RequestedPlayerSettings.mouseLookSensitivity = result);
                        break;
                    case "Invert Mouse":
                        ToggleHandler(self, result => Configuration.RequestedPlayerSettings.invertMouse = result);
                        break;
                    case "Shadows":
                        DropDownHandler(self, Configuration.CurrentPlayerSettings.ShadowQuality, result =>
                            Configuration.RequestedPlayerSettings.ShadowQuality = result);
                        break;
                    case "Texture Quality":
                        DropDownHandler(self, Configuration.CurrentPlayerSettings.textureQuality, result =>
                            Configuration.RequestedPlayerSettings.textureQuality = result);
                        break;
                    case "Anti Aliasing":
                        DropDownHandler(self, Configuration.CurrentPlayerSettings.antiAliasing, result =>
                            Configuration.RequestedPlayerSettings.antiAliasing = result);
                        break;
                    case "Anisotropic Filtering":
                        DropDownHandler(self, Configuration.CurrentPlayerSettings.anisotropicFiltering, result =>
                            Configuration.RequestedPlayerSettings.anisotropicFiltering = result);
                        break;
                    default:
                        Debug.LogError(new NotImplementedException($"{self.name} was not in the list"));
                        break;
                }
            }
        }

        public static void Populate()
        {
            SetSetting(singleton._generalGameObject.transform);
            SetSetting(singleton._graphicsGameObject.transform);
            SetSetting(singleton._audioGameObject.transform);
            SetSetting(singleton._inputGameObject.transform);
            SetSetting(singleton._keybindingsGameObject.transform);
        }
        private static void ToggleHandler(Transform self, Action<bool> callBackAction)
        {
            var _ = self.Find("Toggle").GetComponent<Toggle>();
            _.isOn = Configuration.CurrentPlayerSettings.invertMouse;
            _.onValueChanged.AddListener(result =>
                callBackAction(result));
        }
        private static void DropDownHandler<T>(Transform self, T current, Action<T> callBackAction) where T : struct
        {
            var drop = self.Find("Dropdown").GetComponent<Dropdown>();
            drop.options = Enum.GetNames(typeof(T)).ToList().Select(x => new Dropdown.OptionData(x)).ToList();
            drop.value = drop.options.IndexOf(drop.options.FirstOrDefault(x => x.text == current.ToString()));
            drop.onValueChanged.AddListener(changed =>
            {
                Enum.TryParse(Enum.GetName(typeof(T), changed), true, out T result);
                callBackAction(result);
            });
        }
        private static void SliderHandler(Transform self, float current, Action<float> callBackAction)
        {
            var slider = self.Find("Slider").GetComponent<Slider>();
            var input = self.Find("InputField").GetComponent<InputField>();
            slider.value = current;
            input.text = $"{current}";

            slider.onValueChanged.AddListener(result =>
            {
                result = Mathf.Clamp(result, slider.minValue, slider.maxValue);
                input.text = $"{result}";
                callBackAction(result);
            });

            input.onEndEdit.AddListener(change =>
            {
                if (float.TryParse(change, out var result))
                {
                    result = Mathf.Clamp(result, slider.minValue, slider.maxValue);
                    slider.value = result;
                    callBackAction(result);
                    input.text = $"{result}";
                }
                else
                    input.text = $"{slider.value}";
            });
        }

#pragma warning disable 649
        [SerializeField]
        private GameObject _generalGameObject,
            _graphicsGameObject,
            _audioGameObject,
            _inputGameObject,
            _keybindingsGameObject;
#pragma warning restore 649
    }
}
