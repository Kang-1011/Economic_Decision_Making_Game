// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class CurrentDepositRateManagerTests
{
    private CurrentDepositRateManager currentDepositRateManager;
    private BankRateManager bankRateManager;
    private TextMeshProUGUI text;

    [SetUp]
    public void SetUp()
    {
        GameObject bankRateManagerOb = new GameObject();
        bankRateManager = bankRateManagerOb.AddComponent<BankRateManager>();
        bankRateManager.InitializeBankRates();

        GameObject currentDepositRateManagerOb = new GameObject();
        currentDepositRateManager = currentDepositRateManagerOb.AddComponent<CurrentDepositRateManager>();
        
        GameObject textOb = new GameObject();
        text = textOb.AddComponent<TextMeshProUGUI>();
        currentDepositRateManager.dRate = text;
    }

    [Test]
    public void CurrentDepositRateManager_Initialize()
    {
        Assert.IsNotNull(currentDepositRateManager.dRate);
        Assert.IsNotNull(currentDepositRateManager);
    }

    [Test]
    public void UpdateCurrentRateText()
    {
        // Update deposit rate text
        currentDepositRateManager.UpdateDepositRate(bankRateManager.GetDepositRate_Current());

        float expectedRate = 0.0035f * 100;
        string expectedText = "Rate : " + expectedRate.ToString() + "%";
        Assert.AreEqual(expectedText, currentDepositRateManager.dRate.text);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(bankRateManager.gameObject);
        Object.DestroyImmediate(currentDepositRateManager.gameObject);
        Object.DestroyImmediate(text.gameObject);
    }
}
