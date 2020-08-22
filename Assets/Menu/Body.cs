using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Menu
{
    public class Body : MonoBehaviour
    {
        private static Body _singleton;
        //private void Awake() => _singleton = this;
        // TODO: temporary
        private void Awake()
        {
            _singleton = this;
            Configuration.RequestedPlayerSettings = Config.Load();
            if (Configuration.RequestedPlayerSettings.resolution == string.Empty)
                Configuration.RequestedPlayerSettings.resolution = Screen.resolutions[Screen.resolutions.Length - 1].ToString();
            Configuration.RequestedPlayerSettings.Apply();
        }
        private void OnEnable() => Populate();
        private void OnDisable() => Configuration.RequestedPlayerSettings = Configuration.CurrentPlayerSettings.Clone();

        private void Populate(Transform target)
        {
            for (var i = 0; i < target.childCount; i++)
            {
                var self = target.GetChild(i);
                switch (self.name)
                {
                    #region General
                    case "FullScreen Mode":
                        DropDownHandler(self, Configuration.CurrentPlayerSettings.fullScreenMode, result =>
                            Configuration.RequestedPlayerSettings.fullScreenMode = result);
                        break;
                    case "Resolution":
                        var drop = self.Find("Dropdown").GetComponent<Dropdown>();
                        drop.options = Screen.resolutions.ToList().Select(x => new Dropdown.OptionData($"{x.width} x {x.height} @ {x.refreshRate}Hz")).ToList();
                        drop.value = drop.options.IndexOf(drop.options.FirstOrDefault(x => $"{x.text}" == Configuration.CurrentPlayerSettings.resolution));
                        drop.onValueChanged.RemoveAllListeners();
                        drop.onValueChanged.AddListener(changed =>
                        {
                            Configuration.RequestedPlayerSettings.resolution =
                                Screen.resolutions.FirstOrDefault(x => $"{drop.options[drop.value].text}" == $"{x.width} x {x.height} @ {x.refreshRate}Hz").ToString();
                        });
                        break;
                    case "Brightness":
                        SliderHandler(self, Configuration.CurrentPlayerSettings.brightness, result =>
                            Configuration.RequestedPlayerSettings.brightness = result);
                        break;
                    case "Field of View":
                        SliderHandler(self, Configuration.CurrentPlayerSettings.fieldOfView, result =>
                            Configuration.RequestedPlayerSettings.fieldOfView = Convert.ToInt32(result));
                        break;
                    case "Colorblind Mode":
                        Debug.Log(new NotImplementedException());
                        break;
                    #endregion

                    #region Graphics
                    case "Texture Quality":
                        DropDownHandler(self, Configuration.CurrentPlayerSettings.textureQuality, result =>
                            Configuration.RequestedPlayerSettings.textureQuality = result);
                        break;
                    case "Shadows":
                        DropDownHandler(self, Configuration.CurrentPlayerSettings.ShadowQuality, result =>
                            Configuration.RequestedPlayerSettings.ShadowQuality = result);
                        break;
                    case "Anti Aliasing":
                        DropDownHandler(self, Configuration.CurrentPlayerSettings.antiAliasing, result =>
                            Configuration.RequestedPlayerSettings.antiAliasing = result);
                        break;
                    case "Anisotropic Filtering":
                        DropDownHandler(self, Configuration.CurrentPlayerSettings.anisotropicFiltering, result =>
                            Configuration.RequestedPlayerSettings.anisotropicFiltering = result);
                        break;
                    #endregion

                    #region Audio
                    case "Device Mode":
                        DropDownHandler(self, Configuration.CurrentPlayerSettings.deviceMode, result =>
                            Configuration.RequestedPlayerSettings.deviceMode = result);
                        break;
                    case "Master Volume":
                        SliderHandler(self, Configuration.CurrentPlayerSettings.masterVolume, result =>
                            Configuration.RequestedPlayerSettings.masterVolume = result);
                        break;
                    case "Effects Volume":
                        SliderHandler(self, Configuration.CurrentPlayerSettings.effectsVolume, result =>
                            Configuration.RequestedPlayerSettings.effectsVolume = result);
                        break;
                    case "Music Volume":
                        SliderHandler(self, Configuration.CurrentPlayerSettings.musicVolume, result =>
                            Configuration.RequestedPlayerSettings.musicVolume = result);
                        break;
                    case "Interface Volume":
                        SliderHandler(self, Configuration.CurrentPlayerSettings.interfaceVolume, result =>
                            Configuration.RequestedPlayerSettings.interfaceVolume = result);
                        break;
                    #endregion

                    #region input
                    case "Mouse Look Sensitivity":
                        SliderHandler(self, Configuration.CurrentPlayerSettings.mouseLookSensitivity, result =>
                            Configuration.RequestedPlayerSettings.mouseLookSensitivity = result);
                        break;
                    case "Mouse Aim Sensitivity":
                        SliderHandler(self, Configuration.CurrentPlayerSettings.mouseAimSensitivity, result =>
                            Configuration.RequestedPlayerSettings.mouseAimSensitivity = result);
                        break;
                    case "Invert Mouse":
                        ToggleHandler(self, result => Configuration.RequestedPlayerSettings.invertMouse = result);
                        break;
                    case "Crouch Mode":
                        DropDownHandler(self, Configuration.CurrentPlayerSettings.crouchMode, result =>
                            Configuration.RequestedPlayerSettings.crouchMode = result);
                        break;
                    case "Sprint Mode":
                        DropDownHandler(self, Configuration.CurrentPlayerSettings.sprintMode, result =>
                            Configuration.RequestedPlayerSettings.sprintMode = result);
                        break;
                    case "Aim Mode":
                        DropDownHandler(self, Configuration.CurrentPlayerSettings.aimMode, result =>
                            Configuration.RequestedPlayerSettings.aimMode = result);
                        break;
                    #endregion

                    #region keybindings
                    #endregion




                    case "Forward":
                        RebindHandler(self);
                        break;
                    case "Left":
                        RebindHandler(self);
                        break;
                    case "Back":
                        RebindHandler(self);
                        break;
                    case "Right":
                        RebindHandler(self);
                        break;
                    case "Jump":
                        RebindHandler(self);
                        break;
                    case "Sprint":
                        RebindHandler(self);
                        break;
                    case "Crouch":
                        RebindHandler(self);
                        break;
                    default:
                        Debug.LogError(new NotImplementedException($"{self.name} was not in the list"));
                        break;
                }
            }
        }

        public static void Populate()
        {
            _singleton.Populate(_singleton._generalGameObject.transform);
            _singleton.Populate(_singleton._graphicsGameObject.transform);
            _singleton.Populate(_singleton._audioGameObject.transform);
            _singleton.Populate(_singleton._inputGameObject.transform);
            _singleton.Populate(_singleton._keybindingsGameObject.transform);
        }
        private static void ToggleHandler(Transform self, Action<bool> callBackAction)
        {
            var _ = self.Find("Toggle").GetComponent<Toggle>();
            _.isOn = Configuration.CurrentPlayerSettings.invertMouse;
            _.onValueChanged.RemoveAllListeners();
            _.onValueChanged.AddListener(result =>
                callBackAction(result));
        }
        private static void DropDownHandler<T>(Transform self, T current, Action<T> callBackAction) where T : struct
        {
            var _ = self.Find("Dropdown").GetComponent<Dropdown>();
            _.options = Enum.GetNames(typeof(T)).ToList().Select(x => new Dropdown.OptionData(x)).ToList();
            _.value = _.options.IndexOf(_.options.FirstOrDefault(x => x.text == current.ToString()));
            _.onValueChanged.RemoveAllListeners();
            _.onValueChanged.AddListener(changed =>
            {
                Enum.TryParse(Enum.GetName(typeof(T), changed), true, out T result);
                callBackAction(result);
            });
        }
        private static void SliderHandler(Transform self, float current, Action<float> callBackAction)
        {
            var _ = self.Find("Slider").GetComponent<Slider>();
            var input = self.Find("InputField").GetComponent<InputField>();
            _.value = current;
            input.text = $"{current}";

            _.onValueChanged.RemoveAllListeners();
            _.onValueChanged.AddListener(result =>
            {
                result = Mathf.Clamp(result, _.minValue, _.maxValue);
                input.text = $"{result}";
                callBackAction(result);
            });

            input.onEndEdit.RemoveAllListeners();
            input.onEndEdit.AddListener(change =>
            {
                if (float.TryParse(change, out var result))
                {
                    result = Mathf.Clamp(result, _.minValue, _.maxValue);
                    _.value = result;
                    callBackAction(result);
                    input.text = $"{result}";
                }
                else
                    input.text = $"{_.value}";
            });
        }

        //TODO: popup screen
        private void RebindHandler(Transform self)
        {
            var primary = self.Find("Primary").GetComponent<Button>();
            var secondary = self.Find("Secondary").GetComponent<Button>();

            if (!Enum.TryParse(self.name, true, out Keybinding.ActionType result))
                throw new ArgumentException(self.name);

            if (!InputManager.Actions.ContainsKey(result))
                throw new ArgumentException(self.name);

            primary.transform.Find("Text").GetComponent<Text>().text = $"{InputManager.Actions[result].Primary}";
            secondary.transform.Find("Text").GetComponent<Text>().text = $"{InputManager.Actions[result].Secondary}";

            primary.onClick.RemoveAllListeners();
            primary.onClick.AddListener(() =>
            {
                // open menu
                StartCoroutine(Configuration.ListenForInputHandler(primary.transform));
            });

            secondary.onClick.RemoveAllListeners();
            secondary.onClick.AddListener(() =>
            {
                // open menu
                StartCoroutine(Configuration.ListenForInputHandler(secondary.transform));
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
