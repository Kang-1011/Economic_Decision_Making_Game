// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class HouseTests
{
    [SetUp]
    public void SetUp()
    {
        House.purchasedHouses = new List<House>();
        House.purchasedHousesHistory = new List<House>();
    }

    [Test]
    public void AddPurchasedHouse()
    {
        House house = new House(1, "House A", 60, 23000, 276000, 6619);
        Assert.AreEqual(1, house.round);
        Assert.AreEqual(-1, house.sellRound);
        Assert.AreEqual("House A", house.title);
        Assert.AreEqual(60, house.area);
        Assert.AreEqual(23000, house.purchasePrice);
        Assert.AreEqual(0.0, house.sellPrice);
        Assert.AreEqual(0.0, house.profitAndLoss);
        Assert.AreEqual(276000, house.downPayment);
        Assert.AreEqual(6619, house.monthlyPayment);
        Assert.AreEqual(0.0f, house.percentage_change);
        Assert.AreEqual(1, House.purchasedHouses.Count);
        Assert.AreEqual(1, House.purchasedHousesHistory.Count);
    }
}
