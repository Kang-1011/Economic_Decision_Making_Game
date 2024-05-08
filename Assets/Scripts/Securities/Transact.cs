// All code was written by the team

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Transact : MonoBehaviour
{
    private GameObject baseGameObject;
    private Text statusText;

    private string ticker;
    private double entryPrice;
    private double quantity;
    private double totalValue;
    private double sl;
    private double tp;

    // File path to save the order history
    public string orderFilePath;

    // Futures(Long/Short) / Stocks (Buy/Sell)
    private int executionMode;

    public void SetExecutionMode(int mode)
    {
        orderFilePath = Path.Combine(Application.streamingAssetsPath, orderFilePath);
        executionMode = mode;
        ExecuteOrders();
    }

    public void ExecuteOrders()
    {
        string[] modeText = { "FuturesTrade", "FuturesTrade", "StockTrade", "StockTrade" };
        baseGameObject = GameObject.Find(modeText[executionMode]);
        statusText = baseGameObject.transform.Find("TxStatusText").GetComponent<Text>();

        GetOrderDetails();

        ValidateOrders();
    }

    private void GetOrderDetails()
    {
        ticker = baseGameObject.transform.Find("TickerText").GetComponent<Text>().text;
        entryPrice = double.Parse(baseGameObject.transform.Find("PriceText (1)").GetComponent<Text>().text);

        if (string.IsNullOrEmpty(baseGameObject.transform.Find("QuantityInput").GetComponent<InputField>().text))
        {
            statusText.color = Color.red;
            statusText.text = "Please fill the quantity field";
            return;
        }
        else
        {
            quantity = double.Parse(baseGameObject.transform.Find("QuantityInput").GetComponent<InputField>().text);
        }

        totalValue = double.Parse(baseGameObject.transform.Find("ValueInput").GetComponent<InputField>().text);

        string[] inputFields = { "TPInput", "SLInput" };
        foreach (string text in inputFields){
            if (string.IsNullOrEmpty(baseGameObject.transform.Find(text).GetComponent<InputField>().text))
            {
                if (text == "TPInput")
                {
                    tp = 0;
                }
                else
                {
                    sl = 0;
                }
            }
            else
            {
                if (text == "TPInput")
                {
                    tp = double.Parse(baseGameObject.transform.Find(text).GetComponent<InputField>().text);
                }
                else
                {
                    sl = double.Parse(baseGameObject.transform.Find(text).GetComponent<InputField>().text);
                }
            }
        }
    }

    // Validate the content in each input field
    private void ValidateOrders()
    {
        if (!string.IsNullOrEmpty(baseGameObject.transform.Find("QuantityInput").GetComponent<InputField>().text))
        {
            string[] modeText = { "longed", "shorted", "bought", "sold" };
            double newBalance;

            // 0 = Long(Futures), 1 = Short(Futures), 2 = Buy(Stocks) 
            if (executionMode == 0 || executionMode == 1 || executionMode == 2)
            {
                if (totalValue < PlayerManager.Instance.GetPlayerCash())
                {
                    if ((executionMode == 0) || (executionMode == 2))
                    {
                        if (tp == 0)
                        {

                        }
                        else
                        {
                            if (tp <= entryPrice)
                            {
                                statusText.color = Color.red;
                                statusText.text = "Take profit trigger price should be higher than the entry price in a long order";
                                return;
                            }
                        }

                        if (sl == 0)
                        {

                        }
                        else
                        {
                            if (sl >= entryPrice)
                            {
                                statusText.color = Color.red;
                                statusText.text = "Stop loss trigger price should be lower than the entry price in a long order";
                                return;
                            }
                        }
                    }
                    else if (executionMode == 1)
                    {
                        if (tp == 0)
                        {

                        }
                        else
                        {
                            if (tp >= entryPrice)
                            {
                                statusText.color = Color.red;
                                statusText.text = "Take profit trigger price should be lower than the entry price in a short order";
                                return;
                            }
                        }

                        if (sl == 0)
                        {

                        }
                        else
                        {
                            if (sl <= entryPrice)
                            {
                                statusText.color = Color.red;
                                statusText.text = "Stop loss trigger price should be higher than the entry price in a short order";
                                return;
                            }
                        }
                    }

                    statusText.color = Color.black;
                    statusText.text = $"Successfully {modeText[executionMode]} {quantity} {ticker} at {entryPrice}";

                    OrderData order = new OrderData(executionMode, ticker, entryPrice, quantity, totalValue, sl, tp);

                    SaveOrder(order);

                    if (order.TradeType == "Buy")
                    {
                        ModifyBalance(order, "Buy");

                        // Update remaining balance on StockTrade
                        newBalance = double.Parse(baseGameObject.transform.Find("Balance").GetComponent<Text>().text) + quantity;
                        baseGameObject.transform.Find("Balance").GetComponent<Text>().text = $"{newBalance}";
                    }

                    if (order.TradeType == "Short")
                    {
                        PlayerManager.Instance.AddPlayerCash((float)totalValue);
                    }
                    else
                    {
                        PlayerManager.Instance.SubtractPlayerCash((float)totalValue);
                    }                    
                }
                else
                {
                    statusText.color = Color.red;
                    statusText.text = $"Insufficient account balance";
                    return;
                }
            }
            else if (executionMode == 3) // 3 = Sell(Stocks)
            {
                // Show warning text if the quantity selling is greater than available balance
                if (quantity > double.Parse(baseGameObject.transform.Find("Balance").GetComponent<Text>().text)){
                    statusText.color = Color.red;
                    statusText.text = $"Insufficient account balance";
                    return;
                }
                else
                {
                    statusText.color = Color.black;
                    statusText.text = $"Successfully {modeText[executionMode]} {quantity} {ticker} at {entryPrice}";

                    OrderData order = new OrderData(executionMode, ticker, entryPrice, quantity, totalValue, 0.0, 0.0);

                    SaveOrder(order);

                    if (order.TradeType == "Sell")
                    {
                        ModifyBalance(order, "Sell");
                    }
                    PlayerManager.Instance.AddPlayerCash((float)totalValue);

                    // Update remaining balance on StockTrade page
                    newBalance = double.Parse(baseGameObject.transform.Find("Balance").GetComponent<Text>().text) - quantity;
                    baseGameObject.transform.Find("Balance").GetComponent<Text>().text = $"{newBalance}";
                }
            }
        }
    }

    public void SaveOrder(OrderData order)
    {
        OrderData[] orders;

        // Check if the file exists
        if (File.Exists(orderFilePath))
        {
            string jsonString = File.ReadAllText(orderFilePath);

            // File exist but empty
            if (jsonString == "")
            {
                orders = new OrderData[0];
            }
            else
            {
                OrderDataList orderDataList = JsonUtility.FromJson<OrderDataList>(jsonString);
                orders = orderDataList.Orders;
            }
        }
        else
        {
            // If the file does not exist, create a new empty array
            orders = new OrderData[0];
        }

        // Increase the size of the records array by one
        Array.Resize(ref orders, orders.Length + 1);

        // Add the new record to the last index of the array
        orders[orders.Length - 1] = order;

        string updatedJson = JsonUtility.ToJson(new OrderDataList(orders), true);

        File.WriteAllText(orderFilePath, updatedJson);
    }
    
    [System.Serializable]
    public class StockBalance
    {
        public string Ticker;
        public double Quantity;
        public double Cost;
        public double RealizedProfit;
    }

    [System.Serializable]
    public class StockBalanceWrapper
    {
        public List<StockBalance> Balance;
    }

    // Save to stock balance
    public void ModifyBalance(OrderData order, string orderType)
    {
        string balanceFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/StocksBalance.json");

        string jsonString = File.ReadAllText(balanceFilePath);

        StockBalanceWrapper stockBalanceList = JsonUtility.FromJson<StockBalanceWrapper>(jsonString);
        
        if (orderType == "Buy")
        {
            foreach (StockBalance stock in stockBalanceList.Balance)
            {
                if (stock.Ticker == order.Ticker)
                {
                    stock.Cost += order.TotalValue;
                    stock.Quantity += order.Quantity;
                }
            }
        }
        else if (orderType == "Sell")
        {
            foreach (StockBalance stock in stockBalanceList.Balance)
            {
                if (stock.Ticker == order.Ticker)
                {
                    stock.RealizedProfit += order.TotalValue;
                    stock.Quantity -= order.Quantity;
                }
            }
        }
        
        string updatedJson = JsonUtility.ToJson(stockBalanceList, true);
        File.WriteAllText(balanceFilePath, updatedJson);
    }
}