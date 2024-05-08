// All code was written by the team

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static Transact;

public class FuturesMarket : MonoBehaviour
{
    private GameObject baseGameObject;
    private Transform entryContainer;
    private Transform entryTemplate;

    public ScrollViewText ScrollViewManager;

    private int chosenContract;

    public string jsonFilePath;

    MarketDataWrapper jsonData;

    // Class representing the structure of JSON data
    [System.Serializable]
    public class MarketData
    {
        public int Round;
        public int Id;
        public string Name;
        public string Ticker;
        public string Category;
        public string Price;
        public string Change;
        public string PercentageChange;
        public string Volume;
        public string MarketCap;
    }

    // Wrapper class to hold all the MarketData
    [System.Serializable]
    public class MarketDataWrapper
    {
        public List<MarketData> data;
    }

    public void DisplayFuturesMarketData()
    {
        baseGameObject = GameObject.Find("FuturesMarket");
        jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/Futures.json");
        LoadData();
    }

    public void DisplayStockMarketData()
    {
        baseGameObject = GameObject.Find("StockMarket");
        jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/Stocks.json");
        LoadData();
    }

    private void LoadData()
    {
        entryContainer = baseGameObject.transform.Find("EntryContainer");
        entryTemplate = entryContainer.Find("EntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        string jsonString = File.ReadAllText(jsonFilePath);

        jsonData = JsonUtility.FromJson<MarketDataWrapper>("{\"data\":" + jsonString + "}");

        float templateHeight = -80f;
        for (int i = 1; i <= 5; i++)
        {
            Transform clonedEntry = Instantiate(entryTemplate, entryContainer);
            RectTransform clonedEntryRectTransform = clonedEntry.GetComponent<RectTransform>();
            clonedEntryRectTransform.anchoredPosition = new Vector2(0, templateHeight * (i - 1));
            clonedEntry.gameObject.SetActive(true);

            MarketData filteredData = FilterData(jsonData.data, i, PlayerManager.Instance.GetRound());

            clonedEntry.Find("CategoryText (1)").GetComponent<Text>().text = filteredData.Category;
            clonedEntry.Find("TickerText (1)").GetComponent<Text>().text = filteredData.Ticker;
            clonedEntry.Find("NameText (1)").GetComponent<Text>().text = filteredData.Name;
            clonedEntry.Find("PriceText (1)").GetComponent<Text>().text = filteredData.Price;
            clonedEntry.Find("%ChangeText (1)").GetComponent<Text>().text = filteredData.PercentageChange;

            int index = i;
            clonedEntry.Find("ActionButton").GetComponent<Button>().onClick.AddListener(() => SelectContract(index));
        }
    }

    private void SelectContract(int i)
    {
        chosenContract = i;

        if (jsonFilePath == Path.Combine(Application.streamingAssetsPath, "Data/Securities/Futures.json"))
            DisplayFuturesTradeData();
        else
            DisplayCompanyInfo();
    }

    public int GetChosenContract()
    {
        return chosenContract;
    }

    public MarketDataWrapper GetJsonData()
    {
        return jsonData;
    }

    // Function to filter the data based on Id and Round
    public MarketData FilterData(List<MarketData> data, int id, int round)
    {
        foreach (var item in data)
        {
            if (item.Id == id && item.Round == round)
            {
                return item;
            }
        }
        return null;
    }

    public void DisplayFuturesTradeData()
    {
        baseGameObject = GameObject.Find("FuturesTrade");
        LoadTradeData();
    }

    public void DisplayStockTradeData()
    {
        baseGameObject = GameObject.Find("StockTrade");
        LoadTradeData();
    }

    // Load details of the selected contract/stock
    private void LoadTradeData()
    {
        MarketData filteredData = FilterData(jsonData.data, GetChosenContract(), PlayerManager.Instance.GetRound());
        baseGameObject.transform.Find("NameText").GetComponent<Text>().text = filteredData.Name;
        baseGameObject.transform.Find("TickerText").GetComponent<Text>().text = filteredData.Ticker;
        baseGameObject.transform.Find("CategoryText (1)").GetComponent<Text>().text = filteredData.Category;
        baseGameObject.transform.Find("PriceText (1)").GetComponent<Text>().text = filteredData.Price;
        baseGameObject.transform.Find("ChangeText (1)").GetComponent<Text>().text = filteredData.Change;
        baseGameObject.transform.Find("%ChangeText (1)").GetComponent<Text>().text = filteredData.PercentageChange;
        baseGameObject.transform.Find("VolumeText (1)").GetComponent<Text>().text = filteredData.Volume;
        baseGameObject.transform.Find("PriceInput").GetComponent<InputField>().text = filteredData.Price;

        baseGameObject.transform.Find("QuantityInput").GetComponent<InputField>().text = "";
        baseGameObject.transform.Find("SLInput").GetComponent<InputField>().text = "";
        baseGameObject.transform.Find("TPInput").GetComponent<InputField>().text = "";
        baseGameObject.transform.Find("TxStatusText").GetComponent<Text>().text = "";

        if (jsonFilePath == Path.Combine(Application.streamingAssetsPath, "Data/Securities/Stocks.json"))
        {
            baseGameObject.transform.Find("MarketCapText (1)").GetComponent<Text>().text = filteredData.MarketCap;

            string balanceFilePath = Path.Combine(Application.streamingAssetsPath, "Data/Securities/StocksBalance.json");
            string jsonString = File.ReadAllText(balanceFilePath);
            StockBalanceWrapper stockBalanceList = JsonUtility.FromJson<StockBalanceWrapper>(jsonString);

            foreach (StockBalance stock in stockBalanceList.Balance)
            {
                if (stock.Ticker == filteredData.Ticker)
                {
                    baseGameObject.transform.Find("Balance").GetComponent<Text>().text = stock.Quantity.ToString();
                }
            }

            baseGameObject.transform.Find("SLInput").GetComponent<InputField>().interactable = false;
            baseGameObject.transform.Find("TPInput").GetComponent<InputField>().interactable = false;
        }
        else
        {
            baseGameObject.transform.Find("SettlementText (1)").GetComponent<Text>().text = "Round " + filteredData.Ticker.Substring(3);
        }
    }

    public void DisplayCompanyInfo()
    {
        MarketData filteredData = FilterData(jsonData.data, GetChosenContract(), PlayerManager.Instance.GetRound());

        string fileName = filteredData.Name;

        GameObject stockCompanyInfoGameObject = GameObject.Find("StockCompanyInfo");
        Transform stockCompanyInfoTitleText = stockCompanyInfoGameObject.transform.Find("TitleText");
        stockCompanyInfoTitleText.GetComponent<Text>().text = filteredData.Name;

        string companyInfoFilePath = Path.Combine(Application.streamingAssetsPath, "Text/CompanyInfo/" + fileName + ".txt");
        ScrollViewManager.ShowText(companyInfoFilePath);
        
        GameObject companyContinueButton = GameObject.Find("ContinueButton");
        companyContinueButton.GetComponent<Button>().onClick.AddListener(() => DisplayStockTradeData());
    }
}
