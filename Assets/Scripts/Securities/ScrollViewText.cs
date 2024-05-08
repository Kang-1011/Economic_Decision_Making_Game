// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewText : MonoBehaviour
{
    Transform scrollbar;
    Transform textUI;

    public void ShowText(string TextFilePath)
    {
        scrollbar = transform.Find("Scrollbar");
        textUI = transform.Find("Text");

        string text = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, TextFilePath));
        textUI.GetComponent<Text>().text = text;

        // scrollbar always start from top
        scrollbar.GetComponent<Scrollbar>().value = 1.0f;
    }

}
