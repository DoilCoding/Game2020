using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Assets.Modules.Console
{
    //TODO: redo the ui elements with the new knowledge we got.
    //optimize, disable objects when not on screen.
    //Console.SetOut
    public class Console : MonoBehaviour
    {
        public static void Log(object message, [CallerLineNumber] int sourceLineNumber = 0, [CallerFilePath] string sourceFilePath = "")
        {
            string[] sourceFilePathArray = sourceFilePath.Split('\\');
            string sourceScript = sourceFilePathArray[sourceFilePathArray.Length - 1];
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
            if (ConsoleLines.Count > maxLines)
            {
                Destroy(ConsoleLines[0]);
                ConsoleLines.RemoveAt(0);
            }

            if (!Singleton.gameObject.GetComponent<Canvas>().enabled)
                Singleton.gameObject.transform.Find("Panel").Find("Scrollbar").GetComponent<Scrollbar>()
                    .value = 0f;
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

        public static bool IsVisible => Singleton.gameObject.GetComponent<Canvas>().enabled;

        public static void ToggleConsoleCanvas()
        {
            Singleton.gameObject.GetComponent<Canvas>().enabled = !Singleton.gameObject.GetComponent<Canvas>().isActiveAndEnabled;
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
                Destroy(this);
            else
                Singleton = this;
            ConsoleTextPrefab = _consoleTextPrefab;
            ContentTransform = _contentTransform;
            Application.logMessageReceivedThreaded += LogMessagesHandler;
        }
        private void OnApplicationQuit() => Application.logMessageReceivedThreaded -= LogMessagesHandler;

        private static void LogMessagesHandler(string condition, string stacktrace, LogType type)
        {
            StackTrace trace = new StackTrace(4, true);
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
#pragma warning restore 649
        public static GameObject ConsoleTextPrefab;
        public static Transform ContentTransform;
        private static readonly List<GameObject> ConsoleLines = new List<GameObject>();
        private static int maxLines = 100;
    }
}
