using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Menu;
using UnityEngine;
using UnityEngine.EventSystems;
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

            if (Input.GetKeyDown(KeyCode.Return) && Console.IsVisible && EventSystem.current.currentSelectedGameObject.transform == Console.InputField.transform)
            {
                Console.SendButton.onClick.Invoke();
                Console.InputField.Select();
                Console.InputField.ActivateInputField();
                Console.selectedConsoleLine = Console.PreviousConsoleLines.Count - 1;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && Console.IsVisible && EventSystem.current.currentSelectedGameObject.transform == Console.InputField.transform)
            {
                if (Console.PreviousConsoleLines.Count == 0) return;
                Console.selectedConsoleLine++;
                Console.selectedConsoleLine = Mathf.Clamp(Console.selectedConsoleLine, 0, Console.PreviousConsoleLines.Count - 1);
                Console.InputField.text = Console.PreviousConsoleLines[Console.selectedConsoleLine];
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && Console.IsVisible && EventSystem.current.currentSelectedGameObject.transform == Console.InputField.transform)
            {
                if (Console.PreviousConsoleLines.Count == 0) return;
                Console.selectedConsoleLine--;
                Console.selectedConsoleLine = Mathf.Clamp(Console.selectedConsoleLine, 0, Console.PreviousConsoleLines.Count - 1);
                Console.InputField.text = Console.PreviousConsoleLines[Console.selectedConsoleLine];
            }
        }
    }
}