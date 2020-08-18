using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Console = Assets.Modules.Console.Console;

public class MenuOptions : MonoBehaviour
{
    public static MenuOptions singleton { get; private set; }
    private void Awake()
    {
        singleton = this;
        var slider = transform.Find("Visible").Find("Slider").GetComponent<Slider>();
        ActiveIndexGameObject = _indexGameObjects[(int)slider.value];
        slider.onValueChanged.AddListener((float changed) => { OnClick(Convert.ToInt32(changed)); });

        transform.Find("Visible").Find("Confirmation").Find("Cancel").GetComponent<Button>().onClick.AddListener(CloseConfirmationWindow);
        transform.Find("Visible").Find("Confirmation").Find("Confirm").GetComponent<Button>().onClick.AddListener(() =>
        {
            CloseConfirmationWindow();
            SettingsManager.singleton.ResetToDefaults();
        });
    }

    public static bool IsVisible => singleton.transform.Find("Visible").gameObject.activeSelf;

    public void ToggleMenuCanvas()
    {
        var visible = singleton.transform.Find("Visible");
        visible.gameObject.SetActive(!visible.gameObject.activeSelf);
        foreach (var obj in (Dropdown[])FindObjectsOfType(typeof(Dropdown)))
            obj.Hide();
        CloseConfirmationWindow();
    }

    public void ToggleServerList()
    {
        Console.Log(new NotImplementedException());
    }

    public void ApplySettings() => SettingsManager.singleton.ApplySettings();
    public bool ConfirmationWindowVisible => singleton.transform.Find("Visible").Find("Confirmation").gameObject.activeSelf;
    public bool RebindWindowVisible => singleton.transform.Find("Visible").Find("Rebinding").gameObject.activeSelf;

    public void ListenForInput(Transform target)
    {
        OpenRebindingWindow();
        StartCoroutine(ListenForInputHandler(target));
    }

    public void OpenRebindingWindow()
    {
        var self = singleton.transform.Find("Visible").Find("Rebinding").gameObject;
        self.SetActive(true);
    }

    // TODO: support for scroll mouse up / down
    // dont use a foreach 
    private static IEnumerator ListenForInputHandler(Transform self)
    {
        var parent = self.parent;
        var done = false;   
        while (!done)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (!Input.GetKey(key) || key == KeyCode.None) continue;
                if (Input.GetKey(KeyCode.BackQuote))
                {
                    done = true;
                    CloseRebindingWindow();
                    break;
                }

                var _key = key;
                if (Input.GetKey(KeyCode.Escape))
                    _key = KeyCode.None;

                    if (!Enum.TryParse(parent.name, true, out Keybinding.ActionType result)) continue;

                if (self.name == "Primary")
                    InputManager.Actions[result] = new Keybinding { Primary = _key, Secondary = InputManager.Actions[result].Secondary};
                else
                    InputManager.Actions[result] = new Keybinding { Primary = InputManager.Actions[result].Primary, Secondary = _key};
                self.Find("Text").GetComponent<Text>().text = $"{_key}";
                done = true;
                CloseRebindingWindow();
                break;
            }
            yield return null;
        }
    }

    public static void CloseRebindingWindow()
    {
        var self = singleton.transform.Find("Visible").Find("Rebinding").gameObject;
        self.SetActive(false);
    }

    public void OpenConfirmationWindow()
    {
        var self = singleton.transform.Find("Visible").Find("Confirmation").gameObject;
        self.SetActive(true);
    }

    public void CloseConfirmationWindow()
    {
        var self = singleton.transform.Find("Visible").Find("Confirmation").gameObject;
        self.SetActive(false);
    }

    public void ExitGame()
    {
#if !UNITY_EDITOR
        Application.Quit();
#else
        EditorApplication.isPlaying = false;
#endif
    }

    public void OnClick(int index)
    {
        transform.Find("Visible").Find("Slider").GetComponent<Slider>().value = index;
        if (ActiveIndexGameObject != _indexGameObjects[index]) ActiveIndexGameObject.SetActive(false);
        ActiveIndexGameObject = _indexGameObjects[index];
        ActiveIndexGameObject.SetActive(true);
    }

#pragma warning disable 649
    [SerializeField] private GameObject[] _indexGameObjects;
#pragma warning restore 649
    public GameObject ActiveIndexGameObject { get; private set; }
}
