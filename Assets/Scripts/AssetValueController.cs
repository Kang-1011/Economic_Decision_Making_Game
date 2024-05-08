// All code was written by the team

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Codice.Client.BaseCommands.QueryParser;
using static Transact;

public class AssetValueController : MonoBehaviour
{
    public TextMeshProUGUI totalAsset;
    public TextMeshProUGUI liabilitiesText;
    public TextMeshProUGUI availableCash;
    public TextMeshProUGUI netAssetValue;

    public Text monthlyRentIncomeText;
    public Text monthlyPaymentText;

    public float total;
    public float liabilities;
    public float netAsset;

    public PlayerManager playerManager;
    
    void Update()
    {
        playerManager = PlayerManager.Instance;
        UpdateFuturesValue();
        UpdateStocksValue();
        UpdateData();
    }

    public void UpdateData()
    {
        total = playerManager.GetPlayerCash() + playerManager.GetBankDepositValue() + playerManager.GetBankFinanceValue() + playerManager.GetFuturesValue() + playerManager.GetStockValue() +playerManager.houseValue;
        totalAsset.text = "¥ " + total.ToString("F0");
        availableCash.text = "¥ " + playerManager.GetPlayerCash().ToString("F0");   // Show available cash
        liabilities = playerManager.houseLoan + playerManager.GetBankLoanValue();
        liabilitiesText.text = "¥ " + liabilities.ToString("F0");

        // Calculate the net asset value
        netAsset = total - liabilities;
        netAssetValue.text = "¥ " + netAsset.ToString("F0");   // Show net asset value

        monthlyRentIncomeText.text = playerManager.monthlyRentIncome.ToString("F0");
        monthlyPaymentText.text = playerManager.monthlyPayment.ToString("F0");
    }

    public void UpdateFuturesValue()
    {
        string ordersFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/FuturesOrders.json");
        string jsonString = File.ReadAllText(ordersFilePath);
        OrderDataList orderDataList = null;

        double totalFuturesValuation = 0;

        if (jsonString == "")
        {
            
        }
        else
        {
            orderDataList = JsonUtility.FromJson<OrderDataList>(jsonString);

            GameObject gameObject = new GameObject();
            FuturesPortfolio futuresPortfolioInstance = gameObject.AddComponent<FuturesPortfolio>();

            foreach (OrderData order in orderDataList.Orders)
            {
                FuturesMarket.MarketData filteredData = futuresPortfolioInstance.FilterData(order.Ticker.Substring(0, 3));
                if (order.Status == "Ongoing")
                {
                    double currentValue = float.Parse(filteredData.Price) * order.Quantity;
                    totalFuturesValuation += currentValue;
                }
            }

            playerManager.SetFuturesValue((float)totalFuturesValuation);
        }
    }

    public void UpdateStocksValue()
    {
        string balanceFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/StocksBalance.json");
        string jsonString = File.ReadAllText(balanceFilePath);
        StockBalanceWrapper jsonData = JsonUtility.FromJson<StockBalanceWrapper>(jsonString);

        GameObject gameObject = new GameObject();
        StocksPortfolio stocksPortfolioInstance = gameObject.AddComponent<StocksPortfolio>();
        Transact transactInstance = gameObject.AddComponent<Transact>();

        double totalStocksValuation = 0;

        foreach (StockBalance stock in jsonData.Balance)
        {
            FuturesMarket.MarketData filteredData = stocksPortfolioInstance.FilterData(stock.Ticker);

            if (stock.Quantity > 0)
            {
                double price = double.Parse(filteredData.Price);
                double quantity = stock.Quantity;
                double totalValue = price * quantity;

                totalStocksValuation += totalValue;
            }
        }

        playerManager.SetStockValue((float)totalStocksValuation);
    }
}
