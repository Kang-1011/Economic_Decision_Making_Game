// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FinancialSummaryManagerTests
{
    private FinancialSummaryManager financialSummaryManager;
    private FinancialManager financialManager;
    [SetUp]
    public void SetUp()
    {
        GameObject gameObject = new GameObject();
        financialManager = gameObject.AddComponent<FinancialManager>();
        financialSummaryManager = gameObject.AddComponent<FinancialSummaryManager>();
        financialSummaryManager.financialDatas = financialManager.financialDatas;
        financialSummaryManager.redeemedDatas = financialManager.redeemedDatas;
    }

    [Test]
    public void CalculateTotalSpending()
    {
        financialManager.AddFinancial("Product 1", 1000f, "R1", 1);
        financialManager.AddFinancial("Product 2", 5000f, "R3", 3);
        financialManager.AddRedeemedProduct(1000f, "R5", 2000f);
        Assert.AreEqual(7000f, financialSummaryManager.CalculateTotalSpending());
    }

    [Test]
    public void CalculateTotalIncome()
    {
        financialManager.AddRedeemedProduct(1000f, "R1", 2000f);
        financialManager.AddRedeemedProduct(1000f, "R3", 2500f);
        financialManager.AddRedeemedProduct(1000f, "R5", 800f);
        Assert.AreEqual(5300f, financialSummaryManager.CalculateTotalIncome());
    }

    [Test]
    public void IsProfitable()
    {
        financialManager.AddRedeemedProduct(1000f, "R1", 2000f);
        financialManager.AddRedeemedProduct(1000f, "R3", 2500f);
        financialManager.AddRedeemedProduct(1000f, "R5", 800f);
        financialSummaryManager.totalSpending = financialSummaryManager.CalculateTotalSpending();
        financialSummaryManager.totalIncome = financialSummaryManager.CalculateTotalIncome();
        Assert.AreEqual(true, financialSummaryManager.IsProfitable());
    }

    [Test]
    public void CalculateDiffTypeInfo()
    {
        financialManager.AddRedeemedProduct(1000f, "R1", 2000f);
        financialManager.AddRedeemedProduct(1000f, "R3", 2500f);
        financialManager.AddRedeemedProduct(1000f, "R5", 800f);
        financialSummaryManager.CalculateDiffTypeInfo();
        Assert.AreEqual(1000, financialSummaryManager.R1_NetIncome);
        Assert.AreEqual(1500, financialSummaryManager.R3_NetIncome);
        Assert.AreEqual(-200, financialSummaryManager.R5_NetIncome);
    }

    [Test]
    public void GetProfitableSummary_R1()
    {
        financialManager.AddRedeemedProduct(1000f, "R1", 2000f);
        financialSummaryManager.CalculateDiffTypeInfo();
        string correctText = "Among all the financial products you've purchased, products with risk level R1 yield the highest net income, reaching 1000 yuan.\n"
                            + "In economics, expected payoff refers to the anticipated average profit from an investment or business activity over a certain future period, taking into account various risk factors. In theory, the actual investment outcomes of low-risk products typically tend to be closer to their expected payoff. This is because low-risk products exhibit lower volatility and uncertainty, rendering them relatively stable.";
        Assert.AreEqual(correctText, financialSummaryManager.GetProfitableSummary());
    }

    [Test]
    public void GetProfitableSummary_R3()
    {
        financialManager.AddRedeemedProduct(1000f, "R3", 2500f);
        financialSummaryManager.CalculateDiffTypeInfo();
        string correctText = "Among all the financial products you've purchased, products with risk level R3 yield the highest net income, reaching 1500 yuan.\n"
                            + "In economics, expected payoff refers to the anticipated average profit from an investment or business activity over a certain future period, taking into account various risk factors. Medium-risk products typically involve a certain degree of market volatility and uncertainty, albeit less pronounced than high-risk products, which may still impact investment outcomes to some extent. Consequently, deviations from expected payoff are also plausible.";
        Assert.AreEqual(correctText, financialSummaryManager.GetProfitableSummary());
    }

    [Test]
    public void GetProfitableSummary_R5()
    {
        financialManager.AddRedeemedProduct(1000f, "R5", 3000f);
        financialSummaryManager.CalculateDiffTypeInfo();
        string correctText = "Among all the financial products you've purchased, products with risk level R5 yield the highest net income, reaching 2000 yuan.\n"
                            + "Products with Risk R5 entail the highest interest rates, yet they also pose the greatest potential for losses. In economics, expected payoff refers to the anticipated average profit from an investment or business activity over a certain future period, taking into account various risk factors. Generally, high-risk products tend to offer higher expected payoff. It's important to note that expected payoff is merely an anticipated value, and actual investment outcomes may differ.";
        Assert.AreEqual(correctText, financialSummaryManager.GetProfitableSummary());
    }

    [Test]
    public void GetLossSummary_R3()
    {
        financialManager.AddRedeemedProduct(1000f, "R3", 800f);
        financialSummaryManager.CalculateDiffTypeInfo();
        string correctText = "In this module, the product with the highest loss is classified as R3 risk level, reaching 200 yuan.\n"
                            + "In economics, expected payoff refers to the anticipated average profit from an investment or business activity over a certain future period, taking into account various risk factors. Medium-risk products typically involve a certain degree of market volatility and uncertainty, albeit less pronounced than high-risk products, which may still impact investment outcomes to some extent. Consequently, deviations from expected payoff are also plausible.";
        Assert.AreEqual(correctText, financialSummaryManager.GetLossSummary());
    }

    [Test]
    public void GetLossSummary_R5()
    {
        financialManager.AddRedeemedProduct(1000f, "R5", 800f);
        financialSummaryManager.CalculateDiffTypeInfo();
        string correctText =  "In this module, the product with the highest loss is classified as R5 risk level, reaching 200 yuan.\n"
                            + "Products with Risk R5 entail the highest interest rates, yet they also pose the greatest potential for losses. In economics, expected payoff refers to the anticipated average profit from an investment or business activity over a certain future period, taking into account various risk factors. Due to their typically higher volatility and uncertainty, high-risk products may exhibit significant deviations from the expected returns in their actual performance.";
        Assert.AreEqual(correctText, financialSummaryManager.GetLossSummary());
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(financialSummaryManager.gameObject);
    }
}
