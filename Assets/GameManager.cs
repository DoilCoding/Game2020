using System.Collections;
using System.Collections.Generic;
using Assets.Menu;
using UnityEngine;

// mouse exit screen
public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(gameObject);
            return;
        }
        Singleton = this;

        // Console Console = new Console();
        // Console.Create();

        InputManager = new InputManager();
        Configuration.RequestedPlayerSettings = Assets.Menu.Config.Load();
        Configuration.RequestedPlayerSettings.resolution = Screen.resolutions[Screen.resolutions.Length - 1].ToString();
        Configuration.RequestedPlayerSettings.Apply();
    }

    private void Start()
    {

    }

    private void Update()
    {
        InputManager.Update();
    }

    private void FixedUpdate()
    {

    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    ~GameManager()
    {

    }

    public static GameManager Singleton { get; private set; }
    public static InputManager InputManager { get; private set; }
}
