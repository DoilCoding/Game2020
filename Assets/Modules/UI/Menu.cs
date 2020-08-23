using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace Assets.Modules
{
    public class Menu : MonoBehaviour
    {
        private void Awake()
        {
            var buttons = transform.GetComponentsInChildren<Button>();
            buttons[0].transform.GetChild(0).GetComponent<Text>().text = "Options";
            buttons[0].onClick.AddListener(() => Options.SetActive(true));

            buttons[1].transform.GetChild(0).GetComponent<Text>().text = "Exit";
            buttons[1].onClick.AddListener(ExitGame);
        }
        private static void ExitGame()
        {
#if !UNITY_EDITOR
        Application.Quit();
#else
            EditorApplication.isPlaying = false;
#endif
        }

    }
}