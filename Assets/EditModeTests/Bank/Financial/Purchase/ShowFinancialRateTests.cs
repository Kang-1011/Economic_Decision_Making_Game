// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class ShowFinancialRateTests
{
    private ShowFinancialRate showFinancialRate;
    private FinancialRateManager financialRateManager;

    private Text R1RateText;
    private Text R3RateText;
    private Text R5RateText;

    [SetUp]
    public void SetUp()
    {
        GameObject financialRateManagerOb = new GameObject();
        financialRateManager = financialRateManagerOb.AddComponent<FinancialRateManager>();

        GameObject showFinancialRateOb = new GameObject();
        showFinancialRate = showFinancialRateOb.AddComponent<ShowFinancialRate>();

        R1RateText = new GameObject().AddComponent<Text>();
        showFinancialRate.R1Rate = R1RateText;
        R3RateText = new GameObject().AddComponent<Text>();
        showFinancialRate.R3Rate = R3RateText;
        R5RateText = new GameObject().AddComponent<Text>();
        showFinancialRate.R5Rate = R5RateText;
    }

    [Test]
    public void ShowFinancialRate_Initialize()
    {
        Assert.IsNotNull(showFinancialRate);
        Assert.IsNotNull(showFinancialRate.R1Rate);
        Assert.IsNotNull(showFinancialRate.R3Rate);
        Assert.IsNotNull(showFinancialRate.R5Rate);
    }

    [Test]
    public void UpdateDifferentRatesText()
    {
        // Update financial rate text
        showFinancialRate.UpdateFinancialRate(financialRateManager.GetAverageRate_R1(), 
                                        financialRateManager.GetAverageRate_R3(),
                                        financialRateManager.GetAverageRate_R5());

        Assert.AreEqual((financialRateManager.GetAverageRate_R1()).ToString("F2") + "%", showFinancialRate.R1Rate.text);
        Assert.AreEqual((financialRateManager.GetAverageRate_R3()).ToString("F2") + "%", showFinancialRate.R3Rate.text);
        Assert.AreEqual((financialRateManager.GetAverageRate_R5()).ToString("F2") + "%", showFinancialRate.R5Rate.text);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(financialRateManager.gameObject);
        Object.DestroyImmediate(showFinancialRate.gameObject);
        Object.DestroyImmediate(R1RateText.gameObject);
        Object.DestroyImmediate(R3RateText.gameObject);
        Object.DestroyImmediate(R5RateText.gameObject);
    }
}
