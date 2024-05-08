// All code was written by the team

using UnityEngine;

public class SecuritiesSceneInitialization : MonoBehaviour
{
    private GameObject futuresMarket;
    private GameObject futuresTrade;
    private GameObject futuresInfo;

    private GameObject stockMarket;
    private GameObject stockTrade;
    private GameObject stockInfo;
    private GameObject stockCompanyInfo;

    // Ensure the initial visibility of game objects in securities trading scene
    void Start()
    {
        futuresMarket = GameObject.Find("FuturesMarket");
        futuresMarket.SetActive(false);

        futuresInfo = GameObject.Find("FuturesInfo");
        futuresInfo.SetActive(false);

        futuresTrade = GameObject.Find("FuturesTrade");
        futuresTrade.SetActive(false);

        stockMarket = GameObject.Find("StockMarket");
        stockMarket.SetActive(false);

        stockInfo = GameObject.Find("StockInfo");
        stockInfo.SetActive(false);

        stockTrade = GameObject.Find("StockTrade");
        stockTrade.SetActive(false);

        stockCompanyInfo = GameObject.Find("StockCompanyInfo");
        stockCompanyInfo.SetActive(false);
    }
}
