using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Console = Assets.Modules.Console.Console;

[Serializable]
public class Config : IEquatable<Config>
{
    public bool Equals(Config other)
    {
        return
             other.fullScreenMode == fullScreenMode &&
             other.resolution == resolution &&
             other.brightness == brightness &&
             other.fieldOfView == fieldOfView &&
             other.textureQuality == textureQuality &&
             other.ShadowQuality == ShadowQuality &&
             other.antiAliasing == antiAliasing &&
             other.anisotropicFiltering == anisotropicFiltering &&
             other.deviceMode == deviceMode &&
             other.masterVolume == masterVolume &&
             other.effectsVolume == effectsVolume &&
             other.musicVolume == musicVolume &&
             other.interfaceVolume == interfaceVolume &&
             other.mouseLookSensitivity == mouseLookSensitivity &&
             other.mouseAimSensitivity == mouseAimSensitivity &&
             other.invertMouse == invertMouse &&
             other.crouchMode == crouchMode &&
             other.sprintMode == sprintMode &&
             other.aimMode == aimMode
            ;
    }

    #region Graphics
    public FullScreenMode fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    public string resolution;
    public float brightness = 1;
    public int fieldOfView = 75;

    public TextureQuality textureQuality = TextureQuality.Full;
    public ShadowQuality ShadowQuality = ShadowQuality.All;
    public AntiAliasing antiAliasing = AntiAliasing.Eight;
    public AnisotropicFiltering anisotropicFiltering = AnisotropicFiltering.Enable;
    public enum TextureQuality { Full, Half, Quarter, Eighth }
    public enum AntiAliasing { Disabled, Two, Four, Eight }
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

