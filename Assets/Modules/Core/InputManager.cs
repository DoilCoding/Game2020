using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Menu;
using UnityEngine;
using Console = Assets.Modules.Console;

namespace Assets.Modules.Core
{
    [Serializable]
    public struct Keybinding
    {
        public KeyCode Primary;
        public KeyCode Secondary;

        public enum ActionType
        {
            forward,
            left,
            right,
            back,
            jump,
            crouch,
            reload,
            sprint
        }
    }

    public class InputManager
    {
        public InputManager()
        {
            Console.Log($"{this} initiated");
        }

        [SerializeField]
        public static Dictionary<Keybinding.ActionType, Keybinding> Actions => Configuration.CurrentPlayerSettings.Actions;

        public bool GetKey(Keybinding.ActionType actionType)
        {
            return Input.GetKey(Actions[actionType].Primary) ||
                   Input.GetKey(Actions[actionType].Secondary);
        }

        public bool GetKeyDown(Keybinding.ActionType actionType)
        {
            return Input.GetKeyDown(Actions[actionType].Primary) ||
                   Input.GetKeyDown(Actions[actionType].Secondary);
        }

        public void Update()
        {
            if (Actions == null) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Console.IsVisible)
                {
                    Console.ToggleConsoleCanvas();
                }
                else if (Modules.Options.Active)
                {
                    if (Modules.Options.IsKeybindingPopupOpen) { }
                    else if (Modules.Options.IsConfirmationPopupOpen)
                        Modules.Options.HideConfirmationWindow();
                    else if (Modules.Options.Active)
                        Modules.Options.SetActive(false);
                }
            }
            else if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                Console.ToggleConsoleCanvas();
            }
        }
    }
}