using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

// ReSharper disable once CheckNamespace
namespace Assets.Modules
{
    public class Console : MonoBehaviour
    {
        public static void Log(object message, [CallerLineNumber] int sourceLineNumber = 0, [CallerFilePath] string sourceFilePath = "")
        {
            var sourceFilePathArray = sourceFilePath.Split('\\');
            var sourceScript = sourceFilePathArray[sourceFilePathArray.Length - 1];
            Write($"[{sourceScript} @ {sourceLineNumber}]", $"{message}");
        }

        private static void Write(string source, string text)
        {
            if (Singleton == null) return;

            var timeStamp = $"{DateTime.Now:hh:mm:ss tt}";
            var instantiatedConsoleTextObject = Instantiate(ConsoleTextPrefab, ContentTransform);
            instantiatedConsoleTextObject.transform.Find("MESSAGE").GetComponent<Text>().text = text;
            instantiatedConsoleTextObject.transform.Find("TIME STAMP").GetComponent<Text>().text = timeStamp;
            instantiatedConsoleTextObject.transform.Find("SOURCE").GetComponent<Text>().text = source;

            ConsoleLines.Add(instantiatedConsoleTextObject);
            if (ConsoleLines.Count > _maxLines)
            {
                Destroy(ConsoleLines[0]);
                ConsoleLines.RemoveAt(0);
            }

            if (!Singleton.gameObject.GetComponent<Canvas>().enabled)
                Singleton.gameObject.transform.Find("Panel").Find("Scrollbar").GetComponent<Scrollbar>().value = 0f;
            System.Console.WriteLine($"{source}: {text}");
        }


