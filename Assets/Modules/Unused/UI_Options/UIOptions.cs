using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
/*
public class UIOptions : MonoBehaviour
{
    private void Awake()
    {   
        SetScreenResolution();
    }

    public static float PlayerLookSensitivityTesting;
    public static float PlayerAimSensitivityTesting;

    private void Start()
    {
        CreateHeader("Aiming");
        CreateSlider(
            "Look Sensitivity",
            0,
            100,
            12,
            value =>
            {
                PlayerLookSensitivityTesting = value; 
            });
        CreateSlider(
            "Aim Sensitivity",
            0,
            100,
            8,
            value =>
            {
                PlayerAimSensitivityTesting = value; 
            });
        CreateEmpty(1);
        CreateHeader("Graphics");
        CreateDropDown(
            "Resolution",
            -1,
            Screen.resolutions.Where(x => x.refreshRate == Screen.currentResolution.refreshRate).Select(x => x.ToString()).ToArray(),
            value =>
            {
                Resolution selectedResolution = Screen.resolutions.Where(x => x.refreshRate == Screen.currentResolution.refreshRate).ToArray()[value];
                Screen.SetResolution(
                    selectedResolution.width,
                    selectedResolution.height,
                    true,
                    selectedResolution.refreshRate);
            });
        CreateDropDown(
            "Shadows",
            0,
            Enum.GetNames(typeof(ShadowQuality)),
            value =>
            {
                QualitySettings.shadows = (ShadowQuality)Enum.GetValues(typeof(ShadowQuality)).GetValue(value); 

            });
        CreateEmpty(2);
        CreateBool(
            "test boolean",
            true,
            value =>
            {

            });
        CreateEmpty(3);


        foreach (var setting in settings.Values)
            setting.createElement();
    }
    
    public void ApplyChanged()
    {
        foreach (var setting in settings.Values)
            setting.onApply?.Invoke();
        userSettings.Clear();
    }

    private void CreateHeader(string _header)
    {
        var setting = new Settings
        {
            header = _header,
            createElement = () => Instantiate(header_prefab, content).transform.Find("Header").GetComponent<Text>().text = _header
        };
        settings.Add(_header, setting);
    }
    private void CreateEmpty(int id)
    {
        var setting = new Settings
        {
            type = Settings._type.Empty,
            createElement = () => Instantiate(empty_prefab, content)
        };
        settings.Add(id.ToString(), setting);
    }

    /// <summary>
    /// <param name="_default">when setting default to -1 it will select the last value in the list</param>
    /// </summary>
    private void CreateDropDown(string _header, int _default, string[] _dropDown, Action<int> applyFunction)
    {

        var currentPlayerValue = PlayerPrefs.GetString(_header, (_dropDown[_default == -1 ? _dropDown.Length - 1 : _default]).ToString());
        var setting = new Settings
        {
            header = _header,
            type = Settings._type.DropDown,
            value = currentPlayerValue,
            createElement = (() =>
            {
                var instantiatedObject = Instantiate(dropdown_prefab, content).transform;
                instantiatedObject.Find("Header").GetComponent<Text>().text = _header;
                var instantiatedDropDown = instantiatedObject.Find("Dropdown").GetComponent<Dropdown>();
                instantiatedDropDown.options = _dropDown.Select(x => new Dropdown.OptionData { text = x }).ToList();
                instantiatedDropDown.value = Array.FindIndex(_dropDown, x => x == currentPlayerValue);
                instantiatedDropDown.onValueChanged.AddListener((int changedValue) =>
                {
                    userSettings[_header] = _dropDown[changedValue];
                });
            }),
            onApply = (() =>
            {
                if (!userSettings.ContainsKey(_header)) return;
                PlayerPrefs.SetString(_header, (string)userSettings[_header]);
                applyFunction(_dropDown.ToList().FindIndex(x => x == (string)userSettings[_header]));
            })
        };


        settings.Add(_header, setting);
    }

    private void CreateBool(string _header, bool _default, Action<bool> applyFunction)
    {
        var currentPlayerValue = Convert.ToBoolean(PlayerPrefs.GetInt(_header, Convert.ToInt32(_default)));
        var setting = new Settings
        {
            header = _header,
            type = Settings._type.Checkbox,
            value = currentPlayerValue,
            createElement = () =>
            {
                var instantiatedObject = Instantiate(checkbox_prefab, content).transform;
                instantiatedObject.Find("Header").GetComponent<Text>().text = _header;
                var instantiatedToggle = instantiatedObject.Find("Toggle").GetComponent<Toggle>();
                instantiatedToggle.isOn = currentPlayerValue;
                instantiatedToggle.onValueChanged.AddListener(value => { userSettings[_header] = value; });
            },
            onApply = () =>
            {
                if (!userSettings.ContainsKey(_header)) return;
                PlayerPrefs.SetInt(_header, Convert.ToInt32(userSettings[_header]));
                applyFunction((bool)userSettings[_header]);
            }
        };

        settings.Add(_header, setting);
    }

    public static void SetScreenResolution()
    {
        var resolutions = Screen.resolutions.Where(x => x.refreshRate == Screen.currentResolution.refreshRate).ToArray();
        var currentPlayerValue = PlayerPrefs.GetString("Resolution", resolutions[resolutions.Length - 1].ToString());
        var split = currentPlayerValue.Split(new[] { "x", "@", "Hz" }, StringSplitOptions.RemoveEmptyEntries);
        var width = Convert.ToInt32(split[0]);
        var height = Convert.ToInt32(split[1]);
        var refreshRate = Convert.ToInt32(split[2]);
        Screen.SetResolution(width, height, true, refreshRate);
        Application.targetFrameRate = refreshRate;
    }

    private void CreateSlider(string _header, float _min, float _max, float _default, Action<float> applyFunction)
    {
        var currentPlayerValue = PlayerPrefs.GetFloat(_header, _default);
        var setting = new Settings
        {
            header = _header,
            min = _min,
            max = _max,
            type = Settings._type.Slider,
            value = currentPlayerValue,
            createElement = (() =>
            {
                var instantiatedObject = Instantiate(slider_prefab, content).transform;
                var instantiatedSlider = instantiatedObject.Find("Slider").GetComponent<Slider>();

                void ChangeTextValue(float value)
                {
                    instantiatedObject.Find("Value").GetComponent<Text>().text = $"{value:F2}:";
                }

                instantiatedObject.Find("Header").GetComponent<Text>().text = _header;
                instantiatedSlider.minValue = _min;
                instantiatedSlider.maxValue = _max;
                instantiatedSlider.value = currentPlayerValue;
                instantiatedSlider.onValueChanged.AddListener((float changedValue) =>
                {
                    userSettings[_header] = changedValue;
                    ChangeTextValue(changedValue);
                });

                ChangeTextValue(currentPlayerValue);
            }),
            onApply = (() =>
            {
                if (!userSettings.ContainsKey(_header)) return;
                PlayerPrefs.SetFloat(_header, (float)userSettings[_header]);
                applyFunction((float)userSettings[_header]);
            })
        };

        settings.Add(_header, setting);
    }


    private struct Settings
    {
        public string header { get; set; }
        public _type type { get; set; }
        public object value { get; set; }
        public float min { get; set; }
        public float max { get; set; }
        public Action onApply;
        public Action createElement;

        public enum _type
        {
            Slider,
            DropDown,
            Checkbox,
            Header,
            Empty
        }
    }

    private Dictionary<string, Settings> settings { get; set; } = new Dictionary<string, Settings>();
    private Dictionary<string, object> userSettings { get; set; } = new Dictionary<string, object>();


    public GameObject slider_prefab;
    public GameObject dropdown_prefab;
    public GameObject checkbox_prefab;
    public GameObject header_prefab;
    public GameObject empty_prefab;
    public Transform content;
}
*/