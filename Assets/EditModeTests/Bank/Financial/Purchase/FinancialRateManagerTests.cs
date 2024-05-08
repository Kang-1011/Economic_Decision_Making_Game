// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FinancialRateManagerTests
{
    private FinancialRateManager financialRateManager;

    [SetUp]
    public void SetUp()
    {
        GameObject testGameObject = new GameObject();
        financialRateManager = testGameObject.AddComponent<FinancialRateManager>();
    }

    [Test]
    public void FinancialRateManager_Initialization()
    {
        Assert.AreEqual(0, financialRateManager.GetAverageRate_R1());
        Assert.AreEqual(0, financialRateManager.GetAverageRate_R3());
        Assert.AreEqual(0, financialRateManager.GetAverageRate_R5());

        financialRateManager.Initialization();
        Assert.AreNotEqual(0, financialRateManager.GetAverageRate_R1());
        Assert.AreNotEqual(0, financialRateManager.GetAverageRate_R3());
        Assert.AreNotEqual(0, financialRateManager.GetAverageRate_R5());
    }

    [Test]
    public void UpdateRates_GetNewAverageRate()
    {
        financialRateManager.Initialization();
        float originalRate_R1 = financialRateManager.GetAverageRate_R1();
        float originalRate_R3 = financialRateManager.GetAverageRate_R3();
        float originalRate_R5 = financialRateManager.GetAverageRate_R5();
        
        financialRateManager.UpdateRates();
        Assert.AreNotEqual(originalRate_R1, financialRateManager.GetAverageRate_R1());
        Assert.AreNotEqual(originalRate_R3, financialRateManager.GetAverageRate_R3());
        Assert.AreNotEqual(originalRate_R5, financialRateManager.GetAverageRate_R5());
    }

    [Test]
    public void GetCurrentRate()
    {
        financialRateManager.Initialization();
        Assert.AreEqual(0, financialRateManager.GetCurrentRate("R1"));
        Assert.AreEqual(0, financialRateManager.GetCurrentRate("R3"));
        Assert.AreEqual(0, financialRateManager.GetCurrentRate("R5"));

        financialRateManager.UpdateRates();
        Assert.AreNotEqual(0, financialRateManager.GetCurrentRate("R1"));
        Assert.AreNotEqual(0, financialRateManager.GetCurrentRate("R3"));
        Assert.AreNotEqual(0, financialRateManager.GetCurrentRate("R5"));
    }


    // Destory the gameobject after each testing finished
    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(financialRateManager.gameObject);
    }
}
