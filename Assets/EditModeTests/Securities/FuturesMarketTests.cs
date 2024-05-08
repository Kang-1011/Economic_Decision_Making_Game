// All code was written by the team

using System.IO;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using static FuturesMarket;

public class FuturesMarketTests
{
    FuturesMarket futuresMarketInstance = null;
    MarketDataWrapper jsonData;

    [SetUp]
    public void Setup()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Futures_Stock/Futures_Stock.unity");
        GameObject[] rootObjects = EditorSceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            if (rootObject.name == "FuturesMarketWithCanvas")
            {
                GameObject baseGameObject = rootObject.transform.Find("FuturesMarket").gameObject;
                futuresMarketInstance = baseGameObject.AddComponent<FuturesMarket>();
            }
        }
    }

    [Test]
    public void FilterData_ReturnCorrectFuturesData()
    {
        // Arrange
        ReadFuturesData();
        MarketData expectedData = null;
        foreach (var item in jsonData.data)
        {
            if (item.Id == 2 && item.Round == 2)
            {
                expectedData = item;
            }
        }

        // Act
        MarketData returnedData = futuresMarketInstance.FilterData(jsonData.data, 2, 2);

        // Assert
        Assert.AreEqual(expectedData.Round, returnedData.Round);
        Assert.AreEqual(expectedData.Id, returnedData.Id);
        Assert.AreEqual(expectedData.Name, returnedData.Name);
        Assert.AreEqual(expectedData.Ticker, returnedData.Ticker);
        Assert.AreEqual(expectedData.Category, returnedData.Category);
        Assert.AreEqual(expectedData.Price, returnedData.Price);
        Assert.AreEqual(expectedData.Change, returnedData.Change);
        Assert.AreEqual(expectedData.PercentageChange, returnedData.PercentageChange);
        Assert.AreEqual(expectedData.Volume, returnedData.Volume);
        Assert.AreEqual(expectedData.MarketCap, returnedData.MarketCap);
    }

    [Test]
    public void FilterData_ReturnCorrectStocksData()
    {
        // Arrange
        ReadStocksData();
        MarketData expectedData = null;
        foreach (var item in jsonData.data)
        {
            if (item.Id == 2 && item.Round == 2)
            {
                expectedData = item;
            }
        }

        // Act
        MarketData returnedData = futuresMarketInstance.FilterData(jsonData.data, 2, 2);

        // Assert
        Assert.AreEqual(expectedData.Round, returnedData.Round);
        Assert.AreEqual(expectedData.Id, returnedData.Id);
        Assert.AreEqual(expectedData.Name, returnedData.Name);
        Assert.AreEqual(expectedData.Ticker, returnedData.Ticker);
        Assert.AreEqual(expectedData.Category, returnedData.Category);
        Assert.AreEqual(expectedData.Price, returnedData.Price);
        Assert.AreEqual(expectedData.Change, returnedData.Change);
        Assert.AreEqual(expectedData.PercentageChange, returnedData.PercentageChange);
        Assert.AreEqual(expectedData.Volume, returnedData.Volume);
        Assert.AreEqual(expectedData.MarketCap, returnedData.MarketCap);
    }

    private void ReadFuturesData()
    {
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/Futures.json");
        string jsonString = File.ReadAllText(jsonFilePath);

        jsonData = JsonUtility.FromJson<MarketDataWrapper>("{\"data\":" + jsonString + "}");

        futuresMarketInstance.jsonFilePath = jsonFilePath;
    }

    private void ReadStocksData()
    {
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/Stocks.json");
        string jsonString = File.ReadAllText(jsonFilePath);

        jsonData = JsonUtility.FromJson<MarketDataWrapper>("{\"data\":" + jsonString + "}");

        futuresMarketInstance.jsonFilePath = jsonFilePath;
    }
}
