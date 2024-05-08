// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class OrderDataListTests
{
    [Test]
    public void Constructor_InitialisesOrdersArray()
    {
        // Arrange
        OrderData[] ordersArray = { 
            new OrderData(0, "AAPL", 150.0, 10, 1500.0, 145.0, 155.0),
            new OrderData(1, "TSLA", 2500.0, 5, 12500.0, 2550.0, 2450.0),
            new OrderData(2, "MSFT", 200.0, 20, 4000.0, 0.0, 0.0),
            new OrderData(3, "NFLX", 500.0, 5, 2500.0, 0.0, 0.0),
        };

        // Act
        OrderDataList orderDataList = new OrderDataList(ordersArray);

        // Assert
        Assert.AreEqual(ordersArray, orderDataList.Orders, "OrderDataList's orders array should match the provided array");
    }
}
