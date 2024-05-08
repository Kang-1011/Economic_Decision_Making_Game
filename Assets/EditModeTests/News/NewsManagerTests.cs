// All code was written by the team

using System.IO;
using NUnit.Framework;
using UnityEngine;

public class NewsManagerTests
{
    private int testRound;
    NewsManager newsManagerInstance;
    NewsLog jsonData;

    [SetUp]
    public void Setup()
    {
        testRound = 1;
        GameObject gameObject = new GameObject();

        // Make empty file
        ResetGame resetGameInstance = gameObject.AddComponent<ResetGame>();
        resetGameInstance.ClearJSONFile("Data/News/NewsLog.json");

        newsManagerInstance = gameObject.AddComponent<NewsManager>();
        newsManagerInstance.round = testRound;
    }

    [Test]
    public void InitNews_LogFileEmpty_InitializeNewsLog()
    {
        newsManagerInstance.InitNews();

        readIntoJsonData();

        Assert.AreEqual(1, jsonData.Logs.Length);
        Assert.AreEqual(testRound, jsonData.Logs[jsonData.Logs.Length - 1].Round);
        Assert.AreEqual("Implementing loose monetary policy, the central bank has lowered the interest rates for mortgages with a term of over five years and decreased the minimum down payment ratio to 20%.", jsonData.Logs[0].Description);
        Assert.AreEqual("Loose monetary policy, by increasing the money supply and lowering interest rates, often stimulates housing demand, driving up property prices.", jsonData.Logs[0].Explanation);
    }
    
    [Test]
    public void InitNews_LogFileNotEmpty_NewsLogUnchanged()
    {
        // Create an entry so that the log file is not empty
        newsManagerInstance.InitNews();

        Assert.AreEqual(1, jsonData.Logs.Length);
        Assert.AreEqual(testRound, jsonData.Logs[jsonData.Logs.Length - 1].Round);
        Assert.AreEqual("Implementing loose monetary policy, the central bank has lowered the interest rates for mortgages with a term of over five years and decreased the minimum down payment ratio to 20%.", jsonData.Logs[0].Description);
        Assert.AreEqual("Loose monetary policy, by increasing the money supply and lowering interest rates, often stimulates housing demand, driving up property prices.", jsonData.Logs[0].Explanation);

        // Log file is not empty now
        newsManagerInstance.InitNews();

        readIntoJsonData();

        Assert.AreEqual(1, jsonData.Logs.Length);
        Assert.AreEqual(testRound, jsonData.Logs[jsonData.Logs.Length - 1].Round);
        Assert.AreEqual("Implementing loose monetary policy, the central bank has lowered the interest rates for mortgages with a term of over five years and decreased the minimum down payment ratio to 20%.", jsonData.Logs[0].Description);
        Assert.AreEqual("Loose monetary policy, by increasing the money supply and lowering interest rates, often stimulates housing demand, driving up property prices.", jsonData.Logs[0].Explanation);
    }

    [Test]
    public void ReadNews_NonEmptyNews_NewsLogUpdated()
    {
        newsManagerInstance.ReadNews();

        readIntoJsonData();

        Assert.AreEqual(1, jsonData.Logs.Length);
        Assert.AreEqual(testRound, jsonData.Logs[jsonData.Logs.Length - 1].Round);
        Assert.AreEqual("Implementing loose monetary policy, the central bank has lowered the interest rates for mortgages with a term of over five years and decreased the minimum down payment ratio to 20%.", jsonData.Logs[jsonData.Logs.Length - 1].Description);
        Assert.AreEqual("Loose monetary policy, by increasing the money supply and lowering interest rates, often stimulates housing demand, driving up property prices.", jsonData.Logs[jsonData.Logs.Length - 1].Explanation);
    }

    [Test]
    public void ReadNews_EmptyNews_NewsLogUnchanged()
    {
        // No news in round 2, if function is valid, there's no news in the log 
        newsManagerInstance.round = 2;
        newsManagerInstance.ReadNews();

        readIntoJsonData();

        Assert.AreEqual(null, jsonData);
    }

    [Test]
    public void SaveToLog_SaveWithoutClearingHistory()
    {
        // 1st mock entry
        News testNews = new News(100, "TestDescription", "TestExplanation");
        newsManagerInstance.SaveToLog(testNews);

        readIntoJsonData();

        Assert.AreEqual(1, jsonData.Logs.Length);
        Assert.AreEqual(testNews.Round, jsonData.Logs[jsonData.Logs.Length - 1].Round);
        Assert.AreEqual(testNews.Description, jsonData.Logs[jsonData.Logs.Length - 1].Description);
        Assert.AreEqual(testNews.Explanation, jsonData.Logs[jsonData.Logs.Length - 1].Explanation);

        // 2nd mock entry
        News testNews2 = new News(200, "TestDescription2", "TestExplanation2");
        newsManagerInstance.SaveToLog(testNews2);

        readIntoJsonData();

        Assert.AreEqual(2, jsonData.Logs.Length);
        Assert.AreEqual(testNews2.Round, jsonData.Logs[jsonData.Logs.Length - 1].Round);
        Assert.AreEqual(testNews2.Description, jsonData.Logs[jsonData.Logs.Length - 1].Description);
        Assert.AreEqual(testNews2.Explanation, jsonData.Logs[jsonData.Logs.Length - 1].Explanation);
    }

    private void readIntoJsonData()
    {
        string logFilePath = Path.Combine(Application.streamingAssetsPath, "Data/News/NewsLog.json");
        string jsonString = File.ReadAllText(logFilePath);
        jsonData = JsonUtility.FromJson<NewsLog>(jsonString);
    }
}
