// All code was written by the team

using NUnit.Framework;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class NewsLoaderTests
{
    GameObject[] rootObjects = null;
    GameObject newsScrollView = null;
    GameObject explanation = null;
    GameObject closeButton = null;
    GameObject explanationText = null;
    GameObject viewport = null;

    NewsLoader newsLoaderInstance;
    NewsManager newsManagerInstance;
    NewsLog jsonData;

    [SetUp]
    public void Setup()
    {
        // Load some news into the log
        GameObject gameObject = new GameObject();
        newsManagerInstance = gameObject.AddComponent<NewsManager>();
        newsManagerInstance.round = 1;
        newsManagerInstance.ReadNews();
        newsManagerInstance.round = 3;
        newsManagerInstance.ReadNews();
        newsManagerInstance.round = 6;
        newsManagerInstance.ReadNews();

        EditorSceneManager.OpenScene("Assets/Scenes/News.unity");
        rootObjects = EditorSceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            if (rootObject.name == "Canvas")
            {
                newsScrollView = rootObject.transform.Find("NewsScrollView").gameObject;
                explanation = rootObject.transform.Find("Explanation").gameObject;
                closeButton = rootObject.transform.Find("CloseButton").gameObject;
            }
        }
        explanationText = explanation.transform.Find("ExplanationText").gameObject;

        viewport = newsScrollView.transform.Find("Viewport").gameObject;
        newsLoaderInstance = viewport.AddComponent<NewsLoader>();
    }

    [Test]
    public void Start_DataLoadedCorrectly()
    {
        Transform content = viewport.transform.Find("Content");
        Transform entry = content.Find("Entry");

        RectTransform entryRectTransform = entry.GetComponent<RectTransform>();
        float entryHeight = entryRectTransform.rect.height;
        float entryXPos = entryRectTransform.anchoredPosition.x;
        float gap = 15f;

        float initialHeight = -75;
        float heightDecline = -(entryHeight + gap);
        int entryCount = 0;
        int maxEntries = 20;

        newsLoaderInstance.Start();

        Assert.IsFalse(explanation.activeSelf, "GameObject 'Explanation' should be inactive");

        readIntoJsonData();

        for (int i = 0; i < content.transform.childCount; i++, entryCount++)
        {
            Transform newsEntry = content.GetChild(i);
            RectTransform newsEntryRectTransform = newsEntry.GetComponent<RectTransform>();
            Assert.AreEqual(new Vector2(entryXPos, initialHeight + heightDecline * entryCount), newsEntryRectTransform.anchoredPosition);
            Assert.IsTrue(newsEntry.gameObject.activeSelf, $"GameObject 'NewsEntry'{i} should be active");

            Assert.AreEqual("Round " + jsonData.Logs[(jsonData.Logs.Length - 1) - i].Round + ": \n" + jsonData.Logs[(jsonData.Logs.Length - 1) - i].Description, newsEntry.Find("NewsDescription").GetComponent<Text>().text);

            if (jsonData.Logs[i].Explanation == "")
            {
                Assert.IsFalse(newsEntry.Find("InfoButton").gameObject.activeSelf, "News entry info button should be inactive");
            }
        }

        Assert.IsTrue(entryCount <= maxEntries, "Number of entries exceeds maximum limit");
    }

    [Test]
    public void ShowExplanation_ShowNext()
    {
        newsLoaderInstance.Start();
        for (int j = 0; j < 3; j++)
        {
            newsLoaderInstance.ShowExplanation(j);

            Assert.IsFalse(newsScrollView.activeSelf, "GameObject 'NewsScrollView' should be inactive");
            Assert.IsTrue(explanation.activeSelf, "GameObject 'Explanation' should be active");
            Assert.IsFalse(closeButton.activeSelf, "GameObject 'CloseButton' should be inactive");

            readIntoJsonData();
            string expectedString = "Round " + jsonData.Logs[j].Round + ": \n" + jsonData.Logs[j].Description + "\n\nInfo:\n" + jsonData.Logs[j].Explanation;
            Assert.AreEqual(expectedString, explanationText.GetComponent<Text>().text, "Displayed incorrect explanation text");

            newsLoaderInstance.CloseExplanation();
        }
    }

    [Test]
    public void CloseExplanation_ShowBack()
    {
        newsLoaderInstance.Start();
        newsLoaderInstance.ShowExplanation(1);
        newsLoaderInstance.CloseExplanation();

        Assert.IsTrue(newsScrollView.activeSelf, "GameObject 'NewsScrollView' should be active");
        Assert.IsFalse(explanation.activeSelf, "GameObject 'Explanation' should be inactive");
        Assert.IsTrue(closeButton.activeSelf, "GameObject 'CloseButton' should be active");
    }

    private void readIntoJsonData()
    {
        string logFilePath = Path.Combine(Application.streamingAssetsPath, "Data/News/NewsLog.json");
        string jsonString = File.ReadAllText(logFilePath);

        if (jsonString == "")
        {
            return;
        }

        jsonData = JsonUtility.FromJson<NewsLog>(jsonString);
    }
}
