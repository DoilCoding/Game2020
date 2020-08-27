using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Assets.Menu;
using Assets.Modules.Core;
//using Assets.network_testing;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;
using Console = Assets.Modules.Console;

// mouse exit screen
// ip connect server
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

            //Networking = Application.isBatchMode ? new Server() : (Networking)new Client();

            //var p = new Packet
            //{
            //    ID = 4,                     // 4
            //    Owner = 1,                  // 4
            //    ReceivedLastPacket = 99,    // 4
            //    DataType = 4,               // 1
            //    PositionX = 12,             // 1
            //    PositionY = -128,           // 1
            //    PositionZ = 127,            // 1
            //    RotationX = 0,              // 1
            //    RotationY = 0,              // 1
            //    RotationZ = 0,              // 1
            //    RotationW = 0               // 1
            //                                // 16 + 1
            //};
            //var bytes = p.Encode();

            //var s1 = Convert.ToString(bytes[12], 2).PadLeft(8, '0');
            //Debug.Log(s1);


            if (!IsClient) return;

            InputManager = new InputManager();
            Configuration.RequestedPlayerSettings = Config.Load();
            if (string.IsNullOrEmpty(Configuration.RequestedPlayerSettings.resolution))
                Configuration.RequestedPlayerSettings.resolution =
                    Screen.resolutions[Screen.resolutions.Length - 1].ToString();
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
        //public static Networking Networking { get; private set; }
        public bool IsClient => !Application.isBatchMode;
        public bool IsServer => !IsClient;
    }
}
