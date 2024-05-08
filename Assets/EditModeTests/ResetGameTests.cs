// All code was written by the team

using System.IO;
using NUnit.Framework;
using UnityEngine;
using static Transact;

public class ResetGameTests
{
    [Test]
    public void ClearJSONFile_EmptyStringInFileAtFilePath()
    {
        // Arrange
        GameObject gameObject = new GameObject();
        ResetGame resetGameInstance = gameObject.AddComponent<ResetGame>();
        string expected = "";
        string filePath = "Data/News/NewsLog.json";

        // Act
        resetGameInstance.ClearJSONFile(filePath);

        // Arrange
        string jsonString = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, filePath));

        // Assert
        Assert.AreEqual(expected, jsonString);
    }

    [Test]
    public void ResetStockBalance_AllFieldsEqualsZero()
    {
        // Arrange
        GameObject gameObject = new GameObject();
        ResetGame resetGameInstance = gameObject.AddComponent<ResetGame>();
        double expected = 0.0;

        // Act
        resetGameInstance.ResetStockBalance();

        // Arrange
        string balanceFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/StocksBalance.json");
        string jsonString = File.ReadAllText(balanceFilePath);
        StockBalanceWrapper stockBalanceList = JsonUtility.FromJson<StockBalanceWrapper>(jsonString);

        // Assert
        foreach (StockBalance stock in stockBalanceList.Balance)
        {
            Assert.AreEqual(expected, stock.Quantity);
            Assert.AreEqual(expected, stock.Cost);
            Assert.AreEqual(expected, stock.RealizedProfit);
        };
    }
}
