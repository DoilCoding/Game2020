using UnityEngine;

namespace Assets.Menu
{
    public static class Configuration
    {
        public static Config CurrentPlayerSettings { get; set; }
        public static Config RequestedPlayerSettings { get; set; }
        
        public static void ResetToDefaults()
        {
            RequestedPlayerSettings = new Config
            {
                resolution = Screen.resolutions[Screen.resolutions.Length - 1].ToString()
            };
            RequestedPlayerSettings.Apply();
            Body.Populate();
        }
    }
}