    //TODO: remove this, we don't need to delete it.
    public static void Delete()
    {
        if (!File.Exists(Application.persistentDataPath + "/save.cfg")) return;
        File.Delete(Application.persistentDataPath + "/save.cfg");
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
        };
    }

    public void Initialize()
    {
        #region Graphics
        // Resolution & FullScreen Mode
        if (string.IsNullOrEmpty(SettingsManager.CurrentPlayerConfiguration.resolution))
        {
            SettingsManager.CurrentPlayerConfiguration.resolution = Screen.currentResolution.ToString();
            SettingsManager.RequestedPlayerConfiguration.resolution = Screen.currentResolution.ToString();
        }

        var split =
            SettingsManager.CurrentPlayerConfiguration.resolution.Split(new string[] { "x", "@", "Hz" },
                StringSplitOptions.RemoveEmptyEntries);
        var width = Convert.ToInt32(split[0]);
        var height = Convert.ToInt32(split[1]);
        var refresh = Convert.ToInt32(split[2]);
        Screen.SetResolution(width, height, SettingsManager.CurrentPlayerConfiguration.fullScreenMode, refresh);

        // Brightness
        Screen.brightness = SettingsManager.CurrentPlayerConfiguration.brightness;

        // Field of View
        Camera.main.fieldOfView = SettingsManager.CurrentPlayerConfiguration.fieldOfView;

        // Texture Quality
        QualitySettings.masterTextureLimit = (int)SettingsManager.CurrentPlayerConfiguration.textureQuality;

        // Shadow Quality
        QualitySettings.shadows = SettingsManager.CurrentPlayerConfiguration.ShadowQuality;

        // Anti Aliasing
        QualitySettings.antiAliasing = (int)SettingsManager.CurrentPlayerConfiguration.antiAliasing;

        // AnisotropicFiltering
        QualitySettings.anisotropicFiltering = SettingsManager.CurrentPlayerConfiguration.anisotropicFiltering;
        #endregion

        #region Audio

        //DeviceMode
        var audioConfiguration = AudioSettings.GetConfiguration();
        Enum.TryParse(SettingsManager.CurrentPlayerConfiguration.deviceMode.ToString(), out AudioSpeakerMode result);
        audioConfiguration.speakerMode = result;

        //MasterVolume
        AudioListener.volume = SettingsManager.CurrentPlayerConfiguration.masterVolume * 0.01f;

        AudioSettings.Reset(audioConfiguration);
        #endregion
    }

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
            Console.Log($"<color=red>Failed</color> to save Config at {Application.persistentDataPath + "/save.cfg"} {ex.Message} {ex.StackTrace}");
        }
        finally
        {
            file?.Close();
        }
    }

    public static Config Load()
    {
        var result = new Config();
        FileStream file = null;
        Console.Log($"Loading Config at {Application.persistentDataPath + "/save.cfg"}");
        try
        {
            file = File.Open(Application.persistentDataPath + "/save.cfg", FileMode.Open);
            if (file.Length != 0)
            {
                var bf = new BinaryFormatter();
                result = (Config)bf.Deserialize(file);
                Console.Log($"<color=green>Loaded</color> Config Result:\n" +
                            "<color=purple>Graphics Settings</color>\n" +
                            $"FullscreenMode: {result.fullScreenMode}\n" +
                            $"Resolution: {result.resolution}\n" +
                            $"Brightness: {result.brightness}\n" +
                            $"Field of View: {result.fieldOfView}\n" +
                            $"Texture Quality: {result.textureQuality}\n" +
                            $"Shadow Quality: {result.ShadowQuality}\n" +
                            $"AntiAliasing: {result.antiAliasing}\n" +
                            $"AnisotropicFiltering: {result.anisotropicFiltering}\n" +
                            "<color=purple>Audio Settings</color>\n" +
                            $"Device Mode: {result.deviceMode}\n" +
                            $"Master Volume: {result.masterVolume}\n" +
                            $"Effects Volume: {result.effectsVolume}\n" +
                            $"Music Volume: {result.musicVolume}\n" +
                            $"Interface Volume {result.interfaceVolume}\n" +
                            "<color=purple>Input Settings</color>\n" +
                            $"Mouse Look Sensitivity: {result.mouseLookSensitivity}\n" +
                            $"Mouse Aim Sensitivity: {result.mouseAimSensitivity}\n" +
                            $"Invert Mouse: {result.invertMouse}\n" +
                            $"Crouch Mode: {result.crouchMode}\n" +
                            $"Sprint Mode: {result.sprintMode}\n" +
                            $"Aim Mode{result.aimMode}");
            }
        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(FileNotFoundException))
            {
                result.Save();
                Console.Log($"<color=green>Created</color> new Config with default settings");
            }
            else
            {
                Console.Log($"<color=red>Failed</color> to open CurrentPlayerConfiguration: {ex.Message} {ex.StackTrace}");
            }
        }
        finally
        {
            file?.Close();
        }
        return result;
    }

    //TODO: do we need Apply AND Init ? seems kinda.. double? no?
    public void Apply()
    {
        #region Graphics
        if (resolution != SettingsManager.CurrentPlayerConfiguration.resolution || fullScreenMode != SettingsManager.CurrentPlayerConfiguration.fullScreenMode)
        {
            var split = resolution.Split(new[] { "x", "@", "Hz" }, StringSplitOptions.RemoveEmptyEntries);
            var width = Convert.ToInt32(split[0]);
            var height = Convert.ToInt32(split[1]);
            var refresh = Convert.ToInt32(split[2]);
            Screen.SetResolution(width, height, fullScreenMode, refresh);
        }

        if (brightness != SettingsManager.CurrentPlayerConfiguration.brightness)
            RenderSettings.ambientIntensity = brightness;

        if (fieldOfView != SettingsManager.CurrentPlayerConfiguration.fieldOfView)
            Camera.main.fieldOfView = fieldOfView;

        if (textureQuality != SettingsManager.CurrentPlayerConfiguration.textureQuality)
            QualitySettings.masterTextureLimit = (int)textureQuality;

        if (ShadowQuality != SettingsManager.CurrentPlayerConfiguration.ShadowQuality)
            QualitySettings.shadows = ShadowQuality;

        if (antiAliasing != SettingsManager.CurrentPlayerConfiguration.antiAliasing)
            QualitySettings.antiAliasing = (int)antiAliasing;

        if (anisotropicFiltering != SettingsManager.CurrentPlayerConfiguration.anisotropicFiltering)
            QualitySettings.anisotropicFiltering = anisotropicFiltering;
        #endregion


        #region Audio
        var audioConfiguration = AudioSettings.GetConfiguration();

        if (deviceMode != SettingsManager.CurrentPlayerConfiguration.deviceMode)
        {
            Enum.TryParse(SettingsManager.CurrentPlayerConfiguration.deviceMode.ToString(), out AudioSpeakerMode result);
            audioConfiguration.speakerMode = result;
        }

        if (masterVolume != SettingsManager.CurrentPlayerConfiguration.masterVolume)
            AudioListener.volume = masterVolume;

        if (effectsVolume != SettingsManager.CurrentPlayerConfiguration.effectsVolume)
            Console.Log(new NotImplementedException());

        if (musicVolume != SettingsManager.CurrentPlayerConfiguration.musicVolume)
            Console.Log(new NotImplementedException());

        if (interfaceVolume != SettingsManager.CurrentPlayerConfiguration.interfaceVolume)
            Console.Log(new NotImplementedException());

        AudioSettings.Reset(audioConfiguration);
        #endregion

        Console.Log($"Changes saved at {Application.persistentDataPath + "/save.cfg"}");
        Save();
        SettingsManager.CurrentPlayerConfiguration = Clone();
    }


}