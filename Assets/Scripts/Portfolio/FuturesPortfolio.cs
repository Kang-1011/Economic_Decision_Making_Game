// All code was written by the team

using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FuturesPortfolio : MonoBehaviour
{
    Transform columnTitle;
    Transform noOngoingContractText;
    Transform futuresPortfolioScrollView;
    Transform contractDetails;

    Transform viewport;
    Transform content;
    Transform entryTemplate;

    string ordersFilePath;
    string jsonString;
    OrderDataList orderDataList;
    OrderData[] orders;

    private void Awake()
    {
        ordersFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/FuturesOrders.json");
        jsonString = File.ReadAllText(ordersFilePath);

        // If FuturesOrders.json is empty, do not read
        if (jsonString == "")
        {
            return;
        }

        orderDataList = JsonUtility.FromJson<OrderDataList>(jsonString);
        orders = orderDataList.Orders;
    }

    // Start is called before the first frame update
    public void LoadFuturesPortfolio()
    {
        columnTitle = transform.Find("ColumnTitle");
        noOngoingContractText = transform.Find("NoOngoingContractText");
        futuresPortfolioScrollView = transform.Find("FuturesPortfolioScrollView");
        contractDetails = transform.Find("ContractDetails");

        viewport = futuresPortfolioScrollView.Find("Viewport");
        content = viewport.Find("Content");
        entryTemplate = content.Find("Entry");
        entryTemplate.gameObject.SetActive(false);

        // 'No Ongoing Contract' text should be hidden in the beginnning
        noOngoingContractText.gameObject.SetActive(false);

        // Initialise the visibility/interactibility of game objects in futures portfolio
        FuturesPortfolioNavigation(true);

        // If FuturesOrders.json is empty, show 'No Ongoing Contract' text
        if (jsonString == "")
        {
            ShowNoOngoingContractText(true);
            return;
        }

        // Clear all children object under scrollview's content before adding new child object (new entry) to ensure scroll view working fine
        ClearAllChildren();

        RectTransform entryRectTransform = entryTemplate.GetComponent<RectTransform>();
        float entryHeight = entryRectTransform.rect.height;
        float entryXPos = entryRectTransform.anchoredPosition.x;
        float entryInitialYPos = entryRectTransform.anchoredPosition.y;

        // Difference in yPos between entries in the scroll view
        float heightDecline = -90f;
        int entryCount = 0;
        foreach (OrderData order in orders)
        {
            // Only show ongoing trade(contract)
            if (order.Status == "Ongoing")
            {
                Transform clonedEntry = Instantiate(entryTemplate, content);
                RectTransform clonedEntryRectTransform = clonedEntry.GetComponent<RectTransform>();
                clonedEntryRectTransform.anchoredPosition = new Vector2(entryXPos, entryInitialYPos + entryCount * heightDecline);
                clonedEntry.gameObject.SetActive(true);

                FuturesMarket.MarketData filteredData = FilterData(order.Ticker.Substring(0, 3));

                Text directionText = clonedEntry.Find("DirectionText").GetComponent<Text>();
                directionText.text = order.TradeType;
                if (order.TradeType == "Long")
                {
                    directionText.color = ColorGenerator(22, 113, 76);
                }
                else if (order.TradeType == "Short")
                {
                    directionText.color = ColorGenerator(237, 85, 59);
                }

                clonedEntry.Find("NameText").GetComponent<Text>().text = filteredData.Name;
                clonedEntry.Find("QuantityText").GetComponent<Text>().text = ((int)order.Quantity).ToString();

                double priceChangePerContract = double.Parse(filteredData.Price) - order.EntryPrice;
                double uPnL = priceChangePerContract * order.Quantity;

                Text uPnLText = clonedEntry.Find("UPNLText").GetComponent<Text>();
                redGreenColorText(uPnLText, uPnL);

                OrderData orderInstance = order;
                clonedEntry.Find("TradeButton").GetComponent<Button>().onClick.AddListener(() => ViewTradeDetails(orderInstance));

                entryCount++;
            }
        }

        // Resize game object 'content' under scroll view to perfectly fix the size of all entries
        RectTransform contentRectTransform = content.GetComponent<RectTransform>();
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, entryHeight * entryCount - ((entryHeight + heightDecline) / 2) * entryCount);

        // If all contracts have expired
        if (entryCount == 0)
        {
            ShowNoOngoingContractText(true);
            return;
        }
    }

    // Clear all children object under scrollview's content
    private void ClearAllChildren()
    {
        foreach (Transform child in content)
        {
            if (child.gameObject.name != "Entry")
            {
                Destroy(child.gameObject);
            }
        }
    }

    // Get the current market data of param @ticker
    public FuturesMarket.MarketData FilterData(string ticker)
    {
        string futuresMarketFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/Futures.json");
        string jsonString = File.ReadAllText(futuresMarketFilePath);
        FuturesMarket.MarketDataWrapper jsonData = JsonUtility.FromJson<FuturesMarket.MarketDataWrapper>("{\"data\":" + jsonString + "}");

        foreach (var item in jsonData.data)
        {
            if (item.Ticker.Substring(0, 3) == ticker && item.Round == PlayerManager.Instance.GetRound())
            {
                return item;
            }
        }
        return null;
    }

    public Color ColorGenerator(int red, int green, int blue)
    {
        red = Mathf.Clamp(red, 0, 255);
        green = Mathf.Clamp(green, 0, 255);
        blue = Mathf.Clamp(blue, 0, 255);

        return new Color(red / 255f, green / 255f, blue / 255f);
    }

    // Set the color of param @textUI to green if param @num if positive, red if negative
    private void redGreenColorText(Text textUI, double num)
    {
        textUI.text = "+" + num.ToString("0.00");

        if (num > 0)
        {
            textUI.color = ColorGenerator(22, 113, 76);
        }
        else if (num < 0)
        {
            textUI.color = ColorGenerator(237, 85, 59);
            textUI.text = num.ToString("0.00");
        }
    }

    // Show 'No Ongoing Contract' text and hide other unnecessary game objects
    public void ShowNoOngoingContractText(bool boolean)
    {
        noOngoingContractText.gameObject.SetActive(boolean);
        columnTitle.gameObject.SetActive(!boolean);
        futuresPortfolioScrollView.gameObject.SetActive(!boolean);
    }

    // Navigate between 1st page & 2nd page of futures portfolio 
    public void FuturesPortfolioNavigation(bool boolean)
    {
        columnTitle.gameObject.SetActive(boolean);
        futuresPortfolioScrollView.gameObject.SetActive(boolean);
        contractDetails.gameObject.SetActive(!boolean);
        contractDetails.Find("CloseTradeButton").gameObject.SetActive(true);
        contractDetails.Find("ClosedText").gameObject.SetActive(false);
    }

    // Load contract details on 2nd page of futures portfolio
    public void ViewTradeDetails(OrderData orderInstance)
    {
        FuturesPortfolioNavigation(false);
        contractDetails.Find("ReturnButton").GetComponent<Button>().onClick.AddListener(() => LoadFuturesPortfolio());

        FuturesMarket.MarketData filteredData = FilterData(orderInstance.Ticker.Substring(0, 3));

        contractDetails.Find("Name(Ticker)Text").GetComponent<Text>().text = filteredData.Name + "(" + filteredData.Ticker + ")";
        contractDetails.Find("Direction").GetComponent<Text>().text = orderInstance.TradeType;

        contractDetails.Find("SettlementText").GetComponent<Text>().text = "Round " + orderInstance.Ticker.Substring(3);
        contractDetails.Find("QuantityText").GetComponent<Text>().text = ((int)orderInstance.Quantity).ToString();
        contractDetails.Find("EntryPriceText").GetComponent<Text>().text = orderInstance.EntryPrice.ToString("0.00");
        contractDetails.Find("ContractValueText").GetComponent<Text>().text = orderInstance.TotalValue.ToString("0.00");

        double priceChange = double.Parse(filteredData.Price) - orderInstance.EntryPrice;
        double valueChange = priceChange * orderInstance.Quantity;

        double percentageChange = priceChange / orderInstance.EntryPrice * 100;
        percentageChange = Math.Round(percentageChange * 100f) / 100f;

        Text lastPriceText = contractDetails.Find("LastPriceText").GetComponent<Text>();
        redGreenColorText(lastPriceText, priceChange);
        lastPriceText.text = filteredData.Price + "(" + lastPriceText.text + ")";

        double currentValue = float.Parse(filteredData.Price) * orderInstance.Quantity;
        Text currentValueText = contractDetails.Find("CurrentValueText").GetComponent<Text>();
        redGreenColorText(currentValueText, valueChange);
        currentValueText.text = currentValue.ToString("0.00") + "(" + currentValueText.text + ")";

        Text ROIText = contractDetails.Find("ROIText").GetComponent<Text>();
        redGreenColorText(ROIText, percentageChange);
        ROIText.text += "%";

        Text tpText = contractDetails.Find("TPText").GetComponent<Text>();
        NAText(tpText, orderInstance.Tp);
        Text slText = contractDetails.Find("SLText").GetComponent<Text>();
        NAText(slText, orderInstance.Sl);

        contractDetails.Find("CloseTradeButton").GetComponent<Button>().onClick.AddListener(() => CloseTrade(orderInstance, filteredData, "MarketPrice"));
    }

    // Display 'N/A' if take-profit/stop-loss trigger price is not set
    private void NAText(Text textUI, double triggerPrice)
    {
        if (triggerPrice == 0.0)
        {
            textUI.text = "N/A";
        }
        else
        {
            textUI.text = triggerPrice.ToString("0.00");
        }
    }

    // Close ongoing trade according to param @orderInstance
    public void CloseTrade(OrderData orderInstance, FuturesMarket.MarketData filteredData, string exitType)
    {
        foreach (OrderData order in orders)
        {
            if (order == orderInstance)
            {
                order.Status = "Closed";
                
                if (exitType == "MarketPrice")
                {
                    order.ExitPrice = double.Parse(filteredData.Price);
                }

                double exitValue = order.ExitPrice * orderInstance.Quantity;
                exitValue = Math.Round(exitValue * 100f) / 100f;
                order.ExitValue = exitValue;

                double pnl = (orderInstance.ExitPrice - orderInstance.EntryPrice) * orderInstance.Quantity;
                pnl = Math.Round(pnl * 100f) / 100f;
                order.PnL = pnl;

                // Modify player's total cash according to contract type (Long/Short)
                if (order.TradeType == "Short")
                {
                    order.PnL *= -1;
                    PlayerManager.Instance.SubtractPlayerCash((float)exitValue);
                }
                else
                {
                    PlayerManager.Instance.AddPlayerCash((float)exitValue);
                }                
            }
        }

        string updatedJson = JsonUtility.ToJson(new OrderDataList(orders), true);
        File.WriteAllText(ordersFilePath, updatedJson);
    }

    // Automatically settle ongoing contract if expiration round is reached / tp is hit / sl is hit
    public void CheckSettlement()
    {
        // No ongoing contract
        if (jsonString == "")
        {
            return;
        }

        foreach (OrderData order in orders)
        {
            if (order.Status == "Ongoing")
            {
                FuturesMarket.MarketData filteredData = FilterData(order.Ticker.Substring(0, 3));
                if (int.Parse(order.Ticker.Substring(3)) == PlayerManager.Instance.GetRound())
                {
                    CloseTrade(order, filteredData, "MarketPrice");
                    initiateNotification("expired", order, filteredData);
                    continue;
                }

                if (order.TradeType == "Long")
                {
                    if (order.Sl >= double.Parse(filteredData.Price) && order.Sl != 0)
                    {
                        order.ExitPrice = order.Sl;
                        CloseTrade(order, filteredData, "TriggerPrice");
                        initiateNotification("sl", order, filteredData);
                        continue;
                    }

                    if (order.Tp <= double.Parse(filteredData.Price) && order.Tp != 0)
                    {
                        order.ExitPrice = order.Tp;
                        CloseTrade(order, filteredData, "TriggerPrice");
                        initiateNotification("tp", order, filteredData);
                        continue;
                    }
                }
                else if (order.TradeType == "Short")
                {
                    if (order.Sl <= double.Parse(filteredData.Price) && order.Sl != 0)
                    {
                        order.ExitPrice = order.Sl;
                        CloseTrade(order, filteredData, "TriggerPrice");
                        initiateNotification("sl", order, filteredData);
                        continue;
                    }

                    if (order.Tp >= double.Parse(filteredData.Price) && order.Tp != 0)
                    {
                        order.ExitPrice = order.Tp;
                        CloseTrade(order, filteredData, "TriggerPrice");
                        initiateNotification("tp", order, filteredData);
                        continue;
                    }
                }
            }
        }
    }

    // Create a news/notification with details of the settled contract
    private void initiateNotification(string type, OrderData order, FuturesMarket.MarketData filteredData)
    {
        string message = "";

        switch (type)
        {
            case "expired":
                message = $"Futures contract for {order.Ticker} has now expired and is settled at market price of {order.ExitPrice:0.00}";
                break;
            case "sl":
                message = $"Stop-loss trigger for futures contract position in {order.Ticker} has been hit and is settled at trigger price of {order.ExitPrice:0.00}";
                break;
            case "tp":
                message = $"Take-profit trigger for futures contract position in {order.Ticker} has been hit and is settled at trigger price of {order.ExitPrice:0.00}";
                break;
        }

        string explanation = $"Name: {filteredData.Name}\n" +
                        $"Ticker: {order.Ticker}\n" +
                        $"Settlement Round: Round {order.Ticker.Substring(3)}\n" +
                        $"Direction: {order.TradeType}\n" +
                        $"Quantity: {order.Quantity}\n" +
                        $"Entry Price: {order.EntryPrice:0.00}\n" +
                        $"Exit Price: {order.ExitPrice:0.00}\n" +
                        $"Profit & Loss: {order.PnL:0.00}\n" +
                        $"Status: Settled\n";

        GameObject gameObject = new GameObject();
        NewsManager newsManager = gameObject.AddComponent<NewsManager>();
        newsManager.SaveToLog(new News(PlayerManager.Instance.GetRound(), message, explanation));
    }
}