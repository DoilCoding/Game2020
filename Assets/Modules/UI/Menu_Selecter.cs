using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace Assets.Modules
{
    public class Menu_Selecter : MonoBehaviour
    {
        private void Awake()
        {
            var buttons = transform.GetComponentsInChildren<Button>();
            buttons[0].transform.GetChild(0).GetComponent<Text>().text = "Options";
            buttons[0].onClick.AddListener(() => Options.SetActive(true));
        }
    }
}