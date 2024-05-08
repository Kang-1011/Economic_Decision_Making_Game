// All code was written by the team

using NUnit.Framework;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Transact;

public class StocksPortfolioTests
{
    private int testRound;

    [SetUp]
    public void Setup()
    {
        testRound = 1;
    }

    [Test]
    public void LoadStocksPortfolio_DataLoadedCorrectly()
    {
        // Open the scene in the Editor
        EditorSceneManager.OpenScene("Assets/Scenes/Portfolio.unity");
        // Check if the scene is open
        Scene activeScene = EditorSceneManager.GetActiveScene();
        Assert.AreEqual("Portfolio", activeScene.name, "Scene not loaded properly");

        // Get the root GameObject of the scene
        GameObject[] rootObjects = EditorSceneManager.GetActiveScene().GetRootGameObjects();
        // Check if the root GameObject is null
        Assert.IsNotNull(rootObjects, "RootObject not found in the scene");

        // Search for a specific GameObject by name
        GameObject stocksChosen = null;
        foreach (GameObject rootObject in rootObjects)
        {
            if (rootObject.name == "Canvas")
            {
                GameObject targetObject = rootObject.transform.Find("SelectedGameObject").gameObject;
                stocksChosen = targetObject.transform.Find("StocksChosen").gameObject;
            }
        }
        // Check if GameObject 'stocksChosen' is null
        Assert.IsNotNull(stocksChosen, "GameObject 'StocksChosen' not found in the scene");


        StocksPortfolio stocksPortfolioInstance = stocksChosen.AddComponent<StocksPortfolio>();
        // Check if the script component is null
        Assert.IsNotNull(stocksPortfolioInstance, "Component 'StocksPortfolio' not found in the scene");

        stocksPortfolioInstance.round = testRound;

        // Act
        stocksPortfolioInstance.LoadStocksPortfolio();
        // Act

        Transform entryContainer = stocksChosen.transform.Find("EntryContainer");

        string balanceFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/StocksBalance.json");
        string jsonString = File.ReadAllText(balanceFilePath);
        StockBalanceWrapper jsonData = JsonUtility.FromJson<StockBalanceWrapper>(jsonString);

        float templateHeight = -90f;
        int i = 0;

        foreach (StockBalance stock in jsonData.Balance)
        {
            Transform clonedEntry = entryContainer.GetChild(i);
            RectTransform clonedEntryRectTransform = clonedEntry.GetComponent<RectTransform>();

            FuturesMarket.MarketData filteredData = stocksPortfolioInstance.FilterData(stock.Ticker);

            Assert.AreEqual(new Vector2(0, 0 + templateHeight * (i)), clonedEntryRectTransform.anchoredPosition);
            Assert.AreEqual(stock.Ticker, clonedEntry.Find("TickerText").GetComponent<Text>().text);
            Assert.AreEqual(filteredData.Name, clonedEntry.Find("NameText").GetComponent<Text>().text);
            Assert.AreEqual(stock.Quantity.ToString(), clonedEntry.Find("BalanceText").GetComponent<Text>().text);
            Assert.AreEqual(filteredData.Price, clonedEntry.Find("PriceText").GetComponent<Text>().text);

            double totalValue = stock.Quantity * float.Parse(filteredData.Price);
            Assert.AreEqual(totalValue.ToString("0.00"), clonedEntry.Find("TotalValueText").GetComponent<Text>().text);

            i++;
        }       
    }

    [Test]
    public void FilterData_DataReturnedCorrectly()
    {
        // Arrange
        GameObject gameObject = new GameObject();
        StocksPortfolio stocksPortfolioInstance = gameObject.AddComponent<StocksPortfolio>();
        stocksPortfolioInstance.round = testRound;
        string testTicker = "SZ677";

        // Act
        FuturesMarket.MarketData filteredData = stocksPortfolioInstance.FilterData(testTicker);

        // Assert
        Assert.AreEqual(testTicker, filteredData.Ticker);
        Assert.AreEqual(testRound, filteredData.Round);
    }
}
