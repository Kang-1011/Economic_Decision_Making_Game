// All code was written by the team

using System.IO;
using UnityEngine;
using static Transact;

public class ResetGame : MonoBehaviour
{
    // Clear the content of JSON file
    public void ClearJSONFile(string filePath)
    {
        filePath = Path.Combine(Application.streamingAssetsPath, filePath);
        File.WriteAllText(filePath, "");
    }

    // Reset StocksBalance.json to initial state
    public void ResetStockBalance()
    {
        string balanceFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/StocksBalance.json");
        string jsonString = File.ReadAllText(balanceFilePath);
        StockBalanceWrapper stockBalanceList = JsonUtility.FromJson<StockBalanceWrapper>(jsonString);

        foreach (StockBalance stock in stockBalanceList.Balance)
        {
            stock.Quantity = 0.0;
            stock.Cost = 0.0;
            stock.RealizedProfit = 0.0;
        }

        // 1st parameter is the object instance to convert into the json
        // 2nd parameter is to format the output for readability (prettyPrint)
        string updatedJson = JsonUtility.ToJson(stockBalanceList, true);
        File.WriteAllText(balanceFilePath, updatedJson);
    }
}
