// All code was written by the team

using System;
using System.IO;
using UnityEngine;

public class NewsManager : MonoBehaviour
{
    public int round;

    public void Start()
    {
        round = PlayerManager.Instance.GetRound();
    }
    // Initialize the first news entry when the game is in round 1
    public void InitNews()
    {
        string logFilePath = Path.Combine(Application.streamingAssetsPath, "Data/News/NewsLog.json");
        string jsonString = File.ReadAllText(logFilePath);

        if (jsonString == "")
        {
            ReadNews();
        }
    }

    // Load news designed for current round into the log
    public void ReadNews()
    {
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Data/News/News.json");
        string jsonString = File.ReadAllText(jsonFilePath);

        NewsLog jsonData = JsonUtility.FromJson<NewsLog>("{\"Logs\":" + jsonString + "}");

        foreach (var news in jsonData.Logs)
        {
            if (news.Round == round && news.Description != "")
            {
                SaveToLog(news);
            }
        }
    }

    // Append new news entry into news log
    public void SaveToLog(News news)
    {
        News[] logs;
        string logFilePath = Path.Combine(Application.streamingAssetsPath, "Data/News/NewsLog.json");

        if (File.Exists(logFilePath))
        {
            string jsonString = File.ReadAllText(logFilePath);

            if (jsonString == "")
            {
                logs = new News[0];
            }
            else
            {
                NewsLog newsLog = JsonUtility.FromJson<NewsLog>(jsonString);
                logs = newsLog.Logs;
            }
        }
        else
        {
            logs = new News[0];
        }

        Array.Resize(ref logs, logs.Length + 1);

        logs[logs.Length - 1] = news;

        string updatedJson = JsonUtility.ToJson(new NewsLog(logs), true);

        File.WriteAllText(logFilePath, updatedJson);
    }
}
