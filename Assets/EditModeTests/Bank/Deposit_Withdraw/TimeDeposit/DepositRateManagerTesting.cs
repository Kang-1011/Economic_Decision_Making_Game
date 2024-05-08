using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class DepositRateManagerTesting
{
    private DepositRateManager depositRateManager;
    private BankRateManager bankRateManager;

    private TextMeshProUGUI rate3RoundsText;
    private TextMeshProUGUI rate6RoundsText;
    private TextMeshProUGUI rate12RoundsText;
    private TextMeshProUGUI rate24RoundsText;
    private TextMeshProUGUI rate36RoundsText;

    [SetUp]
    public void SetUp()
    {
        GameObject bankRateManagerOb = new GameObject();
        bankRateManager = bankRateManagerOb.AddComponent<BankRateManager>();
        bankRateManager.InitializeBankRates();

        GameObject depositRateManagerOb = new GameObject();
        depositRateManager = depositRateManagerOb.AddComponent<DepositRateManager>();
        
        rate3RoundsText = new GameObject().AddComponent<TextMeshProUGUI>();
        depositRateManager.dRate_3Rounds = rate3RoundsText;
        rate6RoundsText = new GameObject().AddComponent<TextMeshProUGUI>();
        depositRateManager.dRate_6Rounds = rate6RoundsText;
        rate12RoundsText = new GameObject().AddComponent<TextMeshProUGUI>();
        depositRateManager.dRate_12Rounds = rate12RoundsText;
        rate24RoundsText = new GameObject().AddComponent<TextMeshProUGUI>();
        depositRateManager.dRate_24Rounds = rate24RoundsText;
        rate36RoundsText = new GameObject().AddComponent<TextMeshProUGUI>();
        depositRateManager.dRate_36Rounds = rate36RoundsText;
    }

    [Test]
    public void DepositRateManager_Initialize()
    {
        Assert.IsNotNull(depositRateManager);
        Assert.IsNotNull(depositRateManager.dRate_3Rounds);
        Assert.IsNotNull(depositRateManager.dRate_6Rounds);
        Assert.IsNotNull(depositRateManager.dRate_12Rounds);
        Assert.IsNotNull(depositRateManager.dRate_24Rounds);
        Assert.IsNotNull(depositRateManager.dRate_36Rounds);
    }

    [Test]
    public void UpdateDifferentRatesText()
    {
        // Update deposit rate text
        depositRateManager.UpdateDepositRate(bankRateManager.GetDepositRate_3Rounds(), bankRateManager.GetDepositRate_6Rounds(),
                            bankRateManager.GetDepositRate_12Rounds(), bankRateManager.GetDepositRate_24Rounds(), 
                            bankRateManager.GetDepositRate_36Rounds());

        Assert.AreEqual("3 Rounds: " + (0.0135f * 100).ToString() + "%", depositRateManager.dRate_3Rounds.text);
        Assert.AreEqual("6 Rounds: " + (0.0155f * 100).ToString() + "%", depositRateManager.dRate_6Rounds.text);
        Assert.AreEqual("12 Rounds: " + (0.0175f * 100).ToString() + "%", depositRateManager.dRate_12Rounds.text);
        Assert.AreEqual("24 Rounds: " + (0.0225f * 100).ToString() + "%", depositRateManager.dRate_24Rounds.text);
        Assert.AreEqual("36 Rounds: " + (0.0275f * 100).ToString() + "%", depositRateManager.dRate_36Rounds.text);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(bankRateManager.gameObject);
        Object.DestroyImmediate(depositRateManager.gameObject);
        Object.DestroyImmediate(rate3RoundsText.gameObject);
        Object.DestroyImmediate(rate6RoundsText.gameObject);
        Object.DestroyImmediate(rate12RoundsText.gameObject);
        Object.DestroyImmediate(rate24RoundsText.gameObject);
        Object.DestroyImmediate(rate36RoundsText.gameObject);
    }
}
