// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit;
using NUnit.Framework;

public class CreditRateManagerTests
{
    private CreditRateManager creditRateManager;

    [SetUp]
    public void SetUp()
    {
        GameObject gameObject = new GameObject();
        creditRateManager = gameObject.AddComponent<CreditRateManager>();
    }

    [Test]
    public void TestGetMaxLoanAmount()
    {
        int result = creditRateManager.GetMaxLoanAmount("CCC");
        Assert.AreEqual(result, 70000);
    }

    [Test]
    public void AddLoanLimit()
    {
        int initialLoanLimit = creditRateManager.GetLoanLimit();
        int amountToAdd = 100;

        creditRateManager.AddLoanLimit(amountToAdd);

        Assert.AreEqual(initialLoanLimit + amountToAdd, creditRateManager.GetLoanLimit());
    }

    [Test]
    public void SubtractLoanLimit()
    {
        creditRateManager.AddLoanLimit(500);
        int initialLoanLimit = creditRateManager.GetLoanLimit();
        int amountToSubtract = 100;

        creditRateManager.SubtractLoanLimit(amountToSubtract);

        Assert.AreEqual(initialLoanLimit - amountToSubtract, creditRateManager.GetLoanLimit());
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.DestroyImmediate(creditRateManager.gameObject);
    }
}
