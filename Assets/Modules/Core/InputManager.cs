using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Console = Assets.Modules.Console.Console;

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

    public static readonly Dictionary<Keybinding.ActionType, Keybinding> Actions = new Dictionary<Keybinding.ActionType, Keybinding>
        {
            {Keybinding.ActionType.forward, new Keybinding {Primary = KeyCode.W, Secondary = KeyCode.UpArrow}},
            {Keybinding.ActionType.left, new Keybinding {Primary = KeyCode.A, Secondary = KeyCode.LeftArrow}},
            {Keybinding.ActionType.right, new Keybinding {Primary = KeyCode.D, Secondary = KeyCode.RightArrow}},
            {Keybinding.ActionType.back, new Keybinding {Primary = KeyCode.S, Secondary = KeyCode.DownArrow}},
            {Keybinding.ActionType.jump, new Keybinding {Primary = KeyCode.Space, Secondary = KeyCode.None}},
            {Keybinding.ActionType.crouch, new Keybinding {Primary = KeyCode.LeftControl, Secondary = KeyCode.C}},
            {Keybinding.ActionType.reload, new Keybinding {Primary = KeyCode.R, Secondary = KeyCode.None}},
        };

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
        if (GetKeyDown(Keybinding.ActionType.forward))
        {
            Console.Log(new NotImplementedException());
        }

        if (GetKeyDown(Keybinding.ActionType.left))
        {
            Console.Log(new NotImplementedException());
        }

        if (GetKeyDown(Keybinding.ActionType.right))
        {
            Console.Log(new NotImplementedException());
        }

        if (GetKeyDown(Keybinding.ActionType.back))
        {
            Console.Log(new NotImplementedException());
        }

        if (GetKeyDown(Keybinding.ActionType.jump))
        {
            Console.Log(new NotImplementedException());
        }

        if (GetKeyDown(Keybinding.ActionType.crouch))
        {
            Console.Log(new NotImplementedException());
        }

        if (GetKeyDown(Keybinding.ActionType.reload))
        {
            Console.Log(new NotImplementedException());
        }

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
                else
                    MenuOptions.singleton.ToggleMenuCanvas();
            }
        }
        else if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Console.ToggleConsoleCanvas();
        }
    }
}
