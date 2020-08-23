using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Assets.Menu;
using Assets.Modules.Core;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;
using Console = Assets.Modules.Console;

// mouse exit screen
namespace Assets
{
    public class GameManager : MonoBehaviour
    {
        private async void Awake()
        {
            if (Singleton != null && Singleton != this)
            {
                Destroy(gameObject);
                return;
            }

            Singleton = this;
            DontDestroyOnLoad(gameObject);
            InputManager = new InputManager();
            Configuration.RequestedPlayerSettings = Config.Load();
            if (string.IsNullOrEmpty(Configuration.RequestedPlayerSettings.resolution))
                Configuration.RequestedPlayerSettings.resolution = Screen.resolutions[Screen.resolutions.Length - 1].ToString();

            await Configuration.RequestedPlayerSettings.Apply();
            await LevelManager.singleton.LoadNewScene("1.Main");
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
}
