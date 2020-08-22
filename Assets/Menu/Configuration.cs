using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

        public static IEnumerator ListenForInputHandler(Transform self)
        {
            var parent = self.parent;
            var done = false;
            while (!done)
            {
                foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                {
                    // if rebinding window is closed then done is true
                    if (!Input.GetKey(key) || key == KeyCode.None) continue;
                    if (Input.GetKey(KeyCode.BackQuote))
                    {
                        done = true;
                        //CloseRebindingWindow();
                        break;
                    }

                    var _key = key;
                    if (Input.GetKey(KeyCode.Escape))
                        _key = KeyCode.None;

                    if (!Enum.TryParse(parent.name, true, out Keybinding.ActionType result)) continue;

                    if (self.name == "Primary")
                        InputManager.Actions[result] = new Keybinding { Primary = _key, Secondary = InputManager.Actions[result].Secondary };
                    else
                        InputManager.Actions[result] = new Keybinding { Primary = InputManager.Actions[result].Primary, Secondary = _key };
                    self.Find("Text").GetComponent<Text>().text = $"{_key}";
                    done = true;
                    //CloseRebindingWindow();
                    break;
                }
                yield return null;
            }
        }
    }
}