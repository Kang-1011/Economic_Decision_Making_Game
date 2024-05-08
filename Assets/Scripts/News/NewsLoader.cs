// All code was written by the team

using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NewsLoader : MonoBehaviour
{
    string logFilePath;
    string jsonString;
    NewsLog jsonData;

    GameObject newsScrollView;
    GameObject explanation;
    GameObject closeButton;

    // Load news from news log
    public void Start()
    {
        newsScrollView = GameObject.Find("NewsScrollView");
        explanation = GameObject.Find("Explanation");
        explanation.SetActive(false);

        Transform content = transform.Find("Content");
        Transform entry = content.Find("Entry");
        entry.gameObject.SetActive(false);

        logFilePath = Path.Combine(Application.streamingAssetsPath, "Data/News/NewsLog.json");
        jsonString = File.ReadAllText(logFilePath);

        if (jsonString == "")
        {
            return;
        }

        jsonData = JsonUtility.FromJson<NewsLog>(jsonString);

        RectTransform entryRectTransform = entry.GetComponent<RectTransform>();
        float entryHeight = entryRectTransform.rect.height;
        float entryXPos = entryRectTransform.anchoredPosition.x;
        float gap = 15f;

        float initialHeight = -75;
        float heightDecline = -(entryHeight + gap);
        int entryCount = 0;
        int maxEntries = 20;

        for (int i = jsonData.Logs.Length - 1; i >= 0; i--, entryCount++)
        {
            // Stop loading news entry if # entries reaches maximum 
            if (entryCount >= maxEntries)
                break;

            Transform newsEntry = Instantiate(entry, content);
            RectTransform newsEntryRectTransform = newsEntry.GetComponent<RectTransform>();
            newsEntryRectTransform.anchoredPosition = new Vector2(entryXPos, initialHeight + heightDecline * entryCount);
            newsEntry.gameObject.SetActive(true);

            newsEntry.Find("NewsDescription").GetComponent<Text>().text = "Round " + jsonData.Logs[i].Round + ": \n" + jsonData.Logs[i].Description;

            // Hide the info button if there's no explanation
            if (jsonData.Logs[i].Explanation == "")
            {
                newsEntry.Find("InfoButton").gameObject.SetActive(false);
            }

            int index = i;
            newsEntry.Find("InfoButton").GetComponent<Button>().onClick.AddListener(() => ShowExplanation(index));
        }

        RectTransform contentRectTransform = content.GetComponent<RectTransform>();
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, entryHeight * entryCount + (entryCount - 1) * gap);

        // Destroy template game object
        foreach (Transform child in content)
        {
            if (child.gameObject.name == "Entry")
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    // Redirect to news details page
    public void ShowExplanation(int j)
    {
        newsScrollView.SetActive(false);
        explanation.SetActive(true);

        closeButton = GameObject.Find("CloseButton");
        closeButton.SetActive(false);
        
        GameObject explanationText = GameObject.Find("ExplanationText");
        explanationText.GetComponent<Text>().text = "Round " + jsonData.Logs[j].Round + ": \n" + jsonData.Logs[j].Description + "\n\nInfo:\n" + jsonData.Logs[j].Explanation;
    }

    // Redirect back to news log(history) page
    public void CloseExplanation()
    {
        newsScrollView.SetActive(true);
        explanation.SetActive(false);
        closeButton.SetActive(true);
    }
}
