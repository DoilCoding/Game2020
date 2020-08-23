using System;
using System.Collections;
using Assets.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Modules.Core
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
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {

                    if (!Input.GetKey(keyCode) || keyCode == KeyCode.None) continue;
                    if (Input.GetKey(KeyCode.BackQuote) || !Modules.Options.IsKeybindingPopupOpen)
                    {
                        done = true;
                        Modules.Options.HideKeybindingWindow();
                        break;
                    }

                    var key = keyCode;
                    if (Input.GetKey(KeyCode.Escape))
                        key = KeyCode.None;

                    if (!Enum.TryParse(parent.name, true, out Keybinding.ActionType result)) continue;

                    if (self.name == "Primary")
                        InputManager.Actions[result] = new Keybinding { Primary = key, Secondary = InputManager.Actions[result].Secondary };
                    else
                        InputManager.Actions[result] = new Keybinding { Primary = InputManager.Actions[result].Primary, Secondary = key };
                    self.Find("Text").GetComponent<Text>().text = $"{key}";
                    done = true;
                    Modules.Options.HideKeybindingWindow();
                    break;
                }
                yield return null;
            }
        }
    }
}