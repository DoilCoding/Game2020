using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleMessageResizer : MonoBehaviour
{
    private RectTransform TimeStamp => transform.Find("TIME STAMP").GetComponent<RectTransform>();
    private RectTransform Source => transform.Find("SOURCE").GetComponent<RectTransform>();
    private RectTransform Message => transform.Find("MESSAGE").GetComponent<RectTransform>();

    private void Start()
    {
        StartCoroutine(test());
    }

    IEnumerator test()
    {
        yield return null;
        float targetValue = GetComponent<RectTransform>().sizeDelta.x - (2 * 10);
        float timeStampValue = TimeStamp.sizeDelta.x;
        float sourceValue = Source.sizeDelta.x;
        float newValue = targetValue - (timeStampValue + sourceValue);
        Message.sizeDelta = new Vector2(newValue, 0);
        TimeStamp.GetComponent<Text>().color = new Color(TimeStamp.GetComponent<Text>().color.r, TimeStamp.GetComponent<Text>().color.g, TimeStamp.GetComponent<Text>().color.b, 255);
        Source.GetComponent<Text>().color = new Color(Source.GetComponent<Text>().color.r, Source.GetComponent<Text>().color.g, Source.GetComponent<Text>().color.b, 255);
        Message.GetComponent<Text>().color = new Color(Message.GetComponent<Text>().color.r, Message.GetComponent<Text>().color.g, Message.GetComponent<Text>().color.b, 255);
    }
}
