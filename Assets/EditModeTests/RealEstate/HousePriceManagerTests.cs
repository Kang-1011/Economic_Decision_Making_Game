// All code was written by the team

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HousePriceManagerTests
{
    private HousePriceManager housePriceManager;

    [SetUp]
    public void SetUp()
    {
        GameObject ob = new GameObject();
        housePriceManager = ob.AddComponent<HousePriceManager>();
    }

    [Test]
    public void LoadPrices_LoadValidCSVFile_SetsPricesList()
    {
        housePriceManager.LoadPrices("prices");

        Assert.IsNotNull(housePriceManager.prices);
        Assert.AreEqual(37, housePriceManager.prices.Count); // contain prices for 36 rounds
        
        foreach (List<int> roundPrices in housePriceManager.prices)
        {
            Assert.IsNotNull(roundPrices);
            Assert.AreEqual(4, roundPrices.Count); // contain prices for 4 houses
        }
    }
    
    [Test]
    public void UpdateHousePrices_ValidRoundIndex()
    {
        housePriceManager.LoadPrices("prices");
        housePriceManager.currentRound = 1;

        housePriceManager.houseAText = CreateTextComponent("");
        housePriceManager.houseBText = CreateTextComponent("");
        housePriceManager.houseCText = CreateTextComponent("");
        housePriceManager.houseDText = CreateTextComponent("");

        housePriceManager.UpdateHousePrices();

        Assert.AreEqual("23000/m2", housePriceManager.houseAText.text);
        Assert.AreEqual("21000/m2", housePriceManager.houseBText.text);
        Assert.AreEqual("19000/m2", housePriceManager.houseCText.text);
        Assert.AreEqual("12000/m2", housePriceManager.houseDText.text);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(housePriceManager.gameObject);
    }

    private Text CreateTextComponent(string text)
    {
        GameObject ob = new GameObject();
        Text textComponent = ob.AddComponent<Text>();
        textComponent.text = text;
        return textComponent;
    }
}