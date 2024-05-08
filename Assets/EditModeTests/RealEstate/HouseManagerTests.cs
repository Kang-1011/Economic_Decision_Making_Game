// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class HouseManagerTests
{
    private HouseManager houseManager;
    private PlayerManager playerManager;

    [SetUp]
    public void Setup()
    {

        GameObject gameObject = new GameObject();
        houseManager = gameObject.AddComponent<HouseManager>();

        GameObject gameObject2 = new GameObject();
        houseManager.housePriceManager = gameObject2.AddComponent<HousePriceManager>();
        houseManager.housePriceManager.LoadPrices("prices");

        GameObject gameObject3 = new GameObject();
        playerManager = gameObject3.AddComponent<PlayerManager>();
        // playerManager.Awake();

        House.purchasedHouses = new List<House>();
        House.purchasedHousesHistory = new List<House>();

        houseManager.background = new GameObject();
        houseManager.housePrefabs = new GameObject[4];
        for (int i = 0; i < houseManager.housePrefabs.Length; i++)
        {
            houseManager.housePrefabs[i] = new GameObject();
            houseManager.houseTitleText = CreateTextComponent("", houseManager.housePrefabs[i].transform, "Title");
            houseManager.purchasePriceText = CreateTextComponent("", houseManager.housePrefabs[i].transform, "PurchasePrice");
            houseManager.marketPriceText = CreateTextComponent("", houseManager.housePrefabs[i].transform, "MarketPrice");
            houseManager.changeText = CreateTextComponent("", houseManager.housePrefabs[i].transform, "Change");
            houseManager.upImage = CreateChildGameObject(houseManager.housePrefabs[i].transform, "Up");
            houseManager.downImage = CreateChildGameObject(houseManager.housePrefabs[i].transform, "Down");
        }
        houseManager.sellCanvas = new GameObject();
        houseManager.profitText = CreateTextComponent("", houseManager.sellCanvas.transform, "ProfitText");
        houseManager.lossText = CreateTextComponent("", houseManager.sellCanvas.transform, "LossText");
        houseManager.profitAndLossText = CreateTextComponent("", houseManager.sellCanvas.transform, "ProfitAndLossText");
    }

    [Test]
    public void NoPurchasedHouses_DisableAllHousePrefabs()
    {
        houseManager.DisableAllHousePrefabs();
        Assert.IsFalse(houseManager.background.activeSelf);
        foreach (GameObject housePrefab in houseManager.housePrefabs)
        {
            Assert.IsFalse(housePrefab.activeSelf);
        }
    }

    [Test]
    public void OnePurchasedHouse_EnablePurchasedHousePrefabs()
    {
        houseManager.currentRound = 12;
        houseManager.background.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            houseManager.housePrefabs[i].SetActive(false);
        }

        House house = new House(1, "House A", 60, 23000, 276000, 6619);
        houseManager.EnablePurchasedHousePrefabs();

        Assert.IsTrue(houseManager.background.activeSelf);
        Assert.IsTrue(houseManager.housePrefabs[0].activeSelf);
        for (int i = 1; i <= 3; i++)
        {
            Assert.IsFalse(houseManager.housePrefabs[i].activeSelf);
        }
        Assert.AreEqual("House A", houseManager.houseTitleText.text);
        Assert.AreEqual("23000/m2", houseManager.purchasePriceText.text);
        Assert.AreEqual("25000/m2", houseManager.marketPriceText.text);
        Assert.AreEqual("8.70%", houseManager.changeText.text);
        Assert.IsTrue(houseManager.upImage.activeSelf);
        Assert.IsFalse(houseManager.downImage.activeSelf);
    }
    
    /*
    [Test]
    public void SellOneHouse()
    {
        houseManager.currentRound = 12;
        House house = new House(1, "House A", 60, 23000, 276000, 6619);
        houseManager.EnablePurchasedHousePrefabs();

        PlayerManager.Instance.playerCash = 0;
        PlayerManager.Instance.monthlyPayment = 6619;
        houseManager.lossText.enabled = false;
        houseManager.profitText.enabled = false;

        houseManager.SellHouse(0);

        Assert.AreEqual(0, House.purchasedHouses.Count);
        Assert.AreEqual(1, House.purchasedHousesHistory.Count);
        Assert.AreEqual(12, House.purchasedHousesHistory[0].sellRound);
        Assert.AreEqual(25000, House.purchasedHousesHistory[0].sellPrice);
        Assert.AreEqual(120000, House.purchasedHousesHistory[0].profitAndLoss);
        Assert.AreEqual(1500000, PlayerManager.Instance.playerCash);
        Assert.AreEqual(6619, PlayerManager.Instance.monthlyPayment);
        Assert.IsTrue(houseManager.sellCanvas.activeSelf);
        Assert.IsFalse(houseManager.background.activeSelf);
        for (int i = 0; i < 4; i++)
        {
            Assert.IsFalse(houseManager.housePrefabs[i].activeSelf);
        }
        Assert.IsTrue(houseManager.profitText.gameObject.activeSelf);
        Assert.AreEqual("120000!", houseManager.profitAndLossText.text);
    }
    */

    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(houseManager.gameObject);
    }

    private Text CreateTextComponent(string text, Transform parent, string childName)
    {
        GameObject textObject = new GameObject(childName);
        textObject.transform.SetParent(parent, false);
        Text textComponent = textObject.AddComponent<Text>();
        textComponent.text = text;
        return textComponent;
    }

    private GameObject CreateChildGameObject(Transform parent, string childName)
    {
        GameObject gameObject = new GameObject(childName);
        gameObject.transform.SetParent(parent, false);
        return gameObject;
    }
}
