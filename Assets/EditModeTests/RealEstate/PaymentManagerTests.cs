// All code was written by the team

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class PaymentManagerTests
{
    private PaymentManager paymentManager;
    private HousePriceManager housePriceManager;
    private PlayerManager playerManager;

    [SetUp]
    public void SetUp()
    {
        GameObject object1 = new GameObject();
        paymentManager = object1.AddComponent<PaymentManager>();

        GameObject object2 = new GameObject();
        housePriceManager = object2.AddComponent<HousePriceManager>();
        paymentManager.housePriceManager = housePriceManager;
        housePriceManager.LoadPrices("prices");

        GameObject object3 = new GameObject();
        playerManager = object3.AddComponent<PlayerManager>();
        // playerManager.Awake();

        paymentManager.currentRound = 1;
        paymentManager.houseText = CreateTextComponent("House A");
        paymentManager.areaText = CreateTextComponent("60");
        paymentManager.downPaymentRatioText = CreateTextComponent("0%");
        paymentManager.downPaymentText = CreateTextComponent("0");
        paymentManager.loanText = CreateTextComponent("0");
        paymentManager.monthlyPaymentText = CreateTextComponent("0");
        paymentManager.purchaseText = CreateTextComponent("");
        paymentManager.noCashText = CreateTextComponent("");
    }

    [Test]
    public void DecreaseDownPaymentRatio()
    {
        paymentManager.downPaymentRatio = 0.3f;
        paymentManager.DecreaseDownPaymentRatio();
        float epsilon = 0.000001f;
        Assert.AreEqual(0.2f, paymentManager.downPaymentRatio, epsilon);
    }

    [Test]
    public void IncreaseDownPaymentRatio()
    {
        paymentManager.downPaymentRatio = 0.2f;
        paymentManager.IncreaseDownPaymentRatio();
        float epsilon = 0.000001f;
        Assert.AreEqual(0.3f, paymentManager.downPaymentRatio, epsilon);
    }

    [Test]
    public void ClampDownPaymentRatio_LowerBound()
    {
        paymentManager.downPaymentRatio = 0.1f;
        paymentManager.ClampDownPaymentRatio();
        float epsilon = 0.000001f;
        Assert.AreEqual(0.2f, paymentManager.downPaymentRatio, epsilon);
    }

    [Test]
    public void ClampDownPaymentRatio_UpperBound()
    {
        paymentManager.downPaymentRatio = 1.1f;
        paymentManager.ClampDownPaymentRatio();
        float epsilon = 0.000001f;
        Assert.AreEqual(1.0f, paymentManager.downPaymentRatio, epsilon);
    }

    [Test]
    public void UpdateDownPaymentRatio_UITextComponent()
    {
        paymentManager.downPaymentRatio = 0.8f;
        paymentManager.UpdateDownPaymentRatio();
        Assert.AreEqual("80%", paymentManager.downPaymentRatioText.text);
    }

    [Test]
    public void CalculateDownPaymentAndLoan()
    {
        paymentManager.downPaymentRatio = 0.2f;
        paymentManager.CalculateDownPaymentAndLoan();
        
        float epsilon = 0.000001f;
        Assert.AreEqual(60 * 23000 * 0.2, paymentManager.downPayment, epsilon);
        Assert.AreEqual(60 * 23000 * 0.8, paymentManager.loan, epsilon);
        Assert.AreEqual((60 * 23000 * 0.2).ToString("F0"), paymentManager.downPaymentText.text);
        Assert.AreEqual((60 * 23000 * 0.8).ToString("F0"), paymentManager.loanText.text);
    }

    [Test]
    public void CalculateMonthlyPayment()
    {
        paymentManager.annualInterestRate = 0.06f;
        paymentManager.loan = 1104000;
        paymentManager.loanPeriodsInMonths = 360;

        paymentManager.CalculateMonthlyPayment();

        float epsilon = 0.1f;
        Assert.AreEqual(6619, paymentManager.monthlyPayment, epsilon);
        Assert.AreEqual("6619", paymentManager.monthlyPaymentText.text);
    }

    /* 
    [Test]
    public void PurchaseHouse_NoEnoughCash()
    {
        PlayerManager.Instance.playerCash = 0;
        PlayerManager.Instance.monthlyPayment = 0;
        paymentManager.area = 60;
        paymentManager.price = 23000;
        paymentManager.downPayment = 276000;
        paymentManager.monthlyPayment = 6619;
        paymentManager.purchaseText.enabled = false; 
        paymentManager.noCashText.enabled = false;   

        paymentManager.PurchaseHouse();

        Assert.IsTrue(paymentManager.noCashText.gameObject.activeSelf);
        Assert.AreEqual(0, PlayerManager.Instance.playerCash);
        Assert.AreEqual(0, PlayerManager.Instance.monthlyPayment);
    }
    

    [Test]
    public void PurchaseHouse_EnoughCash()
    {
        PlayerManager.Instance.playerCash = 1000000;
        PlayerManager.Instance.monthlyPayment = 0;
        paymentManager.area = 60;
        paymentManager.price = 23000;
        paymentManager.downPayment = 276000;
        paymentManager.monthlyPayment = 6619;
        paymentManager.purchaseText.enabled = false; 
        paymentManager.noCashText.enabled = false;   

        paymentManager.PurchaseHouse();

        Assert.IsTrue(paymentManager.purchaseText.gameObject.activeSelf);
        Assert.AreEqual(724000, PlayerManager.Instance.playerCash);
        Assert.AreEqual(6619, PlayerManager.Instance.monthlyPayment);
    }
    */
    
    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(paymentManager.gameObject);
        Object.DestroyImmediate(housePriceManager.gameObject);
    }

    private Text CreateTextComponent(string text)
    {
        GameObject ob = new GameObject();
        Text textComponent = ob.AddComponent<Text>();
        textComponent.text = text;
        return textComponent;
    }
}