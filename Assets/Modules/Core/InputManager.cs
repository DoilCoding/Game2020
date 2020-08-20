using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Menu;
using UnityEngine;
using Console = Assets.Modules.Console.Console;

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
        reload
    }
}

public class InputManager : MonoBehaviour
{
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
            else if (MenuOptions.IsVisible)
            {
                if (MenuOptions.singleton.ConfirmationWindowVisible)
                    MenuOptions.singleton.CloseConfirmationWindow();
                else if (!MenuOptions.singleton.RebindWindowVisible)
                    MenuOptions.singleton.ToggleMenuCanvas();
            }
        }
        else if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Console.ToggleConsoleCanvas();
        }
    }
}
