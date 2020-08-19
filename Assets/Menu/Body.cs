using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Menu
{
    public class Body : MonoBehaviour
    {
        private static Body singleton;
        private void Awake() => singleton = this;
        private void OnEnable() => PopulateMenu();
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
                    default:
                        Debug.LogError(new NotImplementedException($"{self.name} was not in the list"));
                        break;
                }
            }
        }

        public static void PopulateMenu()
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


    // replace SettingManager with this one
    public static class Configuration
    {
        private static Config _currentPlayerSettings;
        public static Config CurrentPlayerSettings => // ReSharper disable once ConvertToNullCoalescingCompoundAssignment
            _currentPlayerSettings ?? (_currentPlayerSettings = Config.Load());

        private static Config _requestedPlayerSettings;
        public static Config RequestedPlayerSettings { // ReSharper disable once ConvertToNullCoalescingCompoundAssignment
            get => _requestedPlayerSettings ?? (_requestedPlayerSettings = CurrentPlayerSettings.Clone());
            set => _requestedPlayerSettings = value;
        }

        public static void ResetToDefaults()
        {
            RequestedPlayerSettings = new Config {
                resolution = Screen.resolutions[Screen.resolutions.Length - 1].ToString()};
            RequestedPlayerSettings.Apply();
            Body.PopulateMenu();
        }
    }
}
