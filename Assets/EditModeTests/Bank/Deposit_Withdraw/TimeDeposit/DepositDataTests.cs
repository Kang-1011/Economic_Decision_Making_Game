using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DepositDataTests
{
    [Test]
    public void DepositData_CreateConstructor()
    {
        DepositData depositData = new DepositData(1000f, 0.0135f, 3, 1);
        Assert.AreEqual(1000f, depositData.GetPrincipal());
        Assert.AreEqual(0.0135f, depositData.GetRate());
        Assert.AreEqual(1, depositData.GetDepositDate());
        Assert.AreEqual(4, depositData.GetFinishDate());
        Assert.AreEqual(0, depositData.GetInterest());          // Initial interest is 0
        Assert.AreEqual(1000f, depositData.GetTotalAmount());   // Initial total amount is the principal
        Assert.IsFalse(depositData.GetDue());
        Assert.IsFalse(depositData.GetExpire());
    }

    [Test]
    public void CheckExpireDue_IsNoDue()
    {
        DepositData depositData = new DepositData(1000f, 0.0135f, 3, 1);
        depositData.currentRound = 2;
        depositData.CheckExpireDue();
        Assert.IsFalse(depositData.GetDue());
        Assert.IsFalse(depositData.GetExpire());
    }

    [Test]
    public void CheckExpireDue_IsDue()
    {
        DepositData depositData = new DepositData(1000f, 0.0135f, 3, 1);
        depositData.currentRound = 4;
        depositData.CheckExpireDue();
        Assert.IsTrue(depositData.GetDue());
        Assert.IsFalse(depositData.GetExpire());
    }

    [Test]
    public void CheckExpireDue_HasExpired()
    {
        DepositData depositData = new DepositData(1000f, 0.0135f, 3, 1);
        depositData.currentRound = 5;
        depositData.CheckExpireDue();
        Assert.IsFalse(depositData.GetDue());
        Assert.IsTrue(depositData.GetExpire());
    }

    [Test]
    public void UpdateInterest_HasNotExpired()
    {
        DepositData depositData = new DepositData(1000f, 0.0135f, 3, 1);
        depositData.currentRound = 2;
        depositData.UpdateInterest();

        float expectedInterest = 0 + 1000f * 0.0135f/ 12;         // Interest added
        float expectedTotalAmount = expectedInterest + 1000f;     // Total amount added
        
        Assert.AreEqual(expectedInterest, depositData.GetInterest());
        Assert.AreEqual(expectedTotalAmount, depositData.GetTotalAmount());
    }

    [Test]
    public void UpdateInterest_HasExpired()
    {
        DepositData depositData = new DepositData(1000f, 0.0135f, 3, 1);
        depositData.currentRound = 5;
        depositData.currentDepositRate = 0.004f;
        depositData.UpdateInterest();

        float expectedInterest = 0 + 1000f * 0.004f/ 12;          // Interest added
        float expectedTotalAmount = expectedInterest + 1000f;     // Total amount added
        
        Assert.AreEqual(expectedInterest, depositData.GetInterest());
        Assert.AreEqual(expectedTotalAmount, depositData.GetTotalAmount());
    }

    [Test]
    public void EarlyWithdraw()
    {
        DepositData depositData = new DepositData(1000f, 0.0135f, 3, 1);
        depositData.currentRound = 3;
        depositData.currentDepositRate = 0.0035f;
        depositData.EarlyWithdraw();

        float expectedInterest = 1000f * (0.0035f / 12) * (3 - 1); // Total interest added
        float expectedTotalAmount = expectedInterest + 1000f;     // Total amount added

        Assert.AreEqual(0.0035f, depositData.GetRate());
        Assert.AreEqual(expectedInterest, depositData.GetInterest());
        Assert.AreEqual(expectedTotalAmount, depositData.GetTotalAmount());
    }
}
