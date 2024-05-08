// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class ScrollViewTextTests
{
    [Test]
    public void ShowText_DisplayCorrectly()
    {
        // Arrange
        EditorSceneManager.OpenScene("Assets/Scenes/Futures_Stock/Futures_Stock.unity");
        GameObject[] rootObjects = EditorSceneManager.GetActiveScene().GetRootGameObjects();
        GameObject textContainer = null;
        Transform scrollbar = null;
        Transform textUI = null;

        foreach (GameObject rootObject in rootObjects)
        {
            if (rootObject.name == "FuturesMarketWithCanvas")
            {
                GameObject gameObject1 = rootObject.transform.Find("FuturesInfo").gameObject;
                GameObject gameObject2 = gameObject1.transform.Find("ScrollArea").gameObject;
                textContainer = gameObject2.transform.Find("TextContainer").gameObject;
                scrollbar = textContainer.transform.Find("Scrollbar");
                textUI = textContainer.transform.Find("Text");
            }
        }
        ScrollViewText scrollViewTextInstance = textContainer.AddComponent<ScrollViewText>();
        string textFilePath = "Text/FuturesInfo.txt";
        string expectedString = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, textFilePath));

        // Act
        scrollViewTextInstance.ShowText(textFilePath);

        // Assert
        Assert.AreEqual(expectedString, textUI.GetComponent<Text>().text, "The content(string) in the scrollview should match the content of the given text file");
    }

}
