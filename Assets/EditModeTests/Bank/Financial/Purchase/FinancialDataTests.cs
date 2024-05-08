// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FinancialDataTests
{
    [Test]
    public void FinancialData_CreateConstructor()
    {
        FinancialData financialData = new FinancialData("Product 1", 1000f, "R1", 1);
        Assert.AreEqual("Product 1", financialData.GetName());
        Assert.AreEqual(1000f, financialData.GetPrincipal());
        Assert.AreEqual("R1", financialData.GetFinancialType());
        Assert.AreEqual(1, financialData.GetPurchaseDate());
        Assert.AreEqual(0, financialData.GetInterest());           // Initial interest is 0
        Assert.AreEqual(1000f, financialData.GetTotalAmount());    // Initial total amount is principal
    }

    [Test]
    public void SetTotalAmount()
    {
        FinancialData financialData = new FinancialData("Product 2", 5000f, "R3", 1);
        financialData.SetTotalAmount(6000f);
        Assert.AreEqual(6000f, financialData.GetTotalAmount());
    }

    [Test]
    public void UpdateInterest()
    {
        FinancialData financialData = new FinancialData("Product 1", 1000f, "R1", 1);
        Assert.AreEqual(0, financialData.GetInterest());
        Assert.AreEqual(1000f, financialData.GetTotalAmount());

        financialData.rate = 0.015f;
        financialData.UpdateInterest();
        Assert.AreEqual(15f, financialData.GetInterest());
        Assert.AreEqual(1015f, financialData.GetTotalAmount());
    }

    [Test]
    public void RedeemPartially()
    {
        FinancialData financialData = new FinancialData("Product 2", 5000f, "R3", 1);
        
        financialData.rate = 0.030f;
        financialData.UpdateInterest();
        Assert.AreEqual(5000f, financialData.GetPrincipal());
        Assert.AreEqual(150f, financialData.GetInterest());
        Assert.AreEqual(5150f, financialData.GetTotalAmount());

        financialData.RedeemPartially(1030f);
        Assert.AreEqual(4000f, financialData.GetPrincipal());
        Assert.AreEqual(120f, financialData.GetInterest());
        Assert.AreEqual(4120f, financialData.GetTotalAmount());
    }
}
