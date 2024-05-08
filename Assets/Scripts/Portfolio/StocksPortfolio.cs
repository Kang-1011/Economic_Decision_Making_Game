// All code was written by the team

using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static Transact;

public class StocksPortfolio : MonoBehaviour
{
    public int round;

    private void Awake()
    {
        round = PlayerManager.Instance.GetRound();
    }

    // Load stocks portfolio
    public void LoadStocksPortfolio()
    {
        Transform entryContainer = transform.Find("EntryContainer");
        Transform entryTemplate = entryContainer.Find("EntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        string balanceFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/StocksBalance.json");
        string jsonString = File.ReadAllText(balanceFilePath);
        StockBalanceWrapper jsonData = JsonUtility.FromJson<StockBalanceWrapper>(jsonString);

        float templateHeight = -90f;
        int i = 0;
        foreach (StockBalance stock in jsonData.Balance)
        {
            i++;

            Transform clonedEntry = Instantiate(entryTemplate, entryContainer);
            RectTransform clonedEntryRectTransform = clonedEntry.GetComponent<RectTransform>();
            clonedEntryRectTransform.anchoredPosition = new Vector2(0, templateHeight * (i - 1));
            clonedEntry.gameObject.SetActive(true);

            FuturesMarket.MarketData filteredData = FilterData(stock.Ticker);

            clonedEntry.Find("TickerText").GetComponent<Text>().text = stock.Ticker;
            clonedEntry.Find("NameText").GetComponent<Text>().text = filteredData.Name;
            clonedEntry.Find("BalanceText").GetComponent<Text>().text = stock.Quantity.ToString();
            clonedEntry.Find("PriceText").GetComponent<Text>().text = filteredData.Price;

            double totalValue = stock.Quantity * float.Parse(filteredData.Price);
            clonedEntry.Find("TotalValueText").GetComponent<Text>().text = totalValue.ToString("0.00");
        }

        // Destroy template game object
        foreach (Transform child in entryContainer)
        {
            if (child.gameObject.name == "EntryTemplate")
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    // Get the current market data for param @ticker
    public FuturesMarket.MarketData FilterData(string ticker)
    {
        string stockMarketFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/Stocks.json");
        string jsonString = File.ReadAllText(stockMarketFilePath);
        FuturesMarket.MarketDataWrapper jsonData = JsonUtility.FromJson<FuturesMarket.MarketDataWrapper>("{\"data\":" + jsonString + "}");

        foreach (var item in jsonData.data)
        {
            if (item.Ticker == ticker && item.Round == round)
            {
                return item;
            }
        }
        return null;
    }
}
