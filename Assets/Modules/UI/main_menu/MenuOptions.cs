using System;
using System.Collections;
using System.Collections.Generic;
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
            ResetToDefaultsHandler();
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
    public void ResetToDefaultsHandler() => SettingsManager.singleton.ResetToDefaults();
    public void ResetToDefaults() => CloseConfirmationWindow();
    public bool ConfirmationWindowVisible => singleton.transform.Find("Visible").Find("Confirmation").gameObject.activeSelf;

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
