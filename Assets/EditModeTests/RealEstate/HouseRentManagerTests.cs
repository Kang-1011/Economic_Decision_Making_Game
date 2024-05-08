// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class HouseRentManagerTests
{
    private HouseRentManager houseRentManager;

    [SetUp]
    public void SetUp()
    {
        GameObject ob = new GameObject();
        houseRentManager = ob.AddComponent<HouseRentManager>();
    }

    [Test]
    public void LoadRents_LoadValidCSVFile_SetsRentsList()
    {
        houseRentManager.LoadRents("rents");

        Assert.IsNotNull(houseRentManager.rents);
        Assert.AreEqual(37, houseRentManager.rents.Count); // contain rents for 36 rounds
        
        foreach (List<int> roundRents in houseRentManager.rents)
        {
            Assert.IsNotNull(roundRents);
            Assert.AreEqual(4, roundRents.Count); // contain rents for 4 houses
        }
    }

    [Test]
    public void UpdateHouseRents_ValidRoundIndex()
    {
        houseRentManager.LoadRents("rents");
        houseRentManager.currentRound = 1;

        houseRentManager.houseAText = CreateTextComponent("");
        houseRentManager.houseBText = CreateTextComponent("");
        houseRentManager.houseCText = CreateTextComponent("");
        houseRentManager.houseDText = CreateTextComponent("");

        houseRentManager.UpdateHouseRents();

        Assert.AreEqual("3500/Month", houseRentManager.houseAText.text);
        Assert.AreEqual("2500/Month", houseRentManager.houseBText.text);
        Assert.AreEqual("2000/Month", houseRentManager.houseCText.text);
        Assert.AreEqual("1000/Month", houseRentManager.houseDText.text);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(houseRentManager.gameObject);
    }

    private Text CreateTextComponent(string text)
    {
        GameObject ob = new GameObject();
        Text textComponent = ob.AddComponent<Text>();
        textComponent.text = text;
        return textComponent;
    }
}