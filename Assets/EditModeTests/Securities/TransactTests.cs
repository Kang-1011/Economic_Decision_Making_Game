// All code was written by the team

using System.IO;
using NUnit.Framework;
using UnityEngine;
using static Transact;

public class TransactTests
{
    Transact transactInstance = null;
    ResetGame resetGameInstance = null;
    string relativeFilePath = "";
    string filePath = "";
    string jsonString = "";
    OrderDataList orderDataList = null;
    StockBalanceWrapper stockBalanceList = null;

    [SetUp]
    public void SetUp()
    {
        GameObject gameObject = new GameObject();
        transactInstance = gameObject.AddComponent<Transact>();
        resetGameInstance = gameObject.AddComponent<ResetGame>();

        relativeFilePath = "Data/Securities/FuturesOrders.json";
        filePath = Path.Combine(Application.streamingAssetsPath, relativeFilePath);
        transactInstance.orderFilePath = filePath;
    }

    [Test]
    public void SaveOrder_Saved()
    {
        // Arrange
        // Make empty file
        resetGameInstance.ClearJSONFile(relativeFilePath);

        int mode1 = 1;
        string ticker1 = "BTC";
        double entryPrice1 = 150.0;
        double quantity1 = 10;
        double totalValue1 = 1500.0;
        double sl1 = 155.0;
        double tp1 = 145.0;
        OrderData testOrder = new OrderData(mode1, ticker1, entryPrice1, quantity1, totalValue1, sl1, tp1);

        // Act
        transactInstance.SaveOrder(testOrder);

        // Arrange again
        readOrdersData();

        // Assert
        Assert.AreEqual(1, orderDataList.Orders.Length);
        Assert.AreEqual("Short", orderDataList.Orders[0].TradeType);
        Assert.AreEqual(ticker1, orderDataList.Orders[0].Ticker);
        Assert.AreEqual(entryPrice1, orderDataList.Orders[0].EntryPrice);
        Assert.AreEqual(quantity1, orderDataList.Orders[0].Quantity);
        Assert.AreEqual(totalValue1, orderDataList.Orders[0].TotalValue);
        Assert.AreEqual(sl1, orderDataList.Orders[0].Sl);
        Assert.AreEqual(tp1, orderDataList.Orders[0].Tp);

        resetGameInstance.ClearJSONFile(relativeFilePath);
    }

    [Test]
    public void SaveOrder_SavedWithoutClearingHistory()
    {
        // Arrange
        // Make empty file
        resetGameInstance.ClearJSONFile(relativeFilePath);
        OrderData testOrder = new OrderData(1, "BTC", 100.0, 0, 0.0, 0.0, 0.0);

        // Act
        transactInstance.SaveOrder(testOrder);

        // Arrange again
        readOrdersData();

        // Assert
        Assert.AreEqual(1, orderDataList.Orders.Length);

        // File should be not empty now
        // Arrange2
        testOrder = new OrderData(1, "ETH", 100.0, 0, 0.0, 0.0, 0.0);

        // Act2
        transactInstance.SaveOrder(testOrder);

        // Arrange2 again
        readOrdersData();

        //Assert 2
        Assert.AreEqual(2, orderDataList.Orders.Length);
        Assert.AreEqual("BTC", orderDataList.Orders[0].Ticker);
        Assert.AreEqual("ETH", orderDataList.Orders[1].Ticker);

        resetGameInstance.ClearJSONFile(relativeFilePath);
    }

    private void readOrdersData()
    {
        jsonString = File.ReadAllText(filePath);
        orderDataList = JsonUtility.FromJson<OrderDataList>(jsonString);
    }

    [Test]
    public void ModifyBalance_Buy_Updated()
    {
        // Arrange
        // Clear balance
        resetGameInstance.ResetStockBalance();

        readBalanceData();

        double preCost = stockBalanceList.Balance[0].Cost;
        double totalValue = 20.0; 
        OrderData testOrder = new OrderData(2, "SZ677", 10.0, 2, totalValue, 0.0, 0.0);

        // Act
        transactInstance.ModifyBalance(testOrder, "Buy");

        readBalanceData();

        // Assert
        Assert.AreEqual(totalValue, stockBalanceList.Balance[0].Cost - preCost);

        resetGameInstance.ResetStockBalance();
    }

    [Test]
    public void ModifyBalance_Sell_Updated()
    {
        // Arrange
        // Clear balance
        resetGameInstance.ResetStockBalance();

        readBalanceData();

        double preProfit = stockBalanceList.Balance[0].RealizedProfit;
        double totalValue = 20.0;
        OrderData testOrder = new OrderData(3, "SZ677", 10.0, 2, totalValue, 0.0, 0.0);

        // Act
        transactInstance.ModifyBalance(testOrder, "Sell");

        readBalanceData();

        // Assert
        Assert.AreEqual(totalValue, stockBalanceList.Balance[0].RealizedProfit - preProfit);

        resetGameInstance.ResetStockBalance();
    }

    private void readBalanceData()
    {
        string balanceFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/StocksBalance.json");
        string jsonString = File.ReadAllText(balanceFilePath);
        stockBalanceList = JsonUtility.FromJson<StockBalanceWrapper>(jsonString);
    }
}
