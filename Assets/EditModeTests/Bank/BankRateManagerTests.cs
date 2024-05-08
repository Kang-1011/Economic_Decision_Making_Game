// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BankRateManagerTests
{
    private BankRateManager bankRateManager;

    [SetUp]
    public void SetUp()
    {
        GameObject testGameObject = new GameObject();
        bankRateManager = testGameObject.AddComponent<BankRateManager>();
        bankRateManager.InitializeBankRates();
    }

    [Test]
    public void BankRateManager_InitializeRate()
    {
        // Test the initial deposit rate
        Assert.AreEqual(0.0035f, bankRateManager.GetDepositRate_Current());
        Assert.AreEqual(0.0135f, bankRateManager.GetDepositRate_3Rounds());
        Assert.AreEqual(0.0155f, bankRateManager.GetDepositRate_6Rounds());
        Assert.AreEqual(0.0175f, bankRateManager.GetDepositRate_12Rounds());
        Assert.AreEqual(0.0225f, bankRateManager.GetDepositRate_24Rounds());
        Assert.AreEqual(0.0275f, bankRateManager.GetDepositRate_36Rounds());
        Assert.AreEqual(0.0275f, bankRateManager.GetDepositRate_36Rounds());
        // Test the initial personal credit loan rate
        Assert.AreEqual(0.061f, bankRateManager.GetCreditLoanRate(12));
        Assert.AreEqual(0.0645f, bankRateManager.GetCreditLoanRate(24));
        Assert.AreEqual(0.0685f, bankRateManager.GetCreditLoanRate(36));

    }

    [Test]
    public void SetDepositRate_UpdateRate()
    {
        // Test deposit rate Setter
        float newDepositRate = 0.070f;
        bankRateManager.SetDepositRate_Current(newDepositRate);
        Assert.AreEqual(newDepositRate, bankRateManager.GetDepositRate_Current());
        bankRateManager.SetDepositRate_3Rounds(newDepositRate);
        Assert.AreEqual(newDepositRate, bankRateManager.GetDepositRate_3Rounds());
        bankRateManager.SetDepositRate_6Rounds(newDepositRate);
        Assert.AreEqual(newDepositRate, bankRateManager.GetDepositRate_6Rounds());
        bankRateManager.SetDepositRate_12Rounds(newDepositRate);
        Assert.AreEqual(newDepositRate, bankRateManager.GetDepositRate_12Rounds());
        bankRateManager.SetDepositRate_24Rounds(newDepositRate);
        Assert.AreEqual(newDepositRate, bankRateManager.GetDepositRate_24Rounds());
        bankRateManager.SetDepositRate_36Rounds(newDepositRate);
        Assert.AreEqual(newDepositRate, bankRateManager.GetDepositRate_36Rounds());
    }

    [Test]
    public void SetLoanRate_UpdateRate()
    {
        // Test loan rate Setter
        float newLoanRate = 1.000f;
        bankRateManager.SetCreditLoanRate_12Rounds(newLoanRate);
        Assert.AreEqual(newLoanRate, bankRateManager.GetCreditLoanRate_12Rounds());
        bankRateManager.SetCreditLoanRate_24Rounds(newLoanRate);
        Assert.AreEqual(newLoanRate, bankRateManager.GetCreditLoanRate_24Rounds());
        bankRateManager.SetCreditLoanRate_36Rounds(newLoanRate);
        Assert.AreEqual(newLoanRate, bankRateManager.GetCreditLoanRate_36Rounds());
    }


    // Destory the gameobject after each testing finished
    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(bankRateManager.gameObject);
    }
}
