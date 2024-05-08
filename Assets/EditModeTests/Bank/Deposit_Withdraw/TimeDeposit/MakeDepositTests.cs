using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using TMPro;

public class MakeDepositTests
{
    private MakeDeposit makeDeposit;
    private PlayerManager playerManager;
    private BankRateManager bankRateManager;
    private GameObject inputFieldOb;
    private GameObject warningTextOb;
    private GameObject buttonOb;

    private GameObject periodTextOb;
    private GameObject amountTextOb;
    private GameObject rateTextOb;

    [SetUp]
    public void SetUp()
    {
        GameObject playerManagerOb = new GameObject();
        playerManager = playerManagerOb.AddComponent<PlayerManager>();
        playerManager.InitializePlayerAssets();

        GameObject bankRateManagerOb = new GameObject();
        bankRateManager = bankRateManagerOb.AddComponent<BankRateManager>();
        bankRateManager.InitializeBankRates();

        GameObject makeDepositOb = new GameObject();
        makeDeposit = makeDepositOb.AddComponent<MakeDeposit>();
        makeDeposit.cash = 100000.0f;

        inputFieldOb = new GameObject();
        makeDeposit.inputField = inputFieldOb.AddComponent<TMP_InputField>();
        warningTextOb = new GameObject();
        makeDeposit.warning = warningTextOb.AddComponent<TextMeshProUGUI>();
        buttonOb = new GameObject();
        makeDeposit.submitButton = buttonOb.AddComponent<Button>();

        periodTextOb = new GameObject();
        makeDeposit.periodText = periodTextOb.AddComponent<TextMeshProUGUI>();
        amountTextOb = new GameObject();
        makeDeposit.amountText = amountTextOb.AddComponent<TextMeshProUGUI>();
        rateTextOb = new GameObject();
        makeDeposit.rateText = rateTextOb.AddComponent<TextMeshProUGUI>();
    }

    [Test]
    public void OnDropdownChanged_SetCorrectPeriod()
    {
        int[] expectedPeriods = {3, 6, 12, 24, 36};
        for (int index = 0; index < expectedPeriods.Length; index++)
        {
            makeDeposit.OnDropdownChanged(index);
            Assert.AreEqual(expectedPeriods[index], makeDeposit.GetPeriod());
        }
    }

    [Test]
    public void ValidateInput_CheckNoInput()
    {
        // inputNumber == null
        makeDeposit.ValidateInput("");
        Assert.IsFalse(makeDeposit.GetValidate(), "Validate should be false for invalid input.");
        Assert.IsNull(makeDeposit.warning.text);
    }

    [Test]
    public void ValidateInput_CheckInvalidInput()
    {
        // inputNumber < 50f
        makeDeposit.ValidateInput("1");
        Assert.IsFalse(makeDeposit.GetValidate(), "Validate should be false for invalid input.");
        Assert.AreEqual("The minimum deposit is 50 yuan", makeDeposit.warning.text);

        // inputNumber > cash
        makeDeposit.ValidateInput("10000000000");
        Assert.IsFalse(makeDeposit.GetValidate(), "Validate should be false for invalid input.");
        Assert.AreEqual("You don't have enough cash", makeDeposit.warning.text);
    }

    [Test]
    public void ValidateInput_CheckValidInput()
    {
        // 50f <= inputNumber <= cash
        makeDeposit.ValidateInput("100"); 
        Assert.IsTrue(makeDeposit.GetValidate(), "Validate should be true for valid input.");
        Assert.AreEqual("", makeDeposit.warning.text);
    }

    [Test]
    public void ShowInfo_Choose3Rounds()
    {
        makeDeposit.timeRate = new float[5] {0.01f, 0.02f, 0.03f, 0.04f, 0.05f};
        makeDeposit.OnDropdownChanged(0);
        makeDeposit.ValidateInput("100"); 
        Assert.AreEqual("Period of the deposit: 3", makeDeposit.periodText.text);
        Assert.AreEqual("Amount of the deposit: 100", makeDeposit.amountText.text);
        Assert.AreEqual("Annual Interest Rate: 1%", makeDeposit.rateText.text);
    }

    [Test]
    public void ShowInfo_Choose6Rounds()
    {
        makeDeposit.timeRate = new float[5] {0.01f, 0.02f, 0.03f, 0.04f, 0.05f};
        makeDeposit.OnDropdownChanged(1);
        makeDeposit.ValidateInput("100"); 
        Assert.AreEqual("Period of the deposit: 6", makeDeposit.periodText.text);
        Assert.AreEqual("Amount of the deposit: 100", makeDeposit.amountText.text);
        Assert.AreEqual("Annual Interest Rate: 2%", makeDeposit.rateText.text);
    }

    [Test]
    public void ShowInfo_Choose12Rounds()
    {
        makeDeposit.timeRate = new float[5] {0.01f, 0.02f, 0.03f, 0.04f, 0.05f};
        makeDeposit.OnDropdownChanged(2);
        makeDeposit.ValidateInput("1000"); 
        Assert.AreEqual("Period of the deposit: 12", makeDeposit.periodText.text);
        Assert.AreEqual("Amount of the deposit: 1000", makeDeposit.amountText.text);
        Assert.AreEqual("Annual Interest Rate: 3%", makeDeposit.rateText.text);
    }

    [Test]
    public void ShowInfo_Choose24Rounds()
    {
        makeDeposit.timeRate = new float[5] {0.01f, 0.02f, 0.03f, 0.04f, 0.05f};
        makeDeposit.OnDropdownChanged(3);
        makeDeposit.ValidateInput("1234"); 
        Assert.AreEqual("Period of the deposit: 24", makeDeposit.periodText.text);
        Assert.AreEqual("Amount of the deposit: 1234", makeDeposit.amountText.text);
        Assert.AreEqual("Annual Interest Rate: 4%", makeDeposit.rateText.text);
    }

    [Test]
    public void ShowInfo_Choose36Rounds()
    {
        makeDeposit.timeRate = new float[5] {0.01f, 0.02f, 0.03f, 0.04f, 0.05f};
        makeDeposit.OnDropdownChanged(4);
        makeDeposit.ValidateInput("123"); 
        Assert.AreEqual("Period of the deposit: 36", makeDeposit.periodText.text);
        Assert.AreEqual("Amount of the deposit: 123", makeDeposit.amountText.text);
        Assert.AreEqual("Annual Interest Rate: 5%", makeDeposit.rateText.text);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(makeDeposit.gameObject);
        Object.DestroyImmediate(playerManager.gameObject);
        Object.DestroyImmediate(bankRateManager.gameObject);
        Object.DestroyImmediate(inputFieldOb);
        Object.DestroyImmediate(warningTextOb);
        Object.DestroyImmediate(buttonOb);
        Object.DestroyImmediate(periodTextOb);
        Object.DestroyImmediate(amountTextOb);
        Object.DestroyImmediate(rateTextOb);
    }
}

