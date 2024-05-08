// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CurrentDepositDataTests
{
    [Test]
    public void CurrentDepositData_CreateConstructor()
    {
        CurrentDepositData currentDepositData = new CurrentDepositData(1000f, 0.01f, 1);
        Assert.AreEqual(1000f, currentDepositData.GetPrincipal());
        Assert.AreEqual(0.01f, currentDepositData.GetRate());
        Assert.AreEqual(1, currentDepositData.GetDepositDate());
        Assert.AreEqual(0, currentDepositData.GetInterest());          // Initial interest is 0
        Assert.AreEqual(1000f, currentDepositData.GetTotalAmount());   // Initial total amount is principal
    }

    [Test]
    public void UpdateInterest_UpdateTotalAmount()
    {
        CurrentDepositData currentDepositData = new CurrentDepositData(1000f, 0.01f, 1);
        currentDepositData.UpdateInterest();
        float expectedInterest = 0 + 1000f * 0.01f/ 12;          // Interest added
        float expectedTotalAmount = expectedInterest + 1000f;    // Total amount added
        Assert.AreEqual(expectedInterest, currentDepositData.GetInterest());
        Assert.AreEqual(expectedTotalAmount, currentDepositData.GetTotalAmount());
    }
}
