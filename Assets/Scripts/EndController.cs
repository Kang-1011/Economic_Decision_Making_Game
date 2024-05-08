// All code was written by the team

using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Transact;

public class EndController : MonoBehaviour
{
    public PlayerManager playerManager;
    public TextMeshProUGUI netAssetValue;
    public TextMeshProUGUI initialAssetValue;
    public TextMeshProUGUI profitLossValue;

    public float totalAsset;
    public float netAsset;
    public float profit;
    public float initialAsset = 1000000.0f;

    public GameObject Futures;
    public GameObject Stocks;

    // Update is called once per frame
    void Update()
    {
        playerManager = PlayerManager.Instance;
        SellAllStocks();
        UpdateData();
        LoadFutures();
        LoadStocks();
    }

    public void SellAllStocks()
    {
        string balanceFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/StocksBalance.json");
        string jsonString = File.ReadAllText(balanceFilePath);
        StockBalanceWrapper jsonData = JsonUtility.FromJson<StockBalanceWrapper>(jsonString);

        GameObject gameObject = new GameObject();
        StocksPortfolio stocksPortfolioInstance = gameObject.AddComponent<StocksPortfolio>();
        Transact transactInstance = gameObject.AddComponent<Transact>();

        foreach (StockBalance stock in jsonData.Balance)
        {
            FuturesMarket.MarketData filteredData = stocksPortfolioInstance.FilterData(stock.Ticker);

            if (stock.Quantity > 0)
            {
                double price = double.Parse(filteredData.Price);
                double quantity = stock.Quantity;
                double totalValue =  price * quantity;
                OrderData order = new OrderData(3, stock.Ticker, price, quantity, totalValue, 0.0, 0.0);

                transactInstance.ModifyBalance(order, "Sell");
                PlayerManager.Instance.AddPlayerCash((float)totalValue);
            }
        }
    }

    public void UpdateData()
    {
        // Calculate the total asset value
        totalAsset = playerManager.GetPlayerCash() + playerManager.houseValue + playerManager.GetStockValue() + 
        playerManager.GetFuturesValue() + playerManager.GetBankDepositValue() + playerManager.GetBankFinanceValue();

        // Calculate the net asset value
        netAsset = totalAsset - playerManager.GetBankLoanValue();

        netAssetValue.text = "¥ " + netAsset.ToString("F2");   // Show net asset value

        initialAssetValue.text = "¥ 1000000"; // Show initial asset value

        // Calculate the profit/loss
        profit = netAsset - initialAsset;

        profitLossValue.text = "¥ " + profit.ToString("F2");   // Show profit/loss value
    }

    public void LoadFutures()
    {
        Text cost = Futures.transform.Find("Cost").GetComponent<Text>();
        Text pnl = Futures.transform.Find("RealizedPNL").GetComponent<Text>();

        string ordersFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/FuturesOrders.json");
        string jsonString = File.ReadAllText(ordersFilePath);

        if (jsonString == "")
        {
            cost.text = "￥0";
            pnl.text = "￥0";
        }
        else
        {
            OrderDataList orderDataList = JsonUtility.FromJson<OrderDataList>(jsonString);
            double totalCost = 0;
            double totalPnL = 0;

            foreach (OrderData order in orderDataList.Orders)
            {
                totalCost += order.TotalValue;
                totalPnL += order.PnL;
            }

            cost.text = $"￥{totalCost}";
            pnl.text = $"￥{totalPnL}";
        }
    }

    public void LoadStocks()
    {
        Text cost = Stocks.transform.Find("Cost").GetComponent<Text>();
        Text pnl = Stocks.transform.Find("RealizedPNL").GetComponent<Text>();

        string balanceFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/StocksBalance.json");
        string jsonString = File.ReadAllText(balanceFilePath);
        StockBalanceWrapper stockBalanceList = JsonUtility.FromJson<StockBalanceWrapper>(jsonString);

        double totalCost = 0;
        double totalPnL = 0;

        foreach (StockBalance stock in stockBalanceList.Balance)
        {
            totalCost += stock.Cost;
            totalPnL += stock.RealizedProfit;
        }

        totalPnL -= totalCost;

        cost.text = $"￥{totalCost}";
        pnl.text = $"￥{totalPnL}";
    }
}