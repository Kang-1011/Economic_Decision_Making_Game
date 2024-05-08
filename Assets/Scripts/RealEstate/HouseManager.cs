// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseManager : MonoBehaviour
{
    public int currentRound;
    public HousePriceManager housePriceManager;
    public AssetValueController assetValueController;
    public float profitAndLoss;

    public GameObject background;
    public GameObject[] housePrefabs;
    public GameObject sellCanvas;
    public Button OKBtn;
    public Text profitAndLossText;
    public Text houseTitleText;
    public Text purchasePriceText;
    public Text marketPriceText;
    public Text changeText;
    public GameObject upImage;
    public GameObject downImage;
    public Text profitText;
    public Text lossText;

    public void Start()
    {
        currentRound = PlayerManager.Instance.GetRound();
        OKBtn.onClick.AddListener(OnOKButtonClick);
        DisableAllHousePrefabs();
        EnablePurchasedHousePrefabs();
    }

    public void DisableAllHousePrefabs()
    {
        background.SetActive(false);
        foreach (GameObject housePrefab in housePrefabs)
        {
            housePrefab.SetActive(false);
        }
    }

    public void EnablePurchasedHousePrefabs()
    {
        int currentHouseIndex;
        for (currentHouseIndex = 0; currentHouseIndex < House.purchasedHouses.Count; currentHouseIndex++)
        {
            background.SetActive(true);
            House currentHouse = House.purchasedHouses[currentHouseIndex];
            GameObject currentHousePrefab = housePrefabs[currentHouseIndex];
            currentHousePrefab.SetActive(true);

            houseTitleText = currentHousePrefab.transform.Find("Title")?.GetComponent<Text>();
            purchasePriceText = currentHousePrefab.transform.Find("PurchasePrice")?.GetComponent<Text>();
            marketPriceText = currentHousePrefab.transform.Find("MarketPrice")?.GetComponent<Text>();
            changeText = currentHousePrefab.transform.Find("Change")?.GetComponent<Text>();
            upImage = currentHousePrefab.transform.Find("Up")?.gameObject;
            downImage = currentHousePrefab.transform.Find("Down")?.gameObject;

            houseTitleText.text = currentHouse.title;
            purchasePriceText.text = currentHouse.purchasePrice.ToString() + "/m2";
            switch (currentHouse.title)
            {
                case "House A":
                    currentHouse.marketPrice = housePriceManager.prices[currentRound - 1][0];
                    break;
                case "House B":
                    currentHouse.marketPrice = housePriceManager.prices[currentRound - 1][1];
                    break;
                case "House C":
                    currentHouse.marketPrice = housePriceManager.prices[currentRound - 1][2];
                    break;
                case "House D":
                    currentHouse.marketPrice = housePriceManager.prices[currentRound - 1][3];
                    break;
                default:
                    Debug.LogError("Invalid House Identifier!");
                    break;
            }

            marketPriceText.text = currentHouse.marketPrice.ToString() + "/m2";
            currentHouse.percentage_change = ((currentHouse.marketPrice - currentHouse.purchasePrice) / currentHouse.purchasePrice) * 100;

            if (currentHouse.percentage_change >= 0)
            {
                downImage.SetActive(false);
                changeText.text = currentHouse.percentage_change.ToString("F2") + "%";
            }
            else
            {
                upImage.SetActive(false);
                changeText.text = Mathf.Abs(currentHouse.percentage_change).ToString("F2") + "%";
            }
        }
    }

    public void SellHouse(int idx)
    {
        sellCanvas.SetActive(true);
        House house = House.purchasedHouses[idx];
        House.purchasedHousesHistory[House.purchasedHousesHistory.IndexOf(house)].sellRound = currentRound;
        House.purchasedHousesHistory[House.purchasedHousesHistory.IndexOf(house)].sellPrice = house.marketPrice;
        profitAndLoss = (house.marketPrice - house.purchasePrice) * house.area;
        House.purchasedHousesHistory[House.purchasedHousesHistory.IndexOf(house)].profitAndLoss = profitAndLoss;
        profitText = sellCanvas.transform.Find("ProfitText")?.GetComponent<Text>();
        lossText = sellCanvas.transform.Find("LossText")?.GetComponent<Text>();
        if (profitAndLoss >= 0)
        {
            lossText.enabled = false;
        }
        else
        {
            profitText.enabled = false;
        }
        profitAndLossText.text = Mathf.Abs(profitAndLoss).ToString("F0") + "!";
        PlayerManager.Instance.playerCash += house.marketPrice * house.area;
        House.purchasedHouses.RemoveAt(idx);
        DisableAllHousePrefabs();
        EnablePurchasedHousePrefabs();

        PlayerManager.Instance.UpdateHouseValue();
        PlayerManager.Instance.UpdateMonthlyRentIncome();
        assetValueController.UpdateData();
    }

    private void OnOKButtonClick()
    {
        sellCanvas.SetActive(false);
    }
}
