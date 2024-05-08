// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FinancialManagerTests
{
    private FinancialManager financialManager;
    [SetUp]
    public void SetUp()
    {
        GameObject gameObject = new GameObject();
        financialManager = gameObject.AddComponent<FinancialManager>();
    }

    [Test]
    public void AddFinancial()
    {
        financialManager.AddFinancial("Product 1", 1000f, "R1", 1);
        Assert.IsNotNull(financialManager);
        Assert.AreEqual(1, financialManager.financialDatas.Count);
    }

    [Test]
    public void FindRedeemedProduct()
    {
        financialManager.AddFinancial("Product 1", 1000f, "R1", 1);
        financialManager.AddFinancial("Product 2", 5000f, "R3", 3);
        financialManager.AddFinancial("Product 3", 10000f, "R5", 4);

        Assert.AreEqual("Product 1", financialManager.FindRedeemedProduct(0).GetName());
        Assert.AreEqual(1000f, financialManager.FindRedeemedProduct(0).GetPrincipal());
        Assert.AreEqual("R3", financialManager.FindRedeemedProduct(1).GetFinancialType());
        Assert.AreEqual(4, financialManager.FindRedeemedProduct(2).GetPurchaseDate());
    }

    [Test]
    public void RemoveFinancial()
    {
        financialManager.AddFinancial("Product 1", 1000f, "R1", 1);
        financialManager.AddFinancial("Product 2", 5000f, "R3", 3);
        financialManager.AddFinancial("Product 3", 10000f, "R5", 4);

        financialManager.RemoveFinancial(2);
        Assert.AreEqual(2, financialManager.financialDatas.Count);
        financialManager.RemoveFinancial(0);
        Assert.AreEqual(1, financialManager.financialDatas.Count);
    }

    [Test]
    public void CalculateTotalAmount()
    {
        financialManager.AddFinancial("Product 1", 10000f, "R1", 1);
        financialManager.AddFinancial("Product 2", 5000f, "R3", 3);
        financialManager.AddFinancial("Product 3", 10000f, "R5", 4);
        Assert.AreEqual(25000, financialManager.CalculateTotalAmount());
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(financialManager.gameObject);
    }
}
