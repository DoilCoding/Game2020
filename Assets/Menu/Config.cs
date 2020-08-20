using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Assets.Menu
{
    [Serializable]
    public class Config
    {
        public void Save()
        {
            FileStream file = null;
            try
            {
                file = File.Open(Application.persistentDataPath + "/save.cfg", FileMode.OpenOrCreate);
                var bf = new BinaryFormatter();
                bf.Serialize(file, this);
            }
            catch (Exception ex)
            {
                Debug.Log($"<color=red>Failed</color> to save Config at {Application.persistentDataPath + "/save.cfg"} {ex.Message} {ex.StackTrace}");
            }
            finally
            {
                file?.Close();
            }
        }

        public void DebugSettings() => DebugSettings(this);
        public static void DebugSettings(Config config)
        {
            Debug.Log($"<color=green>Loaded</color> Config Result:\n" +
                      "<color=purple>Graphics Settings</color>\n" +
                      $"FullscreenMode: {config.fullScreenMode}\n" +
                      $"Resolution: {config.resolution}\n" +
                      $"Brightness: {config.brightness}\n" +
                      $"Field of View: {config.fieldOfView}\n" +
                      $"Texture Quality: {config.textureQuality}\n" +
                      $"Shadow Quality: {config.ShadowQuality}\n" +
                      $"AntiAliasing: {config.antiAliasing}\n" +
                      $"AnisotropicFiltering: {config.anisotropicFiltering}\n" +
                      "<color=purple>Audio Settings</color>\n" +
                      $"Device Mode: {config.deviceMode}\n" +
                      $"Master Volume: {config.masterVolume}\n" +
                      $"Effects Volume: {config.effectsVolume}\n" +
                      $"Music Volume: {config.musicVolume}\n" +
                      $"Interface Volume {config.interfaceVolume}\n" +
                      "<color=purple>Input Settings</color>\n" +
                      $"Mouse Look Sensitivity: {config.mouseLookSensitivity}\n" +
                      $"Mouse Aim Sensitivity: {config.mouseAimSensitivity}\n" +
                      $"Invert Mouse: {config.invertMouse}\n" +
                      $"Crouch Mode: {config.crouchMode}\n" +
                      $"Sprint Mode: {config.sprintMode}\n" +
                      $"Aim Mode{config.aimMode}");
        }

        public static Config Load()
        {
            var result = new Config();
            FileStream file = null;
            Debug.Log($"Loading Config at {Application.persistentDataPath + "/save.cfg"}");
            try
            {
                file = File.Open(Application.persistentDataPath + "/save.cfg", FileMode.Open);
                if (file.Length != 0)
                {
                    var bf = new BinaryFormatter();
                    result = (Config)bf.Deserialize(file);
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(FileNotFoundException))
                {
                    result.Save();
                    Debug.Log($"<color=green>Created</color> new Config with default settings");
                }
                else
                {
                    Debug.Log($"<color=red>Failed</color> to open CurrentPlayerConfiguration: {ex.Message} {ex.StackTrace}");
                }
            }
            finally
            {
                file?.Close();
            }
            return result;
        }

        /// <summary>
        /// Normally called on RequestedPlayerSettings, we check the differences between this and CurrentPlayerSettings, apply the changes and set CurrentPlayerSettings to this.
        /// </summary>
        public void Apply()
        {
            #region Graphics
            if (Configuration.CurrentPlayerSettings == null || resolution != Configuration.CurrentPlayerSettings.resolution || fullScreenMode != Configuration.CurrentPlayerSettings.fullScreenMode)
            {
                var split = resolution.Split(new[] { "x", "@", "Hz" }, StringSplitOptions.RemoveEmptyEntries);
                var width = Convert.ToInt32(split[0]);
                var height = Convert.ToInt32(split[1]);
                var refresh = Convert.ToInt32(split[2]);
                Screen.SetResolution(width, height, fullScreenMode, refresh);
            }

            if (Configuration.CurrentPlayerSettings == null || brightness != Configuration.CurrentPlayerSettings.brightness)
                RenderSettings.ambientIntensity = brightness;

            if (Configuration.CurrentPlayerSettings == null || fieldOfView != Configuration.CurrentPlayerSettings.fieldOfView)
                Camera.main.fieldOfView = fieldOfView;

            if (Configuration.CurrentPlayerSettings == null || textureQuality != Configuration.CurrentPlayerSettings.textureQuality)
                QualitySettings.masterTextureLimit = (int)textureQuality;

            if (Configuration.CurrentPlayerSettings == null || ShadowQuality != Configuration.CurrentPlayerSettings.ShadowQuality)
                QualitySettings.shadows = ShadowQuality;

            if (Configuration.CurrentPlayerSettings == null || antiAliasing != Configuration.CurrentPlayerSettings.antiAliasing)
                QualitySettings.antiAliasing = (int)antiAliasing;

            if (Configuration.CurrentPlayerSettings == null || anisotropicFiltering != Configuration.CurrentPlayerSettings.anisotropicFiltering)
                QualitySettings.anisotropicFiltering = anisotropicFiltering;
            #endregion

            #region Audio
            var audioConfiguration = AudioSettings.GetConfiguration();

            if (Configuration.CurrentPlayerSettings == null || deviceMode != Configuration.CurrentPlayerSettings.deviceMode)
            {
                Enum.TryParse(deviceMode.ToString(), out AudioSpeakerMode result);
                audioConfiguration.speakerMode = result;
            }

            if (Configuration.CurrentPlayerSettings == null || masterVolume != Configuration.CurrentPlayerSettings.masterVolume)
                AudioListener.volume = masterVolume;

            //if (Configuration.CurrentPlayerSettings == null || effectsVolume != Configuration.CurrentPlayerSettings.effectsVolume)
            //    Debug.Log(new NotImplementedException());

            //if (Configuration.CurrentPlayerSettings == null || musicVolume != Configuration.CurrentPlayerSettings.musicVolume)
            //    Debug.Log(new NotImplementedException());

            //if (Configuration.CurrentPlayerSettings == null || interfaceVolume != Configuration.CurrentPlayerSettings.interfaceVolume)
            //    Debug.Log(new NotImplementedException());

            AudioSettings.Reset(audioConfiguration);
            #endregion

            if (Configuration.CurrentPlayerSettings != null)
            {
                Debug.Log($"Changes saved at {Application.persistentDataPath + "/save.cfg"}");
                Save();
            }

            Configuration.CurrentPlayerSettings = Clone();
            DebugSettings();
        }

        public Config Clone()
        {
            return new Config
            {
                fullScreenMode = this.fullScreenMode,
                resolution = this.resolution,
                brightness = this.brightness,
                fieldOfView = this.fieldOfView,
                textureQuality = this.textureQuality,
                ShadowQuality = this.ShadowQuality,
                antiAliasing = this.antiAliasing,
                anisotropicFiltering = this.anisotropicFiltering,
                deviceMode = this.deviceMode,
                masterVolume = this.masterVolume,
                effectsVolume = this.effectsVolume,
                musicVolume = this.musicVolume,
                interfaceVolume = this.interfaceVolume,
                mouseLookSensitivity = this.mouseLookSensitivity,
                mouseAimSensitivity = this.mouseAimSensitivity,
                invertMouse = this.invertMouse,
                crouchMode = this.crouchMode,
                sprintMode = this.sprintMode,
                aimMode = this.aimMode,
                Actions = this.Actions,
            };
        }

        public Dictionary<Keybinding.ActionType, Keybinding> Actions = new Dictionary<Keybinding.ActionType, Keybinding>
        {
            {Keybinding.ActionType.forward, new Keybinding {Primary = KeyCode.W, Secondary = KeyCode.UpArrow}},
            {Keybinding.ActionType.left, new Keybinding {Primary = KeyCode.A, Secondary = KeyCode.LeftArrow}},
            {Keybinding.ActionType.right, new Keybinding {Primary = KeyCode.D, Secondary = KeyCode.RightArrow}},
            {Keybinding.ActionType.back, new Keybinding {Primary = KeyCode.S, Secondary = KeyCode.DownArrow}},
            {Keybinding.ActionType.jump, new Keybinding {Primary = KeyCode.Space, Secondary = KeyCode.None}},
            {Keybinding.ActionType.crouch, new Keybinding {Primary = KeyCode.LeftControl, Secondary = KeyCode.C}},
            {Keybinding.ActionType.reload, new Keybinding {Primary = KeyCode.R, Secondary = KeyCode.None}},
        };

        #region Variables
        #region Graphics
        public FullScreenMode fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        public string resolution;
        public float brightness = 1;
        public int fieldOfView = 75;

        public TextureQuality textureQuality = TextureQuality.Full;
        public ShadowQuality ShadowQuality = ShadowQuality.All;
        public AntiAliasing antiAliasing = AntiAliasing.Eight;
        public AnisotropicFiltering anisotropicFiltering = AnisotropicFiltering.Enable;
        public enum TextureQuality { Eighth = 3, Quarter = 2, Half = 1, Full = 0 }
        public enum AntiAliasing { Eight = 8, Four = 4, Two = 2, Disabled = 0 }
        #endregion

        #region Audio
        public AudioDeviceMode deviceMode = AudioDeviceMode.Mode7point1;
        public float masterVolume = 100;
        public float effectsVolume = 50;
        public float musicVolume = 50;
        public float interfaceVolume = 50;
        public enum AudioDeviceMode { Mono, Stereo, Quad, Surround, Mode5point1, Mode7point1 }
        #endregion

        #region Input
        public float mouseLookSensitivity = 20;
        public float mouseAimSensitivity = 10;
        public bool invertMouse = false;
        public inputMode crouchMode = inputMode.Hold;
        public inputMode sprintMode = inputMode.Toggle;
        public inputMode aimMode = inputMode.Hold;
        public enum inputMode { Toggle, Hold }
        #endregion
        #endregion
    }
}