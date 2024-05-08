// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class OrderDataTests
{
    [Test]
    public void Constructor_InitializesFields()
    {
        // Arrange
        int mode = 0;
        string ticker = "BTC";
        double entryPrice = 150.0;
        double quantity = 10;
        double totalValue = 1500.0;
        double sl = 155.0;
        double tp = 145.0;

        // Act
        OrderData orderData = new OrderData(mode, ticker, entryPrice, quantity, totalValue, sl, tp);

        // Assert
        Assert.AreEqual(ticker, orderData.Ticker);
        Assert.AreEqual(entryPrice, orderData.EntryPrice);
        Assert.AreEqual(quantity, orderData.Quantity);
        Assert.AreEqual(totalValue, orderData.TotalValue);
        Assert.AreEqual(sl, orderData.Sl);
        Assert.AreEqual(tp, orderData.Tp);
    }

    public void SetTradeType_Mode0()
    {
        // Arrange
        int mode = 0;

        // Act
        OrderData orderData = new OrderData(mode, "", 0, 0, 0, 0, 0);

        // Assert
        Assert.AreEqual("Long", orderData.TradeType);
        Assert.AreEqual("Ongoing", orderData.Status);
    }

    public void SetTradeType_Mode1()
    {
        // Arrange
        int mode = 1;

        // Act
        OrderData orderData = new OrderData(mode, "", 0, 0, 0, 0, 0);

        // Assert
        Assert.AreEqual("Short", orderData.TradeType);
        Assert.AreEqual("Ongoing", orderData.Status);
    }

    public void SetTradeType_Mode2()
    {
        // Arrange
        int mode = 2;

        // Act
        OrderData orderData = new OrderData(mode, "", 0, 0, 0, 0, 0);

        // Assert
        Assert.AreEqual("Buy", orderData.TradeType);
        Assert.AreEqual("Filled", orderData.Status);
    }

    public void SetTradeType_Mode3()
    {
        // Arrange
        int mode = 3;

        // Act
        OrderData orderData = new OrderData(mode, "", 0, 0, 0, 0, 0);

        // Assert
        Assert.AreEqual("Sell", orderData.TradeType);
        Assert.AreEqual("Filled", orderData.Status);
    }
}