        public void Clear()
        {
            foreach (var line in ConsoleLines)
                Destroy(line);
            ConsoleLines.Clear();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public static bool IsVisible => Singleton.transform.GetChild(0).gameObject.activeSelf;

        public static void ToggleConsoleCanvas()
        {
            Singleton.transform.GetChild(0).gameObject.SetActive(!Singleton.transform.GetChild(0).gameObject.activeSelf);
            Singleton.transform.GetChild(1).gameObject.SetActive(!Singleton.transform.GetChild(1).gameObject.activeSelf);
            if (Singleton.gameObject.GetComponent<Canvas>().enabled)
            {
                InputField.text = string.Empty;
                InputField.Select();
                InputField.ActivateInputField();
                selectedConsoleLine = PreviousConsoleLines.Count - 1;
            }
            HideDropDownSelections();
        }

        private static void HideDropDownSelections()
        {
            foreach (var obj in (Dropdown[])FindObjectsOfType(typeof(Dropdown)))
                obj.Hide();
        }

        private void Awake()
        {
            if (Singleton != null && Singleton != this)
            {
                Destroy(this);
                return;
            }
            Singleton = this;
            DontDestroyOnLoad(gameObject);
            ConsoleTextPrefab = _consoleTextPrefab;
            ContentTransform = _contentTransform;
            SendButton = _sendButton;
            InputField = _inputField;
            _sendButton.onClick.AddListener(() =>
            {
                if (string.IsNullOrEmpty(_inputField.text)) return;
                Send(_inputField.text);
                _inputField.text = string.Empty;

            });

            _clearButton.onClick.AddListener(Clear);

            Application.logMessageReceivedThreaded += LogMessagesHandler;
        }
        private void OnApplicationQuit() => Application.logMessageReceivedThreaded -= LogMessagesHandler;

        public static void Send(string msg)
        {
            if (string.IsNullOrEmpty(msg)) return;
            PreviousConsoleLines.Add(msg);
            var cmd = msg.Split(new[] { " " }, StringSplitOptions.None)[0].ToLower();
            if (validCommand(cmd))
            {
                //Write("Running:", msg);
                Commands[cmd].Function(msg);
            }
            else
                Write("Failed:", $"Unknown Command: {cmd}");
        }

        public static bool validCommand(string cmd) => Commands.ContainsKey(cmd);

        private static void LogMessagesHandler(string condition, string stacktrace, LogType type)
        {
            var trace = new StackTrace(4, true);
            stacktrace = stacktrace.Replace("\n", "");

            switch (type)
            {
                case LogType.Log:
                    Write($"(<color=yellow>Log</color>)", $"{condition}{type}{trace}");
                    break;
                case LogType.Error:
                    Write($"(<color=red>Error</color>)", $"{condition}{type}{trace}");
                    break;
                case LogType.Assert:
                    Write("(<color=cyan>Assert</color>)", $"{condition}{type}{trace}");
                    break;
                case LogType.Exception:
                    Write("(<color=maroon>Exception</color>)", $"{condition}{type}{trace}");
                    break;
                case LogType.Warning:
                    Write("(<color=orange>Warning</color>)", $"{condition}{trace}");
                    break;
                default:
                    Write("", $"{condition}{trace}");
                    break;
            }
        }

        public static Console Singleton { get; private set; }
#pragma warning disable 649
        // ReSharper disable once NotNullMemberIsNotInitialized
        [SerializeField] [NotNull] private GameObject _consoleTextPrefab;
        // ReSharper disable once NotNullMemberIsNotInitialized
        [SerializeField] [NotNull] private Transform _contentTransform;
        // ReSharper disable once NotNullMemberIsNotInitialized
        [SerializeField] [NotNull] private Button _sendButton;
        // ReSharper disable once NotNullMemberIsNotInitialized
        [SerializeField] [NotNull] private Button _clearButton;
        // ReSharper disable once NotNullMemberIsNotInitialized
        [SerializeField] [NotNull] private InputField _inputField;
#pragma warning restore 649
        public static GameObject ConsoleTextPrefab;
        public static Transform ContentTransform;
        public static Button SendButton;
        public static InputField InputField;
        public static List<string> PreviousConsoleLines = new List<string>();
        public static int selectedConsoleLine;
        private static readonly List<GameObject> ConsoleLines = new List<GameObject>();
        private static readonly int _maxLines = 100;

        private static readonly Dictionary<string, Command> Commands = new Dictionary<string, Command>
        {
            {"quit", new Command
            { Description = "Quit the game", Function = (cmd) => {
#if !UNITY_EDITOR
        Application.Quit();
#else
                EditorApplication.isPlaying = false;
#endif
            }
            }},
            {"connect", new Command
            {Description = "Connect to a specified IP and Port {Connect ConnectionIP:Port}", Function = (cmd) =>
                {
                    if (string.IsNullOrEmpty(cmd))
                    {
                        Write("Error", "Failed to run command, missing parameters {Connect ConnectionIP:Port}");
                        return;
                    }

                    var split = cmd.ToLower().Split(new []{"connect", ":"}, StringSplitOptions.RemoveEmptyEntries);
                    if (split.Length < 2)
                    {
                        Write("Error", "Failed to run command, missing parameters {Connect ConnectionIP:Port}");
                        return;
                    }

                    var ip = split[0].Replace(" ", "");
                    var port = split[1].Replace(" ", "");
                    // TODO: network.Connect(ip:port);
                    Write("", $"{new NotImplementedException($"Connect {ip}:{port}")}");
                }}},
            {"host", new Command{ Description = "Host server on a specified port {Host Port}", Function = (cmd) =>
                {
                    if (string.IsNullOrEmpty(cmd))
                    {
                        Write("Error", "Failed to run command, missing parameters {Host Port}");
                        return;
                    }

                    var split = cmd.ToLower().Split(new []{"host", ":"}, StringSplitOptions.RemoveEmptyEntries);
                    if (split.Length < 1)
                    {
                        Write("Error", "Failed to run command, missing parameters {Host Port}");
                        return;
                    }

                    var port = split[0].Replace(" ", "");
                    // TODO: network.Host(port);
                    Write("", $"{new NotImplementedException($"Host {port}")}");

                }}},
            {"help", new Command
            {Description = "Display all the commands", Function = (cmd) =>
            {
                Write("Console",$"A total of {Console.Commands.Count} commands exist currently");
                foreach (var cmds in Console.Commands){
                    Write("[help]",$"[{cmds.Key}] -> {cmds.Value.Description}");
                }
            }}},
            {"clear", new Command
            {Description = "Clear the console", Function = (cmd) =>
            {
                Singleton.Clear();
            }}}
        };

        public struct Command
        {
            public string Description;
            public Action<string> Function;
        }
    }
}
